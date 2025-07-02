using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrationApi.Models;
using System.ComponentModel.DataAnnotations;

namespace RegistrationApi.Controllers
{
    /// <summary>
    /// API for managing course registrations
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RegistrationsController> _logger;

        public RegistrationsController(
            ApplicationDbContext context,
            ILogger<RegistrationsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Get all registrations with filtering and sorting
        /// </summary>
        /// <param name="search">Search term (name, course, etc.)</param>
        /// <param name="status">Filter by status (Pending/Confirmed/Cancelled)</param>
        /// <param name="sortBy">Field to sort by (personname, coursename, etc.)</param>
        /// <param name="sortOrder">Sort order (asc/desc)</param>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ViwRegistration>))]
        public async Task<ActionResult<IEnumerable<ViwRegistration>>> GetRegistrations(
            [FromQuery] string? search = null,
            [FromQuery] string? status = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? sortOrder = "asc")
        {
            try
            {
                IQueryable<ViwRegistration> registrations = _context.ViwRegistrations;

                // Search Filter
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.ToLower();
                    registrations = registrations.Where(r =>
                        (r.PName != null && r.PName.ToLower().Contains(search)) ||
                        (r.CourseName != null && r.CourseName.ToLower().Contains(search)) ||
                        (r.SPaymentMethod != null && r.SPaymentMethod.ToLower().Contains(search)) ||
                        (r.VTrxId != null && r.VTrxId.ToLower().Contains(search))
                    );
                }

                // Status Filter
                if (!string.IsNullOrEmpty(status))
                {
                    registrations = registrations.Where(r =>
                        r.SStatus != null &&
                        r.SStatus.ToLower() == status.ToLower());
                }

                // Sorting
                registrations = (sortBy?.ToLower(), sortOrder?.ToLower()) switch
                {
                    ("personname", "asc") => registrations.OrderBy(r => r.PName),
                    ("personname", "desc") => registrations.OrderByDescending(r => r.PName),
                    ("coursename", "asc") => registrations.OrderBy(r => r.CourseName),
                    ("coursename", "desc") => registrations.OrderByDescending(r => r.CourseName),
                    ("registrationdate", "asc") => registrations.OrderBy(r => r.DRegistrationDate),
                    ("registrationdate", "desc") => registrations.OrderByDescending(r => r.DRegistrationDate),
                    ("status", "asc") => registrations.OrderBy(r => r.SStatus),
                    ("status", "desc") => registrations.OrderByDescending(r => r.SStatus),
                    (_, "desc") => registrations.OrderByDescending(r => r.Sl),
                    _ => registrations.OrderBy(r => r.Sl)
                };

                return await registrations.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving registrations");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get a specific registration by ID
        /// </summary>
        /// <param name="id">Registration ID</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ViwRegistration))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ViwRegistration>> GetRegistration(int id)
        {
            try
            {
                var registration = await _context.ViwRegistrations
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.Sl == id);

                if (registration == null)
                {
                    return NotFound();
                }

                return registration;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving registration {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Create a new registration
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ViwRegistration))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ViwRegistration>> PostRegistration(
            [FromBody] RegistrationDto registrationDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Validate model
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Check if person exists or create new
                var person = await _context.Personeels
                    .FirstOrDefaultAsync(p => p.EMail == registrationDto.EMail);

                if (person == null)
                {
                    person = new Personeel
                    {
                        PName = registrationDto.PName,
                        Phone = registrationDto.Phone,
                        Address = registrationDto.Address,
                        EMail = registrationDto.EMail,
                        PType = registrationDto.PType,
                        Organization = registrationDto.Organization,
                        Designation = registrationDto.Designation,
                        City = registrationDto.City,
                        PostalCode = registrationDto.PostalCode,
                        EntryDate = DateTime.UtcNow
                    };
                    _context.Personeels.Add(person);
                    await _context.SaveChangesAsync();
                }

                // Create registration record
                var registration = new TblRegistration
                {
                    IPersoneelSl = person.Sl,
                    IAnnouncementSl = registrationDto.AnnouncementId,
                    DRegistrationDate = DateTime.UtcNow,
                    VCategory = registrationDto.Category,
                    IFees = registrationDto.Fees,
                    SPaymentMethod = registrationDto.PaymentMethod,
                    SStatus = "Pending",
                    VPaymentType = registrationDto.PaymentType,
                    VEntryBy = "API"
                };

                _context.TblRegistrations.Add(registration);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Return complete view data
                var result = await _context.ViwRegistrations
                    .FirstOrDefaultAsync(r => r.Sl == registration.Sl);

                return CreatedAtAction(
                    nameof(GetRegistration),
                    new { id = registration.Sl },
                    result);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error creating registration");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Confirm payment for a registration
        /// </summary>
        /// <param name="id">Registration ID</param>
        [HttpPut("ConfirmPayment/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ConfirmPayment(
            int id,
            [FromBody] PaymentConfirmationDto confirmation)
        {
            try
            {
                var registration = await _context.TblRegistrations.FindAsync(id);
                if (registration == null)
                {
                    return NotFound();
                }

                if (string.IsNullOrEmpty(confirmation.PaymentMethod) ||
                    string.IsNullOrEmpty(confirmation.TransactionId))
                {
                    return BadRequest("Payment method and transaction ID are required");
                }

                registration.SStatus = "Confirmed";
                registration.SPaymentMethod = confirmation.PaymentMethod;
                registration.VTrxId = confirmation.TransactionId;
                registration.VPaymentType = confirmation.PaymentType;
                registration.VEntryBy = "API-Confirmed";

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!RegistrationExists(id))
                {
                    return NotFound();
                }
                _logger.LogError(ex, $"Concurrency error confirming payment for registration {id}");
                return StatusCode(500, "Internal server error");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error confirming payment for registration {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Cancel a registration (soft delete)
        /// </summary>
        /// <param name="id">Registration ID</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRegistration(int id)
        {
            try
            {
                var registration = await _context.TblRegistrations.FindAsync(id);
                if (registration == null)
                {
                    return NotFound();
                }

                registration.SStatus = "Cancelled";
                registration.VEntryBy = "API-Cancelled";
                registration.DRegistrationDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error cancelling registration {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        private bool RegistrationExists(int id)
        {
            return _context.TblRegistrations.Any(e => e.Sl == id);
        }
    }

    /// <summary>
    /// Payment confirmation data
    /// </summary>
    public class PaymentConfirmationDto
    {
        /// <summary>
        /// Payment method (Credit Card, Bank Transfer, etc.)
        /// </summary>
        [Required]
        public string PaymentMethod { get; set; } = null!;

        /// <summary>
        /// Unique transaction ID
        /// </summary>
        [Required]
        public string TransactionId { get; set; } = null!;

        /// <summary>
        /// Payment type (Online, Offline)
        /// </summary>
        public string? PaymentType { get; set; }
    }

    /// <summary>
    /// Registration data
    /// </summary>
    public class RegistrationDto
    {
        /// <summary>
        /// Person's full name
        /// </summary>
        [Required]
        public string PName { get; set; } = null!;

        /// <summary>
        /// Contact phone number
        /// </summary>
        [Required]
        public string Phone { get; set; } = null!;

        /// <summary>
        /// Full address
        /// </summary>
        [Required]
        public string Address { get; set; } = null!;

        /// <summary>
        /// Email address
        /// </summary>
        [Required, EmailAddress]
        public string EMail { get; set; } = null!;

        /// <summary>
        /// Person type (Student, Professional, etc.)
        /// </summary>
        [Required]
        public string PType { get; set; } = null!;

        /// <summary>
        /// Organization/Company
        /// </summary>
        public string? Organization { get; set; }

        /// <summary>
        /// Job designation
        /// </summary>
        public string? Designation { get; set; }

        /// <summary>
        /// City
        /// </summary>
        public string? City { get; set; }

        /// <summary>
        /// Postal/Zip code
        /// </summary>
        public string? PostalCode { get; set; }

        /// <summary>
        /// Announcement ID
        /// </summary>
        [Required]
        public int AnnouncementId { get; set; }

        /// <summary>
        /// Registration category
        /// </summary>
        [Required]
        public string Category { get; set; } = null!;

        /// <summary>
        /// Fees amount
        /// </summary>
        [Required]
        public int Fees { get; set; }

        /// <summary>
        /// Payment method
        /// </summary>
        [Required]
        public string PaymentMethod { get; set; } = null!;

        /// <summary>
        /// Payment type
        /// </summary>
        public string? PaymentType { get; set; }
    }
}
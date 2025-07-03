using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrationApi.Models;
using System.ComponentModel.DataAnnotations;

namespace RegistrationApi.Controllers
{
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

    public class PaymentConfirmationDto
    {

        [Required]
        public string PaymentMethod { get; set; } = null!;

        [Required]
        public string TransactionId { get; set; } = null!;

        public string? PaymentType { get; set; }
    }

    public class RegistrationDto
    {
        [Required]
        public string PName { get; set; } = null!;

        [Required]
        public string Phone { get; set; } = null!;

        [Required]
        public string Address { get; set; } = null!;

        [Required, EmailAddress]
        public string EMail { get; set; } = null!;

        [Required]
        public string PType { get; set; } = null!;

        public string? Organization { get; set; }

        public string? Designation { get; set; }

        public string? City { get; set; }

        public string? PostalCode { get; set; }

        [Required]
        public int AnnouncementId { get; set; }

        [Required]
        public string Category { get; set; } = null!;

        [Required]
        public int Fees { get; set; }

        [Required]
        public string PaymentMethod { get; set; } = null!;

        public string? PaymentType { get; set; }
    }
}
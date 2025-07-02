import { Component, OnInit } from '@angular/core';
import { RegistrationService } from '../../services/registration';
import { ViwRegistration } from '../../models/registration.model';
import { Router, RouterModule } from '@angular/router'; // Import RouterModule
import { CommonModule } from '@angular/common'; // Import CommonModule
import { FormsModule } from '@angular/forms'; // Import FormsModule

@Component({
  selector: 'app-registration-list',
  standalone: true, // You might need to add this if it's a standalone component
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './registration-list.html',
  styleUrls: ['./registration-list.css'],
})
export class RegistrationListComponent implements OnInit {
  registrations: ViwRegistration[] = [];
  searchQuery: string = '';
  statusFilter: string = '';
  sortBy: string = 'Sl'; // Default sort by Sl
  sortOrder: string = 'asc'; // Default sort order

  constructor(
    private registrationService: RegistrationService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadRegistrations();
  }

  loadRegistrations(): void {
    this.registrationService
      .getRegistrations(
        this.searchQuery,
        this.statusFilter,
        this.sortBy,
        this.sortOrder
      )
      .subscribe({
        next: (data) => {
          this.registrations = data;
        },
        error: (error) => {
          console.error('Error fetching registrations:', error);
          // Handle error, e.g., show a message to the user
        },
      });
  }

  applyFiltersAndSort(): void {
    this.loadRegistrations();
  }

  viewDetails(id: number): void {
    this.router.navigate(['/registrations', id]);
  }

  confirmPayment(id: number): void {
    this.router.navigate(['/confirm-payment', id]);
  }

  cancelRegistration(id: number): void {
    if (confirm('Are you sure you want to cancel this registration?')) {
      this.registrationService.cancelRegistration(id).subscribe({
        next: () => {
          console.log(`Registration ${id} cancelled successfully.`);
          this.loadRegistrations(); // Refresh the list
        },
        error: (error) => {
          console.error(`Error cancelling registration ${id}:`, error);
          alert('Failed to cancel registration.');
        },
      });
    }
  }

  createNewRegistration(): void {
    this.router.navigate(['/create-registration']);
  }
}

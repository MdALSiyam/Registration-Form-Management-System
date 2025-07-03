import { Component, OnInit } from '@angular/core';
import { RegistrationService } from '../../services/registration';
import { ViwRegistration } from '../../models/registration.model';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-registration-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './registration-list.html',
  styleUrls: ['./registration-list.css'],
})
export class RegistrationListComponent implements OnInit {
  registrations: ViwRegistration[] = [];
  searchQuery: string = '';
  statusFilter: string = '';
  sortBy: string = 'Sl';
  sortOrder: string = 'asc';

  totalRegistrations: number = 0;
  pendingRegistrations: number = 0;
  confirmedRegistrations: number = 0;
  cancelledRegistrations: number = 0;

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
          this.updateCardData();
        },
        error: (error) => {
          console.error('Error fetching registrations:', error);
          this.totalRegistrations = 0;
          this.pendingRegistrations = 0;
          this.confirmedRegistrations = 0;
          this.cancelledRegistrations = 0;
        },
      });
  }

  updateCardData(): void {
    this.totalRegistrations = this.registrations.length;
    this.pendingRegistrations = this.registrations.filter(
      (reg) => reg.sStatus === 'Pending'
    ).length;
    this.confirmedRegistrations = this.registrations.filter(
      (reg) => reg.sStatus === 'Confirmed'
    ).length;
    this.cancelledRegistrations = this.registrations.filter(
      (reg) => reg.sStatus === 'Cancelled'
    ).length;
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
          this.loadRegistrations(); // Refresh the list and card data
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

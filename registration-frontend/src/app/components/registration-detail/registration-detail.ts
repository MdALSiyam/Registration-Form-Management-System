import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { RegistrationService } from '../../services/registration';
import { ViwRegistration } from '../../models/registration.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-registration-detail',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './registration-detail.html',
  styleUrls: ['./registration-detail.css'],
})
export class RegistrationDetailComponent implements OnInit {
  registration: ViwRegistration | undefined;
  id: number = 0;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private registrationService: RegistrationService
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      const idParam = params.get('id');
      if (idParam) {
        this.id = +idParam;
        this.loadRegistrationDetails(this.id);
      }
    });
  }

  loadRegistrationDetails(id: number): void {
    this.registrationService.getRegistration(id).subscribe({
      next: (data) => {
        this.registration = data;
      },
      error: (error) => {
        console.error('Error fetching registration details:', error);
        // Navigate back or show an error message
        this.router.navigate(['/registrations']);
      },
    });
  }

  goBack(): void {
    this.router.navigate(['/registrations']);
  }
}

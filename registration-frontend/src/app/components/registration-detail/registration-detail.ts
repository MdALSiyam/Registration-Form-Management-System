import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router'; // RouterModule if you use routerLink
import { RegistrationService } from '../../services/registration';
import { ViwRegistration } from '../../models/registration.model';
import { CommonModule } from '@angular/common'; // **ADD THIS for *ngIf**

@Component({
  selector: 'app-registration-detail',
  standalone: true, // **ADD THIS**
  imports: [
    CommonModule, // **ADD THIS for *ngIf to handle 'undefined' properties**
    RouterModule, // Include if you use routerLink in the template
  ],
  templateUrl: './registration-detail.html', // Corrected extension based on Screenshot_33.png
  styleUrls: ['./registration-detail.css'], // Corrected extension
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

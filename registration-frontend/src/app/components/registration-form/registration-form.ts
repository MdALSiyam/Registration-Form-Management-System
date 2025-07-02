import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms'; // ReactiveFormsModule needed for reactive forms
import { RegistrationService } from '../../services/registration';
import { Router, RouterModule } from '@angular/router'; // RouterModule needed if using routerLink directly in template
import { CommonModule } from '@angular/common'; // CommonModule needed for common directives like *ngIf, *ngFor

@Component({
  selector: 'app-registration-form',
  standalone: true, // **CRITICAL: Add this line**
  imports: [
    CommonModule, // For directives like *ngIf, *ngFor
    ReactiveFormsModule, // For using FormGroup, FormBuilder, etc.
    RouterModule, // For routerLink and Router functionality
  ],
  templateUrl: './registration-form.html', // **FIXED: Added .html extension**
  styleUrls: ['./registration-form.css'], // **FIXED: Added .css extension**
})
export class RegistrationFormComponent implements OnInit {
  registrationForm: FormGroup;
  loading: boolean = false;
  successMessage: string = '';
  errorMessage: string = '';

  constructor(
    private fb: FormBuilder,
    private registrationService: RegistrationService,
    private router: Router
  ) {
    this.registrationForm = this.fb.group({
      pName: ['', Validators.required],
      phone: ['', Validators.required],
      address: ['', Validators.required],
      eMail: ['', [Validators.required, Validators.email]],
      pType: ['', Validators.required],
      organization: [''],
      designation: [''],
      city: [''],
      postalCode: [''],
      announcementId: [null, [Validators.required, Validators.min(1)]],
      category: ['', Validators.required],
      fees: [null, [Validators.required, Validators.min(0)]],
      paymentMethod: ['', Validators.required],
      paymentType: ['Online'],
    });
  }

  ngOnInit(): void {}

  onSubmit(): void {
    if (this.registrationForm.valid) {
      this.loading = true;
      this.errorMessage = '';
      this.successMessage = '';

      this.registrationService
        .createRegistration(this.registrationForm.value)
        .subscribe({
          next: (response) => {
            this.successMessage = 'Registration created successfully!';
            this.loading = false;
            this.registrationForm.reset();
            this.router.navigate(['/registrations', response.sl]);
          },
          error: (error) => {
            this.errorMessage =
              'Failed to create registration. Please try again.';
            console.error('Error creating registration:', error);
            this.loading = false;
          },
        });
    } else {
      this.errorMessage = 'Please fill in all required fields correctly.';
      this.markAllAsTouched(this.registrationForm);
    }
  }

  private markAllAsTouched(formGroup: FormGroup): void {
    Object.values(formGroup.controls).forEach((control) => {
      control.markAsTouched();
      if (control instanceof FormGroup) {
        this.markAllAsTouched(control);
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/registrations']);
  }
}

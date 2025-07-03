import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { RegistrationService } from '../../services/registration';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-registration-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './registration-form.html',
  styleUrls: ['./registration-form.css'],
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

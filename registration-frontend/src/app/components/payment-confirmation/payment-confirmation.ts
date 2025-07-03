import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms'; // ReactiveFormsModule for forms
import { RegistrationService } from '../../services/registration';
import { ViwRegistration } from '../../models/registration.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-payment-confirmation',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './payment-confirmation.html',
  styleUrls: ['./payment-confirmation.css'],
})
export class PaymentConfirmationComponent implements OnInit {
  registrationId: number = 0;
  registration: ViwRegistration | undefined;
  paymentForm: FormGroup;
  loading: boolean = false;
  successMessage: string = '';
  errorMessage: string = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private registrationService: RegistrationService
  ) {
    this.paymentForm = this.fb.group({
      paymentMethod: ['', Validators.required],
      transactionId: ['', Validators.required],
      paymentType: ['Online'], // Default value
    });
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      const idParam = params.get('id');
      if (idParam) {
        this.registrationId = +idParam;
        this.loadRegistrationDetails(this.registrationId);
      } else {
        this.router.navigate(['/registrations']);
      }
    });
  }

  loadRegistrationDetails(id: number): void {
    this.registrationService.getRegistration(id).subscribe({
      next: (data) => {
        this.registration = data;
        // Pre-fill form if registration has existing payment data (optional)
        if (data.sPaymentMethod) {
          this.paymentForm.patchValue({
            paymentMethod: data.sPaymentMethod,
            transactionId: data.vTrxId,
            paymentType: data.vPaymentType || 'Online',
          });
        }
      },
      error: (error) => {
        console.error(
          'Error fetching registration details for payment confirmation:',
          error
        );
        this.errorMessage = 'Could not load registration details.';
        this.router.navigate(['/registrations']);
      },
    });
  }

  onSubmit(): void {
    if (this.paymentForm.valid && this.registrationId > 0) {
      this.loading = true;
      this.errorMessage = '';
      this.successMessage = '';

      this.registrationService
        .confirmPayment(this.registrationId, this.paymentForm.value)
        .subscribe({
          next: () => {
            this.successMessage = 'Payment confirmed successfully!';
            this.loading = false;
            // Optionally navigate back to the detail page or list
            this.router.navigate(['/registrations', this.registrationId]);
          },
          error: (error) => {
            this.errorMessage = 'Failed to confirm payment. Please try again.';
            console.error('Error confirming payment:', error);
            this.loading = false;
          },
        });
    } else {
      this.errorMessage = 'Please fill in all required fields correctly.';
      this.markAllAsTouched(this.paymentForm);
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
    this.router.navigate(['/registrations', this.registrationId]);
  }
}

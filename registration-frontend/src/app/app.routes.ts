import { Routes } from '@angular/router';
import { PaymentConfirmationComponent } from './components/payment-confirmation/payment-confirmation';
import { RegistrationFormComponent } from './components/registration-form/registration-form';
import { RegistrationDetailComponent } from './components/registration-detail/registration-detail';
import { RegistrationListComponent } from './components/registration-list/registration-list';

export const routes: Routes = [
  { path: '', redirectTo: 'registrations', pathMatch: 'full' },
  { path: 'registrations', component: RegistrationListComponent },
  { path: 'registrations/:id', component: RegistrationDetailComponent },
  { path: 'create-registration', component: RegistrationFormComponent },
  { path: 'confirm-payment/:id', component: PaymentConfirmationComponent },
  { path: '**', redirectTo: 'registrations' },
];

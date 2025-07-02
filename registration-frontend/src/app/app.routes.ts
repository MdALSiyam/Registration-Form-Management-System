import { Routes } from '@angular/router';
import { PaymentConfirmationComponent } from './components/payment-confirmation/payment-confirmation';
import { RegistrationFormComponent } from './components/registration-form/registration-form';
import { RegistrationDetailComponent } from './components/registration-detail/registration-detail';
import { RegistrationListComponent } from './components/registration-list/registration-list';

// Import your components for route configuration

export const routes: Routes = [
  // **Added 'export' here**
  { path: '', redirectTo: 'registrations', pathMatch: 'full' }, // Example: Redirect to list on root
  { path: 'registrations', component: RegistrationListComponent },
  { path: 'registrations/:id', component: RegistrationDetailComponent },
  { path: 'create-registration', component: RegistrationFormComponent },
  { path: 'confirm-payment/:id', component: PaymentConfirmationComponent },
  // Add other routes as needed
  { path: '**', redirectTo: 'registrations' }, // Wildcard route for 404 or redirect
];

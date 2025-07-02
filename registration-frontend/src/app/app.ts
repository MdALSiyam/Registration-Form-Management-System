import { Component } from '@angular/core';
import { CommonModule } from '@angular/common'; // Needed for *ngIf, *ngFor in templates
import { RouterOutlet, RouterModule } from '@angular/router'; // RouterOutlet to render routes, RouterModule for routerLink
import { HttpClientModule } from '@angular/common/http'; // For making HTTP requests (used by services)
import { FormsModule, ReactiveFormsModule } from '@angular/forms'; // For template-driven and reactive forms

@Component({
  selector: 'app-root', // Make sure this matches your index.html <app-root> tag
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet, // Essential for routing to work
    RouterModule, // Provides routerLink directive
    HttpClientModule, // Needed if your app uses HttpClient directly or indirectly via services
    FormsModule, // For ngModel in templates
    ReactiveFormsModule, // For reactive forms
    // Add other standalone components here ONLY if they are directly used in app.ts's template
    // e.g., RegistrationListComponent (if not lazy-loaded via routes)
  ],
  templateUrl: './app.html', // **FIXED: Changed to app.html** [cite: 26]
  styleUrls: ['./app.css'], // **FIXED: Changed to app.css** [cite: 26]
})
export class AppComponent {
  // Your main application logic
}

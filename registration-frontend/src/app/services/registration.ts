import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import {
  ViwRegistration,
  RegistrationDto,
  PaymentConfirmationDto,
} from '../models/registration.model';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class RegistrationService {
  private apiUrl = `${environment.apiUrl}/Registrations`;

  constructor(private http: HttpClient) {}

  getRegistrations(
    search?: string,
    status?: string,
    sortBy?: string,
    sortOrder?: string
  ): Observable<ViwRegistration[]> {
    let params = new HttpParams();
    if (search) params = params.set('search', search);
    if (status) params = params.set('status', status);
    if (sortBy) params = params.set('sortBy', sortBy);
    if (sortOrder) params = params.set('sortOrder', sortOrder);

    return this.http.get<ViwRegistration[]>(this.apiUrl, { params });
  }

  getRegistration(id: number): Observable<ViwRegistration> {
    return this.http.get<ViwRegistration>(`${this.apiUrl}/${id}`).pipe(
      catchError((error) => {
        console.error(`Failed to load registration ${id}:`, error);
        return throwError(() => new Error('Registration not found'));
      })
    );
  }

  createRegistration(
    registration: RegistrationDto
  ): Observable<ViwRegistration> {
    return this.http.post<ViwRegistration>(this.apiUrl, registration);
  }

  confirmPayment(
    id: number,
    confirmation: PaymentConfirmationDto
  ): Observable<any> {
    return this.http.put(`${this.apiUrl}/ConfirmPayment/${id}`, confirmation);
  }

  cancelRegistration(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}

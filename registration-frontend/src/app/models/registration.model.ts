export interface ViwRegistration {
  sl: number;
  iPersoneelSl?: number;
  iAnnouncementSl?: number;
  pName?: string;
  phone?: string;
  address?: string;
  eMail?: string;
  pType?: string;
  description?: string;
  qualification?: string;
  organization?: string;
  designation?: string;
  city?: string;
  postalCode?: string;
  courseSl?: number;
  courseName?: string;
  courseType?: string;
  dRegistrationDate?: Date;
  iFees?: number;
  sPaymentMethod?: string;
  vCategory?: string;
  sStatus?: string;
  vPaymentType?: string;
  vTrxId?: string;
  vEntryBy?: string;
}

export interface RegistrationDto {
  pName: string;
  phone: string;
  address: string;
  eMail: string;
  pType: string;
  organization?: string;
  designation?: string;
  city?: string;
  postalCode?: string;
  announcementId: number;
  category: string;
  fees: number;
  paymentMethod: string;
  paymentType?: string;
}

export interface PaymentConfirmationDto {
  paymentMethod: string;
  transactionId: string;
  paymentType?: string;
}

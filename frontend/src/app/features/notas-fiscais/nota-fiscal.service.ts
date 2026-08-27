import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface InvoiceItem {
  productCode: string;
  quantity: number;
}

export interface InvoiceCreateRequest {
  items: InvoiceItem[];
}

export interface InvoiceResponse {
  sequentialNumber: string;
  items: InvoiceItem[];
  status: string;
  createdAt: string;
}

@Injectable({
  providedIn: 'root'
})
export class NotaFiscalService {
  private apiUrl = `${environment.billingApiUrl}/api/Invoices`;

  constructor(private http: HttpClient) {}

  getInvoices(): Observable<InvoiceResponse[]> {
    return this.http.get<InvoiceResponse[]>(this.apiUrl);
  }

  createInvoice(request: InvoiceCreateRequest): Observable<InvoiceResponse> {
    return this.http.post<InvoiceResponse>(this.apiUrl, request);
  }

  printInvoice(sequentialNumber: string): Observable<void> {
    const url = `${this.apiUrl}/${sequentialNumber}/print`;
    return this.http.post<void>(url, {});
  }
}
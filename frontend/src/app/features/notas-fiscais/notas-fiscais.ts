import { Component, OnInit,ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormArray, Validators, ReactiveFormsModule } from '@angular/forms';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzTooltipModule } from 'ng-zorro-antd/tooltip';
import { NzDrawerModule } from 'ng-zorro-antd/drawer';
import { NzDescriptionsModule } from 'ng-zorro-antd/descriptions';

import { NotaFiscalService, InvoiceResponse, InvoiceCreateRequest } from './nota-fiscal.service';
import { InvoiceTableComponent } from './components/nota-table/nota-table';
import { NotaDrawerComponent } from './components/nota-drawer/nota-drawer';
import { NotaModalComponent } from './components/nota-modal/nota-modal';

@Component({
  selector: 'app-notas-fiscais',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NzTableModule,
    NzButtonModule,
    NzTagModule,
    NzIconModule,
    NzModalModule,
    NzFormModule,
    NzInputModule,
    NzDrawerModule,
    NzDescriptionsModule,
    NzTooltipModule,
    NzInputNumberModule,
    InvoiceTableComponent,
    NotaDrawerComponent,
    NotaModalComponent
  ],
  templateUrl: './notas-fiscais.html',
  styleUrl: './notas-fiscais.css'
})
export class NotasFiscaisComponent implements OnInit {
  notas: InvoiceResponse[] = [];
  isLoadingTable = false;
  isVisible = false;
  isOkLoading = false;
  notaForm: FormGroup;
  drawerVisible = false;
  noteDetails: InvoiceResponse | null = null;

  constructor(
    private fb: FormBuilder,
    private notaFiscalService: NotaFiscalService,
    private message: NzMessageService,
    private cdr: ChangeDetectorRef
  ) {
    this.notaForm = this.fb.group({
      items: this.fb.array([])
    });
    this.addItem();
  }

  ngOnInit(): void {
    this.loadInvoices();
  }

  get itemsFormArray(): FormArray {
    return this.notaForm.get('items') as FormArray;
  }

  addItem(): void {
    const itemGroup = this.fb.group({
      productCode: [null, [Validators.required]],
      quantity: [1, [Validators.required, Validators.min(1)]]
    });
    this.itemsFormArray.push(itemGroup);
  }

  removeItem(index: number): void {
    if (this.itemsFormArray.length > 1) {
      this.itemsFormArray.removeAt(index);
    }
  }

  loadInvoices(): void {
    this.isLoadingTable = true;
    this.notaFiscalService.getInvoices().subscribe({
      next: (data) => {
        this.notas = data;
        this.isLoadingTable = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error(err);
        const msg = err.error?.detail || err.error?.title || err.error?.message || 'Erro ao carregar as notas fiscais (Serviço indisponível).';
        this.message.error(msg);
        this.isLoadingTable = false;
        this.cdr.detectChanges();
      }
    });
  }

  showModal(): void {
    this.isVisible = true;
  }

  handleCancel(): void {
    this.isVisible = false;
    this.resetForm();
  }

  handleOk(): void {
    if (this.notaForm.valid) {
      this.isOkLoading = true;
      const request: InvoiceCreateRequest = this.notaForm.value;

      this.notaFiscalService.createInvoice(request).subscribe({
        next: (response) => {
          this.notas = [response, ...this.notas];
          this.message.success(`Nota #${response.sequentialNumber} emitida com sucesso!`);
          this.isOkLoading = false;
          this.isVisible = false;
          this.resetForm();
        },
        error: (err) => {
          console.error(err);
          const msg = err.error?.detail || err.error?.title || err.error?.message || typeof err.error === 'string' ? err.error : 'Erro ao emitir a nota fiscal.';
          this.message.error(msg);
          this.isOkLoading = false;
        }
      });
    } else {
      Object.values(this.itemsFormArray.controls).forEach((control: any) => {
        Object.values(control.controls).forEach((innerControl: any) => {
          innerControl.markAsDirty();
          innerControl.updateValueAndValidity({ onlySelf: true });
        });
      });
    }
  }

  resetForm(): void {
    this.notaForm.reset();
    this.itemsFormArray.clear();
    this.addItem(); 
  }

  printNote(numero: string): void {
    const idMensagem = this.message.loading(`Enviando nota #${numero} para impressão...`, { nzDuration: 0 }).messageId;
    this.notaFiscalService.printInvoice(numero).subscribe({
      next: () => {
        this.message.remove(idMensagem);
        this.message.success(`Nota #${numero} impressa com sucesso!`);
        
        const notaIndex = this.notas.findIndex(n => n.sequentialNumber === numero);
        if (notaIndex !== -1) {
          this.notas[notaIndex].status = 'Fechada';
          this.notas = [...this.notas];
          this.cdr.detectChanges();
        }
      },
      error: (err) => {
        console.error('Erro na impressão:', err);
        this.message.remove(idMensagem);
        
        const msg = err.error?.detail || err.error?.title || err.error?.message || (typeof err.error === 'string' ? err.error : `Falha ao tentar imprimir a nota #${numero}.`);
        this.message.error(msg);
      }
    });
  }

  viewNote(numero: string): void {
    const note = this.notas.find(n => n.sequentialNumber === numero);
    if (note) {
      this.noteDetails = note;
      this.drawerVisible = true;
    }
  }

  closeDrawer(): void {
    this.drawerVisible = false;
    setTimeout(() => this.noteDetails = null, 300);
  }
}
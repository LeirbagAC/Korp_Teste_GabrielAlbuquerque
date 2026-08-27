import { Component, OnInit } from '@angular/core';
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
import { NotaFiscalService, InvoiceResponse, InvoiceCreateRequest } from './nota-fiscal.service';

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
    NzInputNumberModule
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

  constructor(
    private fb: FormBuilder,
    private notaFiscalService: NotaFiscalService,
    private message: NzMessageService
  ) {
    this.notaForm = this.fb.group({
      items: this.fb.array([])
    });
    this.addItem(); 
  }

  ngOnInit(): void {
    this.carregarNotas();
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

  carregarNotas(): void {
    this.isLoadingTable = true;
    this.notaFiscalService.getInvoices().subscribe({
      next: (data) => {
        this.notas = data;
        this.isLoadingTable = false;
      },
      error: (err) => {
        console.error(err);
        this.message.error('Erro ao carregar as notas fiscais.');
        this.isLoadingTable = false;
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
          this.message.error('Erro ao emitir a nota fiscal.');
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

  imprimirNota(numero: string): void {
    this.message.success(`Nota #${numero} enviada para impressão!`);
  }

  visualizarNota(numero: string): void {
    this.message.info(`Visualizando detalhes da nota #${numero}`);
  }
}
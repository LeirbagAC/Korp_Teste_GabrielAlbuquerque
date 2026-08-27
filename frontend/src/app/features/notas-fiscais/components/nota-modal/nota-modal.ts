import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormGroup, FormArray, ReactiveFormsModule } from '@angular/forms';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';

@Component({
  selector: 'app-nota-modal',
  standalone: true,
  imports: [
    CommonModule, ReactiveFormsModule, NzModalModule, NzFormModule,
    NzInputModule, NzInputNumberModule, NzButtonModule, NzIconModule
  ],
  templateUrl: './nota-modal.html'
})
export class NotaModalComponent {
  @Input() isVisible = false;
  @Input() isOkLoading = false;
  @Input() notaForm!: FormGroup;

  @Output() cancel = new EventEmitter<void>();
  @Output() ok = new EventEmitter<void>();
  @Output() add = new EventEmitter<void>();
  @Output() remove = new EventEmitter<number>();

  get itemsFormArray(): FormArray {
    return this.notaForm.get('items') as FormArray;
  }
}
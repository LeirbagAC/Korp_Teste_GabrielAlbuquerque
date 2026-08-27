import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';

@Component({
  selector: 'app-produto-modal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NzModalModule, NzFormModule, NzInputModule, NzInputNumberModule],
  templateUrl: './produto-modal.html'
})
export class ProdutoModalComponent {
  @Input() isVisible = false;
  @Input() isOkLoading = false;
  @Input() produtoForm!: FormGroup;

  @Output() cancel = new EventEmitter<void>();
  @Output() ok = new EventEmitter<void>();
}
import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzTooltipModule } from 'ng-zorro-antd/tooltip';
import { InvoiceResponse } from '../../nota-fiscal.service';

@Component({
  selector: 'app-invoice-table',
  standalone: true,
  imports: [CommonModule, NzTableModule, NzButtonModule, NzTagModule, NzIconModule, NzTooltipModule],
  templateUrl: './nota-table.html'
})
export class InvoiceTableComponent {
  @Input() notas: InvoiceResponse[] = [];
  @Input() isLoading = false;

  @Output() visualizar = new EventEmitter<string>();
  @Output() imprimir = new EventEmitter<string>();
}
import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzDrawerModule } from 'ng-zorro-antd/drawer';
import { NzDescriptionsModule } from 'ng-zorro-antd/descriptions';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { InvoiceResponse } from '../../nota-fiscal.service';

@Component({
  selector: 'app-nota-drawer',
  standalone: true,
  imports: [CommonModule, NzDrawerModule, NzDescriptionsModule, NzTableModule, NzTagModule],
  templateUrl: './nota-drawer.html'
})
export class NotaDrawerComponent {
  @Input() visible = false;
  @Input() nota: InvoiceResponse | null = null;

  @Output() closeDrawer = new EventEmitter<void>();

  onClose(): void {
    this.closeDrawer.emit();
  }
}


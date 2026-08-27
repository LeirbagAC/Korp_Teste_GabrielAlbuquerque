import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzTagModule } from 'ng-zorro-antd/tag';

@Component({
  selector: 'app-produto-table',
  standalone: true,
  imports: [CommonModule, NzTableModule, NzTagModule],
  templateUrl: './produto-table.html',
  styleUrl: './produto-table.css'
})
export class ProdutoTableComponent {
  @Input() produtos: any[] = [];
  @Input() isLoading = false;
}
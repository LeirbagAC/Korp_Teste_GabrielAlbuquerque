import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzMessageService } from 'ng-zorro-antd/message';
import { ProdutoService, ProductRequest } from './produto.service';
import { Component, OnInit, inject, DestroyRef, ChangeDetectorRef } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { finalize } from 'rxjs';

interface ProdutoTabela {
  codigo: string;
  descricao: string;
  saldo: number;
}

@Component({
  selector: 'app-produtos',
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
  templateUrl: './produtos.html',
  styleUrl: './produtos.css'
})
export class ProdutosComponent implements OnInit {
  produtos: ProdutoTabela[] = [];
  isLoadingTable = false;
  private destroyRef = inject(DestroyRef);

  isVisible = false;
  isOkLoading = false;
  produtoForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private produtoService: ProdutoService,
    private message: NzMessageService,
    private cdr: ChangeDetectorRef
  ) {
    this.produtoForm = this.fb.group({
      productName: [null, [Validators.required]],
      quantity: [0, [Validators.required, Validators.min(0)]]
    });
  }

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.isLoadingTable = true;
    
    this.produtoService.getProducts()
      .pipe(
        takeUntilDestroyed(this.destroyRef),
        finalize(() => {
          this.isLoadingTable = false;
          this.cdr.detectChanges(); //Para garantir que a tabela seja carregada
        })
      )
      .subscribe({
        next: (data) => {
          this.produtos = data.map(item => ({
            codigo: item.code,
            descricao: item.productName,
            saldo: item.quantity
          }));
          
          this.cdr.detectChanges(); 
        },
        error: (err) => {
          console.error('Falha na requisição:', err);
          this.message.error('Erro ao carregar a lista de produtos.');
        }
      });
  }

  showModal(): void {
    this.isVisible = true;
  }

  handleCancel(): void {
    this.isVisible = false;
    this.produtoForm.reset({ quantity: 0 });
  }

  handleOk(): void {
    if (this.produtoForm.valid) {
      this.isOkLoading = true;
      const request: ProductRequest = this.produtoForm.value;

      this.produtoService.createProduct(request).subscribe({
        next: (response) => {
          this.produtos = [
            ...this.produtos, 
            { 
              codigo: response.code, 
              descricao: response.productName, 
              saldo: response.quantity 
            }
          ];

          this.message.success('Produto cadastrado com sucesso!');
          this.isOkLoading = false;
          this.isVisible = false;
          this.produtoForm.reset({ quantity: 0 });
        },
        error: (err) => {
          console.error(err);
          this.message.error('Erro ao cadastrar o produto na API.');
          this.isOkLoading = false;
        }
      });
    } else {
      Object.values(this.produtoForm.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }
}
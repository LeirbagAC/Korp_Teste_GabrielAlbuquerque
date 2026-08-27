import { Routes } from '@angular/router';
import { LayoutComponent } from './core/layout/layout';
import { ProdutosComponent } from './features/produtos/produtos';
import { NotasFiscaisComponent } from './features/notas-fiscais/notas-fiscais';

export const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      { path: 'produtos', component: ProdutosComponent },
      { path: 'notas-fiscais', component: NotasFiscaisComponent },
      { path: '', redirectTo: 'produtos', pathMatch: 'full' }
    ]
  }
];
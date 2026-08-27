import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { Sidebar } from '../sidebar/sidebar';
import { Topbar } from '../topbar/topbar';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [RouterOutlet, NzLayoutModule, Sidebar, Topbar],
  templateUrl: './layout.html',
  styleUrl: './layout.css'
})
export class LayoutComponent {}
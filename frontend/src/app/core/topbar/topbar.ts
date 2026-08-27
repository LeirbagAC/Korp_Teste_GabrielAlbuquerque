import { Component } from '@angular/core';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { NzBadgeModule } from 'ng-zorro-antd/badge';

@Component({
  selector: 'app-topbar',
  standalone: true,
  imports: [NzInputModule, NzIconModule, NzAvatarModule, NzBadgeModule],
  templateUrl: './topbar.html',
  styleUrl: './topbar.css'
})
export class Topbar {}
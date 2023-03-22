import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { SidebarModule } from 'primeng/sidebar';

import { HelpPageComponent } from './help-page.component';


@NgModule({
  imports: [
    CommonModule,
    SidebarModule
  ],
  declarations: [
    HelpPageComponent
  ],
  exports: [
    HelpPageComponent
  ]
})
export class HelpPageModule {}

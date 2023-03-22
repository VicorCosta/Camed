import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { HeaderPageComponent } from './header-page.component';


@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [
    HeaderPageComponent
  ],
  exports: [
    HeaderPageComponent
  ]
})
export class HeaderPageModule {}

import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { PlaceholderGridComponent } from './placeholder-grid.component';


@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [
    PlaceholderGridComponent
  ],
  exports: [
    PlaceholderGridComponent
  ]
})
export class PlaceholderGridModule {}

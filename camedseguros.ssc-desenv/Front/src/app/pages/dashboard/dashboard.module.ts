import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';


import { DashboardComponent } from './dashboard.component';
import { AuthenticationService } from 'src/app/core';

const routes: Routes = [
  { path: '', component: DashboardComponent }
];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    FormsModule,
    ReactiveFormsModule,
  ],

  declarations: [
    DashboardComponent
  ],

  providers: [
    AuthenticationService,
  ],
  exports: [
    RouterModule
  ]
})
export class DashboardModule { }

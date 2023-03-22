import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AuthComponent } from './auth.component';

const routes: Routes = [
  { path: '', component: AuthComponent }
];


@NgModule({
  imports: [CommonModule, FormsModule, ReactiveFormsModule,  RouterModule.forChild(routes)],
  declarations: [
    AuthComponent
  ],
  providers: []
})
export class AuthModule { }

import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { Routes, RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { JwtModule } from '@auth0/angular-jwt';

import { AngularSlickgridModule, GridOption } from 'angular-slickgrid';
import { CoreModule } from './core';
import { Globals } from './globals';
import { HttpClientModule } from '@angular/common/http';
import { gridOptions } from 'src/locale/slickgrid.pt';
import { AppMenuModule } from './shared/layout/app-menu/app-menu.module';
import { AppMenuService } from './shared/layout/app-menu';
import { GraphQLModule } from './graphql.module';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';



const routes: Routes = [
  { path: '', redirectTo: 'pages', pathMatch: 'full' },
  { path: 'pages', loadChildren: () => import('./pages/pages.module').then(m => m.PagesModule) },
  { path: 'login', loadChildren: () => import('./auth/auth.module').then(m => m.AuthModule), },

  // tslint:disable-next-line: comment-format
  //redirecionar para 404
  // { path: '**', redirectTo: '' }
];



@NgModule({
 

  imports: [
    RouterModule.forRoot(routes, { useHash: true }),
    CoreModule,

    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,

    AppMenuModule,

    AngularSlickgridModule.forRoot(gridOptions),

    JwtModule.forRoot({
      config: {
        tokenGetter: () => {
          return localStorage.getItem('__t1ec2ec28-8704-ddc0-1b6a-5cafad524558');
        }
      }
    }),

    GraphQLModule,
  

  ],
  providers: [
    Globals,
    AppMenuService
  ],
  declarations: [
    AppComponent,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

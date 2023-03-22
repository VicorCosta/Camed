import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { ScrollPanelModule } from 'primeng/scrollpanel';
import { UserNotificationComponent } from './user-notification/user-notification.component';
import { UserProfileComponent } from './user-profile';


@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    ScrollPanelModule
  ],
  declarations: [
    UserNotificationComponent,
    UserProfileComponent
  ],
  providers: [
  ],

  exports: [
    UserNotificationComponent,
    UserProfileComponent
  ]
})
export class LayoutModule {}

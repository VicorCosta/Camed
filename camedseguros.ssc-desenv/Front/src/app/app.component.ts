import { Component } from '@angular/core';
import { User, AuthenticationService } from './core';
import { Router } from '@angular/router';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
})
export class AppComponent {

  currentUser: User;

  constructor(
    private router: Router,
    private authenticationService: AuthenticationService
  ) {
    this.currentUser = this.authenticationService.getLoggedUser();
  }
}

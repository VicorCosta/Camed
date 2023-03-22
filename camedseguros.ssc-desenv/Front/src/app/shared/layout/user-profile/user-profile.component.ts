import { Component, OnInit } from '@angular/core';
import { AuthenticationService, User } from 'src/app/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-user-profile',
  templateUrl: 'user-profile.component.html',
})
export class UserProfileComponent implements OnInit {
  loggedUser: User;

  constructor(private authService: AuthenticationService) {

  }

  ngOnInit(): void {
    this.loggedUser = this.authService.getLoggedUser();
  }

  logout(): void {
    this.authService.unauthorize();
    window.location.href = environment.baseUrl;
  }
}

import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder } from '@angular/forms';

import { AuthenticationService, Identity, BaseComponent } from '../core';
import { environment } from 'src/environments/environment';


@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css']
})
export class AuthComponent extends BaseComponent implements OnInit {

  returnUrl: string;
  submitted = false;

  constructor(authenticationService: AuthenticationService, fb: FormBuilder, route: ActivatedRoute, router: Router) {
    super(authenticationService, fb, route, router);

    if (this.authenticationService.isAuthenticated()) {
      this.router.navigate(['/']);
    }
  }

  ngOnInit() {
    this.form = this.fb.group({
      username: [''],
      password: ['']
    });

    this.returnUrl = this.route.snapshot.queryParams.returnUrl || '/';
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;

    const credentials = {
      username: this.f.username.value,
      password: this.f.password.value
    };

    this.authenticationService.login(credentials).subscribe(response => {
      this.setResult(response);

      if (response.successfully) {
        const identity = response.payload as Identity;

        if (identity.authenticated) {
          localStorage.setItem("_user", JSON.stringify(credentials));
          this.authenticationService.setAuth(identity);
          window.location.href = environment.baseUrl;
        }
      }
    }, (error) => this.showError(error));
  }
}

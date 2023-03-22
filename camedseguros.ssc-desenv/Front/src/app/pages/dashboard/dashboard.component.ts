import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder } from '@angular/forms';

import { User, BaseComponent, AuthenticationService } from 'src/app/core';

@Component({
  templateUrl: 'dashboard.component.html'
})
export class DashboardComponent extends BaseComponent implements OnInit {
  loading = false;
  users: Observable<User[]>;

  constructor(authenticationService: AuthenticationService,
              fb: FormBuilder,
              route: ActivatedRoute,
              router: Router
    ) {
      super(authenticationService, fb, route, router);
     }

  ngOnInit() {
    this.loading = true;
  }
}

import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from '../services/authentication.service';

@Component({
  selector: 'app-signoutredirect',
  template:'<div></div>'
})
export class SignoutredirectComponent implements OnInit {

  isSuccess: string;

  constructor(private authService: AuthenticationService, private router: Router, private _activatedRoute: ActivatedRoute) {
    this._activatedRoute.queryParams.subscribe(params => {
      this.isSuccess = params['isSuccess'];
    });
    }

    ngOnInit() {
      this.authService.completeLogout().then(_ => {

        if (this.isSuccess != "0") {
          this.router.navigate(['/'], { replaceUrl: true });
        }
        else {
          this.router.navigate(['/'], { queryParams: { isSuccess: this.isSuccess }, replaceUrl: true });
        }
        })
    }

}

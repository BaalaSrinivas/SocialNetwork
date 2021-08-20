import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from '../shared/authentication.service';

@Component({
  selector: 'app-signoutredirect',
  template:'<div></div>'
})
export class SignoutredirectComponent implements OnInit {

    constructor(private authService: AuthenticationService, private router: Router) {

    }

    ngOnInit() {
        this.authService.completeLogout().then(_ => {
            this.router.navigate(['/'], { replaceUrl: true });
        })
    }

}

import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from '../shared/authentication.service';

@Component({
    selector: 'app-signinredirect',
    template: `<div></div>`
})
export class SigninredirectComponent implements OnInit {

    constructor(private authService: AuthenticationService, private router: Router) { }

    ngOnInit() {
        this.authService.completeLogin().then(user => {
            this.router.navigate(['/profile'], { replaceUrl: true });
        })
    }

}

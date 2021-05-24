import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../complete-signup/user.service';
import { AuthenticationService } from '../shared/authentication.service';

@Component({
    selector: 'app-signinredirect',
    template: `<div></div>`
})
export class SigninredirectComponent implements OnInit {

    constructor(private authService: AuthenticationService, private router: Router, private userService: UserService) { }

    ngOnInit() {
        this.authService.completeLogin().then(user => {

            //fetch userinfo
            this.userService.getUser(sessionStorage.getItem('mailId')).subscribe((r) => {
                if (r) {
                    this.router.navigate(['/profile'], { queryParams: { mailid: sessionStorage.getItem('mailId') }, replaceUrl: true });
                }
                else {
                    this.router.navigate(['/complete-signup'], { replaceUrl: true });
                }
            });
        })
    }

}

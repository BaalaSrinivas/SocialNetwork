import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from '../services/authentication.service';
import { UserService } from '../services/user.service';

@Component({
    selector: 'app-signinredirect',
  template: `<div class="position-fixed" style="left:45%;top:45%;z-index:100">
              <div class="spinner-border" style="width: 5rem; height: 5rem;" role="status">
                <span class="sr-only">Loading...</span>
              </div>
            </div>`
})
export class SigninredirectComponent implements OnInit {

    constructor(private authService: AuthenticationService, private router: Router, private userService: UserService) { }

    ngOnInit() {
        this.authService.completeLogin().then(user => {

            //fetch userinfo
            this.userService.getUser(sessionStorage.getItem('mailId')).subscribe((r) => {
                if (r) {
                    sessionStorage.setItem('profileUrl', r.ProfileImageUrl);
                    this.router.navigate(['/profile'], { queryParams: { mailid: sessionStorage.getItem('mailId') }, replaceUrl: true });
                }
                else {
                  this.router.navigate(['/complete-registration'], { replaceUrl: true });
                }
            });
        })
    }

}

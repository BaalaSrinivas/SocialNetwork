import { Component, OnInit } from '@angular/core';
import { GoogleLoginProvider, SocialAuthService, SocialUser } from 'angularx-social-login';

const googleLoginOptions = {
    scope: 'profile email'
};

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})



export class LoginComponent implements OnInit {


    focus;
    focus1;

    user: SocialUser;
    loggedIn: boolean;

    constructor(private authService: SocialAuthService) { }

    ngOnInit() {
        this.authService.authState.subscribe((user) => {
            this.user = user;
            this.loggedIn = (user != null);
        });
    }

    signInWithGoogle(): void {
        this.authService.signIn(GoogleLoginProvider.PROVIDER_ID, googleLoginOptions);
    }

    signOut(): void {
        this.authService.signOut();
    }

}

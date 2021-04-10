import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../shared/authentication.service';

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

    loggedIn: boolean;

    constructor(private authService: AuthenticationService) { }

    ngOnInit() {

    }

    signInWithGoogle(): void {
        this.authService.login();
    }

    signOut(): void {
        this.authService.logout();
    }

}

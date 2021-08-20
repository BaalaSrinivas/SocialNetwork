import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from '../shared/authentication.service';
import { Register } from './Models/register.model';

@Component({
    selector: 'app-signup',
    templateUrl: './signup.component.html',
    styleUrls: ['./signup.component.scss']
})
export class SignupComponent implements OnInit {

    constructor(private _authService: AuthenticationService, private router: Router) {

    }

    registerModel: Register = new Register();

    test : Date = new Date();
    focus;
    focus1;
    focus2;

    ngOnInit() { }

    registerUser() {
        this._authService.registerIdentity(this.registerModel).subscribe(s => {
            console.log(s);
            console.log(s.succeeded == true);
            if (s.succeeded == true) {
                this.router.navigate(['/complete-signup'], { replaceUrl: true });
            }
        });
    }
}

import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../services/authentication.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {


  constructor(private _authenticationService: AuthenticationService) { }

  userName: string;
  password: string

  ngOnInit(): void {
  }

  signInWithGoogle(): void {
    this._authenticationService.login();
  }

  signOut(): void {
    this._authenticationService.logout();
  }

  signInWithIdentity(): void {
    this._authenticationService.loginIdentity(this.userName, this.password).subscribe((s) => {
      //Once user is loggedin initiate Open Id flow
      this._authenticationService.login();
    });
  }

}

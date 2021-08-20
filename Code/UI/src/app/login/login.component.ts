import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthenticationService } from '../services/authentication.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  isSuccess: string;

  constructor(private _authenticationService: AuthenticationService,
    private _activatedRoute: ActivatedRoute)
  {
    this._activatedRoute.queryParams.subscribe(params => {
      this.isSuccess = params['isSuccess'];
    });
  }

  userName: string;
  password: string
  showLoading: boolean = false;

  ngOnInit(): void {
  }

  emailChange() {
    this.isSuccess = "1";
  }

  signInWithGoogle(): void {
    this._authenticationService.login();
  }

  signOut(): void {
    this._authenticationService.logout();
  }

  signInWithIdentity(): void {
    this.showLoading = true;
    this._authenticationService.loginIdentity(this.userName, this.password).subscribe((s) => {
      //Once user is loggedin initiate Open Id flow
      this._authenticationService.login();
    });
  }

}

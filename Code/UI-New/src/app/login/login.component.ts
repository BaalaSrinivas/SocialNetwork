import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {


  constructor() { }

  ngOnInit(): void {
  }

  signInWithGoogle(): void {
    //this.authService.login();
  }

  signOut(): void {
    //this.authService.logout();
  }

  signInWithIdentity(): void {
    //this.authService.loginIdentity(this.userName, this.password).subscribe((s) => {
    //  //Once user is loggedin initiate Open Id flow
    //  this.authService.login();
    //});
  }

}

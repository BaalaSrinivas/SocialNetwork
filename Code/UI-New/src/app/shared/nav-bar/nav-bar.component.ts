import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '../../models/user.model';
import { AuthenticationService } from '../../services/authentication.service';
import { SignalrService } from '../../services/signalr.service';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent implements OnInit {

  userName: string;
  profileUrl: string;
  searchKey: string;
  users: User[];

  notifications: string[] = [];

  constructor(
    private signalrService: SignalrService,
    private router: Router,
    private authService: AuthenticationService,
    private _userService: UserService
  ) {
    this.authService.loginChanged.subscribe(x => {
      this.userName = sessionStorage.getItem('loggedUser')
      this.profileUrl = sessionStorage.getItem('profileUrl')
    });

    signalrService.addCallbackListener((data) => {
      if (this.notifications.length > 0) {
        console.log(data);
        this.notifications.unshift(data);
      }
      else {
        console.log(data);
        this.notifications.push(data);
        console.log(this.notifications);
      }
    }, 'ReceiveMessage');
  }

  ngOnInit(): void {
    this.userName = sessionStorage.getItem('loggedUser');
    this.profileUrl = sessionStorage.getItem('profileUrl');
  }

  IsUserLoggedIn(): Boolean {
    return sessionStorage.getItem('loggedUser') != null;
  }

  logOut() {
    this.authService.logout();
  }

  getNames(key: string, clear?: boolean) {
    if (clear) {
      this.searchKey = "";
    }
    if (key.length >= 2) {
      this._userService.searchUser(key).subscribe(data => {
        this.users = data.splice(0, 8);
      });
    }
    else {
      this.users = [];
    }
  }

}

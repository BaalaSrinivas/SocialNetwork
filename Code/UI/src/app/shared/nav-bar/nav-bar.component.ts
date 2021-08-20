import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { from } from 'rxjs';
import { User } from '../../models/user.model';
import { AuthenticationService } from '../../services/authentication.service';
import { NotificationService } from '../../services/notification.service';
import { SignalrService } from '../../services/signalr.service';
import { UserService } from '../../services/user.service';
import { Notification } from '../../models/notification.model';

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

  notifications: Notification[] = [];

  constructor(
    private signalrService: SignalrService,
    private router: Router,
    private authService: AuthenticationService,
    private _userService: UserService,
    private _notificationService: NotificationService
  ) {
    this.authService.loginChanged.subscribe(x => {
      this.userName = sessionStorage.getItem('loggedUser');
      this.profileUrl = sessionStorage.getItem('profileUrl');
    });

    this.authService.profileUrlUpdated.subscribe(x => {
      this.profileUrl = sessionStorage.getItem('profileUrl');
      this.GetNotifications();
    });    

    signalrService.addCallbackListener((data) => {
      if (this.notifications.length > 0) {
        console.log(data);
        this.notifications.unshift(data);
        this.playAudio();
      }
      else {
        console.log(data);
        this.notifications.push(data);
        this.notifications = this.notifications;
        this.playAudio();
      }
    }, 'ReceiveMessage');
  }

  GetNotifications() {
    this._notificationService.getNotifications().subscribe((data) => {
      this.notifications = data;
    });
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

  UpdateNotificationReadStatus(notification: Notification) {
    this._notificationService.UpdateNotificationReadStatus(notification.Id).subscribe(data => {
      console.log(notification);
      if (notification.Type == "post") {
        this.router.navigate(['/view-post'], { queryParams: { id: notification.PostId }, replaceUrl: true });
      }
      else if (notification.Type == "friendrequest") {
        this.router.navigate(['/friends']);
      }
      this.GetNotifications();
    });
  }

  playAudio() {
    let audio = new Audio();
    audio.src = "../../../assets/audio/noti-sound.mp3";
    audio.load();
    audio.play();
  }
}

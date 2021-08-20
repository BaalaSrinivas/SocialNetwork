import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd, NavigationStart } from '@angular/router';
import { Location, PopStateEvent } from '@angular/common';
import { AuthenticationService } from '../authentication.service';
import { SignalrService } from '../../signalr.service';
import { UserService } from '../../complete-signup/user.service';
import { User } from '../../complete-signup/Models/user.model';


@Component({
    selector: 'app-navbar',
    templateUrl: './navbar.component.html',
    styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
    public isCollapsed = true;
    private lastPoppedUrl: string;
    private yScrollStack: number[] = [];

    private userName: string;
    private profileUrl: string;
    private searchKey: string;
    private users: User[];

    private notifications: string[] = [];

    constructor(public location: Location,
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
                this.notifications.unshift(data);
            }
            else {
                console.log(data);
                this.notifications.push(data);
                console.log(this.notifications);
            }
        }, 'ReceiveMessage');
    }

    ngOnInit() {
        this.router.events.subscribe((event) => {
            this.isCollapsed = true;
            if (event instanceof NavigationStart) {
                if (event.url != this.lastPoppedUrl)
                    this.yScrollStack.push(window.scrollY);
            } else if (event instanceof NavigationEnd) {
                if (event.url == this.lastPoppedUrl) {
                    this.lastPoppedUrl = undefined;
                    window.scrollTo(0, this.yScrollStack.pop());
                } else
                    window.scrollTo(0, 0);
            }
        });
        this.location.subscribe((ev: PopStateEvent) => {
            this.lastPoppedUrl = ev.url;
        });
        this.userName = sessionStorage.getItem('loggedUser');
        this.profileUrl = sessionStorage.getItem('profileUrl');
    }

    isHome() {
        var titlee = this.location.prepareExternalUrl(this.location.path());

        if (titlee === '#/home') {
            return true;
        }
        else {
            return false;
        }
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
                this.users = data.splice(0, 10);
            });
        }
        else {
            this.users = [];
        }
    }
}

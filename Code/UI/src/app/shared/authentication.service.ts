import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { User, UserManager } from "oidc-client";
import { Subject } from "rxjs";

@Injectable()
export class AuthenticationService {
    private _userManager: UserManager;
    private _user: User;

    public userName: string;

    private _loginChangedSubject = new Subject<boolean>();

    loginChanged = this._loginChangedSubject.asObservable();

    constructor(private _httpClient: HttpClient) {
        const smSettings = {
            authority: 'https://accounts.google.com',
            client_id: '216892140019-lvs71bvj54t4s7stp195uuhl6foggrsd.apps.googleusercontent.com',
            redirect_uri: `http://localhost:4200/signinredirect`,
            scope: 'profile email',
            response_type: 'id_token token',
            post_logout_redirect_uri: `http://localhost:4200/signoutredirect`,
            automaticSilentRenew: true,
            silent_redirect_uri: `$http://localhost:4200/assets/silent-callback.html`,
            metadata: {
                end_session_endpoint: 'http://localhost:4200/signoutredirect',
                authorization_endpoint: 'https://accounts.google.com/o/oauth2/v2/auth',
                issuer: 'https://accounts.google.com',
                jwks_uri: 'https://www.googleapis.com/oauth2/v3/certs',
                token_endpoint: 'https://oauth2.googleapis.com/token',
                userinfo_endpoint: 'https://openidconnect.googleapis.com/v1/userinfo',
            }
        };
        this._userManager = new UserManager(smSettings);

        this._userManager.events.addAccessTokenExpired(_ => {
            this._loginChangedSubject.next(false);
        });

        this._userManager.events.addUserLoaded(user => {
            if (this._user !== user) {
                this._user = user;
            }
        });
    }

    login() {
        return this._userManager.signinRedirect();
    }

    isLoggedIn(): Promise<boolean> {
        return this._userManager.getUser().then(user => {
            const userCurrent = !!user && !user.expired;
            if (this._user !== user) {
                this._loginChangedSubject.next(userCurrent);
            }
            this._user = user;
            return userCurrent;
        });
    }

    completeLogin() {
        return this._userManager.signinRedirectCallback().then(user => {
            console.log(user);
            this._user = user;
            sessionStorage.setItem('loggedUser', this._user.profile.name);
            sessionStorage.setItem('accessToken', this._user.access_token);
            this._loginChangedSubject.next(!!user && !user.expired);           
            return user;
        });
    }

    logout() {
        this._userManager.signoutRedirect();
    }

    completeLogout() {
        this._user = null;
        sessionStorage.clear();
        this._loginChangedSubject.next(false);        
        return this._userManager.signoutRedirectCallback();
    }

    getAccessToken(): Promise<string> {
        return this._userManager.getUser().then(user => {
            if (!!user && !user.expired) {
                return user.access_token;
            }
            else {
                return null;
            }
        });
    }
}
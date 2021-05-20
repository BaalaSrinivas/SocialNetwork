import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { User, UserManager } from "oidc-client";
import { Observable, Subject } from "rxjs";

const httpOptions = {
    headers: new HttpHeaders({
        'Content-Type': 'application/json',
    }),
    withCredentials: true
};


@Injectable()
export class AuthenticationService {
    private _userManager: UserManager;
    private _user: User;

    public userName: string;

    private _loginChangedSubject = new Subject<boolean>();

    loginChanged = this._loginChangedSubject.asObservable();

    constructor(private _httpClient: HttpClient) {
        const identitySettings = {
            authority: 'https://localhost:5004',
            client_id: 'BSKonnectIdentityServerID',
            redirect_uri: `http://localhost:4200/signinredirect`,
            scope: 'profile openid email',
            response_type: 'id_token token',
            post_logout_redirect_uri: `http://localhost:4200/signoutredirect`,
            automaticSilentRenew: true,
            silent_redirect_uri: `$http://localhost:4200/assets/silent-callback.html`,
            metadata: {
                end_session_endpoint: 'https://localhost:5004/connect/endsession',
                authorization_endpoint: 'https://localhost:5004/connect/authorize',
                issuer: 'https://localhost:5004',
                jwks_uri: 'https://localhost:5004/.well-known/openid-configuration/jwks',
                token_endpoint: 'https://localhost:5004/connect/token',
                userinfo_endpoint: 'https://localhost:5004/connect/userinfo',
            }
        };

        const googleSettings = {
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

        this._userManager = new UserManager(googleSettings);

        this._userManager.events.addAccessTokenExpired(_ => {
            this._loginChangedSubject.next(false);
        });

        this._userManager.events.addUserLoaded(user => {
            if (this._user !== user) {
                this._user = user;
            }
        });
    }

    loginIdentity(userName: string, password: string): Observable<any> {

        const data = {
            'MailId': userName,
            'Password': password
        }

        return this._httpClient.post<boolean>('https://localhost:5004/Account/login', data, httpOptions);
    }

    login() {
        return this._userManager.signinRedirect();
    }

    completeLogin() {
        return this._userManager.signinRedirectCallback().then(user => {
            console.log(user);
            this._user = user;
            sessionStorage.setItem('loggedUser', this._user.profile.name);
            sessionStorage.setItem('idToken', this._user.id_token);
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
}
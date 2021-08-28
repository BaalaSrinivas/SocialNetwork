import { EventEmitter, Injectable } from "@angular/core";
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { User, UserManager } from "oidc-client";
import { from, Observable, Subject } from "rxjs";
import { Register } from "../models/register.model";
import { environment } from "../../environments/environment";

const authenticationApi = environment.apiUrl + 'v1/authenticate/';

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

  private _loginChangedSubject = new Subject<boolean>();
  profileUrlUpdated = new EventEmitter<boolean>();

  loginChanged = this._loginChangedSubject.asObservable();

  constructor(private _httpClient: HttpClient) {
    const identitySettings = {
      authority: environment.Issuer,
      client_id: 'BSKonnectIdentityServerID',
      redirect_uri: window.location.origin + '/signinredirect',
      scope: 'profile openid email',
      response_type: 'id_token token',
      post_logout_redirect_uri: window.location.origin + '/signoutredirect',
      automaticSilentRenew: true,      
      metadata: {
        end_session_endpoint: authenticationApi + 'connect/endsession',
        authorization_endpoint: authenticationApi + 'connect/authorize',
        issuer: environment.Issuer,
        jwks_uri: authenticationApi + '.well-known/openid-configuration/jwks',
        token_endpoint: authenticationApi + 'connect/token',
        userinfo_endpoint: authenticationApi + 'connect/userinfo',
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


    this._userManager = new UserManager(identitySettings);

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

    return this._httpClient.post<boolean>(authenticationApi +'Account/login', data, httpOptions);
  }

  registerIdentity(register: Register): Observable<any> {
    return this._httpClient.post<boolean>(authenticationApi +'Account/register', register, httpOptions);
  }

  login() {
    return this._userManager.signinRedirect();
  }

  completeLogin() {
    return this._userManager.signinRedirectCallback().then(user => {
      this._user = user;
      sessionStorage.setItem('loggedUser', this._user.profile.name);
      sessionStorage.setItem('idToken', this._user.id_token);
      sessionStorage.setItem('accessToken', this._user.access_token);
      sessionStorage.setItem('mailId', this._user.profile.email);
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

import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterModule } from '@angular/router';
import { AppRoutingModule } from './app.routing';


import { AppComponent } from './app.component';
import { SignupComponent } from './signup/signup.component';
import { LandingComponent } from './landing/landing.component';
import { ProfileComponent } from './profile/profile.component';
import { HomeComponent } from './home/home.component';
import { NavbarComponent } from './shared/navbar/navbar.component';
import { FooterComponent } from './shared/footer/footer.component';

import { HomeModule } from './home/home.module';
import { LoginComponent } from './login/login.component';
import { NewsFeedModule } from './newsfeed/newsfeed.module';
import { AuthenticationService } from './shared/authentication.service';
import { HttpClientModule } from '@angular/common/http';
import { SigninredirectComponent } from './signinredirect/signinredirect.component';
import { SignoutredirectComponent } from './signoutredirect/signoutredirect.component';



@NgModule({
    declarations: [
        AppComponent,
        SignupComponent,
        LandingComponent,
        ProfileComponent,
        NavbarComponent,
        FooterComponent,
        LoginComponent,
        SigninredirectComponent,
        SignoutredirectComponent,      
    ],
    imports: [
        BrowserModule,
        HttpClientModule,
        NgbModule,
        FormsModule,
        RouterModule,
        AppRoutingModule,
        HomeModule,
        NewsFeedModule
    ],
    providers: [AuthenticationService],
    bootstrap: [AppComponent]
})
export class AppModule { }

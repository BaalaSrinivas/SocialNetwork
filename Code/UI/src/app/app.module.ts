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
import { AuthenticationService } from './shared/authentication.service';
import { HttpClientModule } from '@angular/common/http';
import { SigninredirectComponent } from './signinredirect/signinredirect.component';
import { SignoutredirectComponent } from './signoutredirect/signoutredirect.component';
import { ContentService } from './profile/content.service';
import { PostAdapter } from './profile/Models/Adapters/post.adapter';
import { FeedSectionComponent } from './feed-section/feed-section.component';
import { NewsfeedComponent } from './newsfeed/newsfeed.component';
import { SignalrService } from './signalr.service';
import { CompleteSignupComponent } from './complete-signup/complete-signup.component';
import { UserService } from './complete-signup/user.service';
import { PostSectionComponent } from './post-section/post-section.component';
import { FriendsListComponent } from './friends-list/friends-list.component';
import { PostEditorComponent } from './post-editor/post-editor.component';
import { UserAdapter } from './complete-signup/Models/Adapters/user.adapter';



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
        FeedSectionComponent,
        NewsfeedComponent,
        CompleteSignupComponent,
        PostSectionComponent,
        FriendsListComponent,
        PostEditorComponent
    ],
    imports: [
        BrowserModule,
        HttpClientModule,
        NgbModule,
        FormsModule,
        RouterModule,
        AppRoutingModule,
        HomeModule,
    ],
    providers: [AuthenticationService, ContentService, UserService, SignalrService, PostAdapter, UserAdapter],
    bootstrap: [AppComponent]
})
export class AppModule { }

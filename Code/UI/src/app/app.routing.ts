import { NgModule } from '@angular/core';
import { CommonModule, } from '@angular/common';
import { BrowserModule  } from '@angular/platform-browser';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { ProfileComponent } from './profile/profile.component';
import { SignupComponent } from './signup/signup.component';
import { LandingComponent } from './landing/landing.component';
import { LoginComponent } from './login/login.component';
import { NewsfeedComponent } from './newsfeed/newsfeed.component';
import { SigninredirectComponent } from './signinredirect/signinredirect.component';
import { SignoutredirectComponent } from './signoutredirect/signoutredirect.component';
import { CompleteSignupComponent } from './complete-signup/complete-signup.component';
import { PostSectionComponent } from './post-section/post-section.component';
import { FriendsListComponent } from './friends-list/friends-list.component';
import { AuthGuard } from './auth.guard';

const routes: Routes =[
    { path: 'home', component: HomeComponent },
    { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard] },
    { path: 'register',           component: SignupComponent },
    { path: 'landing',          component: LandingComponent },
    { path: 'login', component: LoginComponent },
    { path: 'newsfeed', component: NewsfeedComponent, canActivate: [AuthGuard] },
    { path: 'signinredirect', component: SigninredirectComponent },
    { path: 'signoutredirect', component: SignoutredirectComponent },
    { path: 'complete-signup', component: CompleteSignupComponent, canActivate: [AuthGuard] },
    { path: 'view-post', component: PostSectionComponent, canActivate: [AuthGuard] },
    { path: 'friends', component: FriendsListComponent, canActivate: [AuthGuard] },
    { path: '', redirectTo: 'login', pathMatch: 'full' }
];

@NgModule({
  imports: [
    CommonModule,
    BrowserModule,
        RouterModule.forRoot(routes, {
            useHash: false
    })
  ],
  exports: [
  ],
})
export class AppRoutingModule { }

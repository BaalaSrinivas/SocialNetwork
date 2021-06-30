import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './auth.guard';
import { CompleteRegisterComponent } from './complete-register/complete-register.component';
import { FriendsComponent } from './friends/friends.component';
import { FullPostViewComponent } from './full-post-view/full-post-view.component';
import { LoginComponent } from './login/login.component';
import { NewsfeedComponent } from './newsfeed/newsfeed.component';
import { ProfileComponent } from './profile/profile.component';
import { RegisterComponent } from './register/register.component';

const routes: Routes = [
  { path: 'complete-registration', component: CompleteRegisterComponent, canActivate: [AuthGuard] },
  { path: 'view-post', component: FullPostViewComponent },
  { path: 'friends', component: FriendsComponent },
  { path: 'login', component: LoginComponent },
  { path: 'newsfeed', component: NewsfeedComponent },
  { path: 'profile', component: ProfileComponent },
  { path: 'register', component: RegisterComponent },
  { path: '', redirectTo: 'login', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

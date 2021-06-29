import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { CompleteRegisterComponent } from './complete-register/complete-register.component';
import { NavBarComponent } from './shared/nav-bar/nav-bar.component';
import { NewsfeedComponent } from './newsfeed/newsfeed.component';
import { ProfileComponent } from './profile/profile.component';
import { FriendsComponent } from './friends/friends.component';
import { AuthenticationService } from './services/authentication.service';
import { HttpClientModule } from '@angular/common/http';
import { UserService } from './services/user.service';
import { NewsfeedService } from './services/newsfeed.service';
import { ContentService } from './services/content.service';
import { FollowService } from './services/follow.service';
import { SignalrService } from './services/signalr.service';
import { CommentAdapter } from './models/adapters/comment.adapter';
import { FollowAdapter } from './models/adapters/follow.adapter';
import { FriendAdapter } from './models/adapters/friend.adapter';
import { PostAdapter } from './models/adapters/post.adapter';
import { UserAdapter } from './models/adapters/user.adapter';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    CompleteRegisterComponent,
    NavBarComponent,
    NewsfeedComponent,
    ProfileComponent,
    FriendsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    NgbModule
  ],
  providers: [
    AuthenticationService,
    ContentService,
    FollowService,
    NewsfeedService,
    SignalrService,
    UserService,
    CommentAdapter,
    FollowAdapter,
    FriendAdapter,
    PostAdapter,
    UserAdapter
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
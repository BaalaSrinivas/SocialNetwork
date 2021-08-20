import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserAdapter } from '../complete-signup/Models/Adapters/user.adapter';
import { User } from '../complete-signup/Models/user.model';
import { UserService } from '../complete-signup/user.service';
import { FollowService } from '../shared/follow.service';
import { ContentService } from './content.service';
import { Post } from './Models/post.model';

@Component({
    selector: 'app-profile',
    templateUrl: './profile.component.html',
    styleUrls: ['./profile.component.scss']
})

export class ProfileComponent implements OnInit {
    mailId: string
    showAddPost: boolean;

    constructor(private _contentService: ContentService,
        private _userService: UserService,
        private _userAdapter: UserAdapter,
        private _followService: FollowService,
        private _activatedRoute: ActivatedRoute) {
      
    }

    user: User = new User();
    userName: string;

    userPosts: Post[];

    ngOnInit() {
        this._activatedRoute.queryParams.subscribe(params => {
            this.mailId = params['mailid'] != undefined ? params['mailid'] : sessionStorage.getItem('mailId');
            this.showAddPost = this.mailId == sessionStorage.getItem('mailId');

            this._userService.getUser(this.mailId).subscribe(u => {
                this.user = u;
            });

            this._contentService.getUserPostIds(20, this.mailId).subscribe(ids => {

                this._contentService.getPosts(ids).subscribe(posts => {
                    this.userPosts = posts;
                    console.log(this.userPosts);
                });
            });
        });        
    }

    sendFriendRequest() {
        this._followService.sendFriendRequest(this.mailId).subscribe(data => {

        });
    }

    followUser() {
        this._followService.followUser(this.mailId).subscribe(data => {

        });
    }

}

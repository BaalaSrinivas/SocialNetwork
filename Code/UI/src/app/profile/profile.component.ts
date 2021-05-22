import { Component, OnInit } from '@angular/core';
import { UserAdapter } from '../complete-signup/Models/Adapters/user.adapter';
import { User } from '../complete-signup/Models/user.model';
import { UserService } from '../complete-signup/user.service';
import { ContentService } from './content.service';
import { Post } from './Models/post.model';

@Component({
    selector: 'app-profile',
    templateUrl: './profile.component.html',
    styleUrls: ['./profile.component.scss']
})

export class ProfileComponent implements OnInit {

    constructor(private _contentService: ContentService, private _userService: UserService, private _userAdapter: UserAdapter) { }

    user: User = new User();
    userName: string;

    userPosts: Post[];

    ngOnInit() {

        this._userService.getUser().subscribe(u => {
            this.user = this._userAdapter.Adapt(u);
        });

        this._contentService.getUserPostIds(20).subscribe(ids => {
            console.log(ids);
            this._contentService.getPosts(ids).subscribe(posts => {
                this.userPosts = posts;
                console.log(this.userPosts);
            });
        });
    }

}

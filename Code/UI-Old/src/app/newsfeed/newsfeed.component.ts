import { Component, OnInit } from '@angular/core';
import { ContentService } from '../profile/content.service';
import { Post } from '../profile/Models/post.model';
import { NewsfeedService } from './newsfeed.service';

@Component({
  selector: 'app-newsfeed',
  templateUrl: './newsfeed.component.html',
  styleUrls: ['./newsfeed.component.css']
})
export class NewsfeedComponent implements OnInit {

    feedPosts: Post[];
    profileImageUrl: string;
    constructor(private _newsfeedService: NewsfeedService, private _contentService: ContentService) { }

    ngOnInit(): void {
        this._newsfeedService.getNewsfeed(sessionStorage.getItem('mailId')).subscribe(data => {
            this._contentService.getPosts(data).subscribe(posts => {
                this.feedPosts = posts;
                console.log(this.feedPosts);
            });
            this.profileImageUrl = sessionStorage.getItem('profileUrl');
        });
  }

}

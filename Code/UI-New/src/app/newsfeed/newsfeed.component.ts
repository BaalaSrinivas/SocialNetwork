import { Component, OnInit } from '@angular/core';
import { Post } from '../models/post.model';
import { ContentService } from '../services/content.service';
import { NewsfeedService } from '../services/newsfeed.service';

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
      });
      this.profileImageUrl = sessionStorage.getItem('profileUrl');
    });
  }

}

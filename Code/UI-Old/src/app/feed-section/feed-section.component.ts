import { Input } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { ContentService } from '../profile/content.service';
import { Post } from '../profile/Models/post.model';
import { Comment } from '../profile/Models/comment.model';

@Component({
    selector: 'app-feed-section',
    templateUrl: './feed-section.component.html',
    styleUrls: ['./feed-section.component.css']
})
export class FeedSectionComponent implements OnInit {
    @Input()
    post: Post = new Post();
    profileUrl: string;

    comment: Comment = new Comment();    

    postComments: Comment[];

    constructor(private _contentService: ContentService) { }
    commentToggle: boolean = false;


    ngOnInit(): void {
        this.profileUrl = sessionStorage.getItem('profileUrl');
    }

    change() {
        this.commentToggle = !this.commentToggle;
        if (this.commentToggle) {
            this.getComments();
        }
    }

    likePost() {
        this._contentService.likePost(this.post.Id).subscribe(l => {
            this.post.LikeCount = l;
            this.post.HasUserLiked = !this.post.HasUserLiked;
        });
    }

    addComment() {
        this.comment.PostId = this.post.Id
        this._contentService.addComment(this.comment).subscribe(r => {
            console.log(r);
            this.post.CommentCount = r;
            this.comment = new Comment();     
            this.getComments();
        });
    }

    getComments() {        
        this._contentService.getComments(this.post.Id).subscribe(comments => {
            this.postComments = comments;
        });
    }
}

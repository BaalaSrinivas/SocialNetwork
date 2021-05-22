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
    comment: Comment = new Comment();    

    postComments: Comment[];

    constructor(private _contentService: ContentService) { }
    commentToggle: boolean = false;


    ngOnInit(): void {
    }

    change() {
        this.commentToggle = !this.commentToggle;
        if (this.commentToggle) {
            this.getComments();
        }
    }

    like() {
        //this._contentService.
    }

    addComment() {
        this.comment.PostId = this.post.Id
        this._contentService.addComment(this.comment).subscribe(r => {
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

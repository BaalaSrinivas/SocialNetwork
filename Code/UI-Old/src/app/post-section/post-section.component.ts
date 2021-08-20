import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ContentService } from '../profile/content.service';
import { Comment } from '../profile/Models/comment.model';
import { Post } from '../profile/Models/post.model';

@Component({
    selector: 'app-post-section',
    templateUrl: './post-section.component.html',
    styleUrls: ['./post-section.component.css']
})
export class PostSectionComponent implements OnInit {

    post: Post = new Post();
    comments: Comment[];
    profileImageUrl: string;
    comment: Comment = new Comment();

    constructor(private _activatedRoute: ActivatedRoute, private _contentService: ContentService) { }

    ngOnInit(): void {
        this._activatedRoute.queryParams.subscribe(params => {
            this.post.Id = params['id'];
            var ids: string[] = [this.post.Id];
            this._contentService.getPosts(ids).subscribe(data => {
                this.post = data[0];
            });
            this.getComments();
        });

        this.profileImageUrl = sessionStorage.getItem('profileUrl');
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
            this.comments = comments;
        });
    }

    likePost() {
        this._contentService.likePost(this.post.Id).subscribe(l => {
            this.post.LikeCount = l;
            this.post.HasUserLiked = !this.post.HasUserLiked;
        });
    }
}

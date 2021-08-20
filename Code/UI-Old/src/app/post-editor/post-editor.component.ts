import { Component, Input, OnInit } from '@angular/core';
import { ContentService } from '../profile/content.service';
import { Post } from '../profile/Models/post.model';

@Component({
    selector: 'app-post-editor',
    templateUrl: './post-editor.component.html',
    styleUrls: ['./post-editor.component.css']
})
export class PostEditorComponent implements OnInit {

    post: Post = new Post();
    @Input()
    userProfile: string

    constructor(private _contentService: ContentService) { }
    textareaColor: string = '#ffffff';

    ngOnInit(): void {
    }

    createPost() {
        this._contentService.createPost(this.post).subscribe(c => {
            this.post = new Post();
        });
    }
}

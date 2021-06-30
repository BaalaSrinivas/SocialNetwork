import { Component, Input, OnInit } from '@angular/core';
import { Post } from '../../models/post.model';
import { ContentService } from '../../services/content.service';

@Component({
  selector: 'app-post-editor-element',
  templateUrl: './post-editor-element.component.html',
  styleUrls: ['./post-editor-element.component.css']
})
export class PostEditorElementComponent implements OnInit {

  post: Post = new Post();
  @Input()
  userProfile: string

  constructor(private _contentService: ContentService) { }

  ngOnInit(): void {
  }

  createPost() {
    this._contentService.createPost(this.post).subscribe(c => {
      this.post = new Post();
    });
  }
}

import { Component, Input, OnInit } from '@angular/core';
import { Post } from '../../models/post.model';
import { ContentService } from '../../services/content.service';
import ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import { ImageUploadAdapter } from '../../models/adapters/imageupload.adapter';

@Component({
  selector: 'app-post-editor-element',
  templateUrl: './post-editor-element.component.html',
  styleUrls: ['./post-editor-element.component.css']
})
export class PostEditorElementComponent implements OnInit {

  public Editor = ClassicEditor;

  public editorConfig = {
    toolbar: ['bold', 'italic', 'link', '|', 'uploadImage', 'mediaEmbed']
  };

  post: Post = new Post();
  @Input()
  userProfile: string

  constructor(private _contentService: ContentService) { }
  textareaColor: string = '#ffffff';

  ngOnInit(): void {
    this.post.Content = "Write something...";
  }

  createPost() {
    this._contentService.createPost(this.post).subscribe(c => {
      this.post = new Post();
      this.post.Content = "Write something...";
    });
  }

  onReady(eventData) {
    eventData.plugins.get('FileRepository').createUploadAdapter = function (loader) {
      return new ImageUploadAdapter(loader);
    };
  }
}

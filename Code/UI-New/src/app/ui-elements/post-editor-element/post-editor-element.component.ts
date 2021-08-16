import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Post } from '../../models/post.model';
import { ContentService } from '../../services/content.service';
import ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import { ImageUploadAdapter } from '../../models/adapters/imageupload.adapter';
import { ToastService } from '../../services/toast.service';

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

  @Output() newPost = new EventEmitter<boolean>();

  constructor(private _contentService: ContentService,
    private toastService: ToastService  ) { }
  textareaColor: string = '#ffffff';

  ngOnInit(): void {
    this.post.Content = "Write something...";
  }

  createPost() {
    this._contentService.createPost(this.post).subscribe(c => {
      this.post = new Post();
      this.post.Content = "Write something...";
      this.toastService.show({
        title: 'Success',
        content: 'Posted Successfully',
        class: 'text-success'
      });
      this.newPost.emit(true);
    }, (error) => {
        this.toastService.show({
          title: 'Error',
          content: 'Error while posting. Please try again later',
          class: 'text-danger'
        });
    });
  }

  onReady(eventData) {
    eventData.plugins.get('FileRepository').createUploadAdapter = function (loader) {
      return new ImageUploadAdapter(loader);
    };
  }
}

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PostEditorElementComponent } from './post-editor-element.component';

describe('PostEditorElementComponent', () => {
  let component: PostEditorElementComponent;
  let fixture: ComponentFixture<PostEditorElementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PostEditorElementComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PostEditorElementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

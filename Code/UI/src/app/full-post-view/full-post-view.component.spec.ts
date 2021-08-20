import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FullPostViewComponent } from './full-post-view.component';

describe('FullPostViewComponent', () => {
  let component: FullPostViewComponent;
  let fixture: ComponentFixture<FullPostViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FullPostViewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FullPostViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

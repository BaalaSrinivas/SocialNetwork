import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FeedSectionComponent } from './feed-section.component';

describe('FeedSectionComponent', () => {
  let component: FeedSectionComponent;
  let fixture: ComponentFixture<FeedSectionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FeedSectionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FeedSectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

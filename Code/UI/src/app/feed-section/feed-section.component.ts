import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-feed-section',
  templateUrl: './feed-section.component.html',
  styleUrls: ['./feed-section.component.css']
})
export class FeedSectionComponent implements OnInit {
    constructor() { }
    comment: boolean = false;
    textareaColor: string = '#1A1A1A';
  ngOnInit(): void {
  }

    change() {
        this.comment = !this.comment;
    }
}

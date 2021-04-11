import { Component, OnInit } from '@angular/core';
import { ContentService } from './content.service';

@Component({
    selector: 'app-profile',
    templateUrl: './profile.component.html',
    styleUrls: ['./profile.component.scss']
})

export class ProfileComponent implements OnInit {

    constructor(private contentService: ContentService) { }

    ngOnInit() {
        var data = ['116bbad4-8b14-4e3f-b0f8-28ba03e5e614'];
        this.contentService.getPosts(data).subscribe(s => {
            console.log(s);
        });
    }

}

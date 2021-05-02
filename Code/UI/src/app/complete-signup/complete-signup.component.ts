import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app-complete-signup',
    templateUrl: './complete-signup.component.html',
    styleUrls: ['./complete-signup.component.css']
})
export class CompleteSignupComponent implements OnInit {

    image: any;
    constructor() { }

    ngOnInit(): void {
    }
    previewImage(event): void {
        if (event.target.files && event.target.files[0]) {
            const file = event.target.files[0];

            const reader = new FileReader();
            reader.onload = e => this.image = reader.result;

            reader.readAsDataURL(file);
        }
    }
}

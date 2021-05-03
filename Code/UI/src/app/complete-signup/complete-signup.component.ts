import { Component, OnInit } from '@angular/core';
import { User } from './Models/user.model';
import { UserService } from './user.service';

@Component({
    selector: 'app-complete-signup',
    templateUrl: './complete-signup.component.html',
    styleUrls: ['./complete-signup.component.css']
})
export class CompleteSignupComponent implements OnInit {

    image: any;
    user: User;

    constructor(private _userService: UserService) {
        this.user = new User();
    }

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

    completeSignUp() {
        console.log(this.user);
        this._userService.createUser(this.user).subscribe((data) => {
            console.log(data);
        });
    }
}

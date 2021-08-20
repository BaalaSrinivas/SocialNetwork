import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '../models/user.model';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-complete-register',
  templateUrl: './complete-register.component.html',
  styleUrls: ['./complete-register.component.css']
})
export class CompleteRegisterComponent implements OnInit {

  imagePreview: any;
  profileImage: File
  user: User;

  constructor(private _userService: UserService, private _router: Router) {
    this.user = new User();
  }

  ngOnInit(): void {
  }

  previewImage(event): void {
    if (event.target.files && event.target.files[0]) {

      this.profileImage = event.target.files[0];

      const reader = new FileReader();
      reader.onload = e => this.imagePreview = reader.result;

      reader.readAsDataURL(this.profileImage);
    }
  }

  completeSignUp() {
    this._userService.createUser(this.user, this.profileImage).subscribe((data) => {
      if (data) {
        this._router.navigate(['/profile'], { queryParams: { mailid: sessionStorage.getItem('mailId') }, replaceUrl: true });
      }
    });
  }
}

import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '../models/user.model';
import { AuthenticationService } from '../services/authentication.service';
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
  hasImage: boolean = true;
  showLoading: boolean = false;
  showImageLoading: boolean = false;

  constructor(private _userService: UserService, private _router: Router,
    private authService: AuthenticationService) {

    //Prevent user from entering complete registration page after registeration is done
    if (sessionStorage.getItem('profileUrl') !== null) {
      this._router.navigate(['/profile'], { queryParams: { mailid: sessionStorage.getItem('mailId') }, replaceUrl: true });
    }

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
      this.hasImage = true;
    }
  }
  

  imageValidation(): boolean {
    if (this.profileImage != null && this.profileImage != undefined) {
      return true;
    }
    this.hasImage = false;
    return false;
  }

  completeSignUp() {
    this.showLoading = true;
    this._userService.createUser(this.user, this.profileImage).subscribe((data) => {
      if (data) {
        sessionStorage.setItem('profileUrl', data.ProfileImageUrl);
        this.authService.profileUrlUpdated.emit(true);
        this._router.navigate(['/profile'], { queryParams: { mailid: sessionStorage.getItem('mailId') }, replaceUrl: true });
      }
    });
  }
}

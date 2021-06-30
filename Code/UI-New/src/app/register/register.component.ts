import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Register } from '../models/register.model';
import { AuthenticationService } from '../services/authentication.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  registerModel: Register = new Register();

  constructor(private _authService: AuthenticationService, private router: Router) {
  }

  ngOnInit(): void {
  }

  registerUser() {
    this._authService.registerIdentity(this.registerModel).subscribe(s => {
      console.log(s);
      console.log(s.succeeded == true);
      if (s.succeeded == true) {
        this.router.navigate(['/complete-signup'], { replaceUrl: true });
      }
    });
  }
}

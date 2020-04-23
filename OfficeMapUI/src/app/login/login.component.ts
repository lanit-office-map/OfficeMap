import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { OAuthService } from 'angular-oauth2-oidc';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  public loginForm: FormGroup;
  private isVisible = false;

  constructor(private oauthService: OAuthService,
    private http: HttpClient) {
    this.createLoginForm();
  }

  ngOnInit(): void { 
      
   }

   public login() {
     
    //this.http.post('http://localhost:5000/UserService/Account/Login',)
  }

  // Creates a form for logging in
  private createLoginForm(): void {
    this.loginForm = new FormGroup({
      username: new FormControl(null, Validators.required),
      password: new FormControl(null, Validators.required)
    });
  }

  // Checks if control's input is invalid
  private isControlInvalid(controlName: string): boolean {
    const control = this.loginForm.controls[controlName];
    return control.invalid && control.touched;
  }

  // Shows/hides password
  private showHidePassword() {
    this.isVisible = !this.isVisible;
  }

}

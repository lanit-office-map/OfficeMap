import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-authorization',
  templateUrl: './authorization.component.html',
  styleUrls: ['./authorization.component.scss']
})
export class AuthorizationComponent implements OnInit {

  public loginForm: FormGroup;
  public isVisible = false;

  constructor() {
    this.createLoginForm();
  }

  ngOnInit(): void {  }

  // Creates a form for logging in
  private createLoginForm(): void {
    this.loginForm = new FormGroup({
      username: new FormControl(null, Validators.required),
      password: new FormControl(null, Validators.required)
    });
  }

  // Checks if control's input is invalid
  public isControlInvalid(controlName: string): boolean {
    const control = this.loginForm.controls[controlName];
    return control.invalid && control.touched;
  }

  // Shows/hides password
  public showHidePassword() {
    this.isVisible = !this.isVisible;
  }

  // Checks if submit button must be disabled
  public isSubmitButtonDisabled() {
    const usernameControl = this.loginForm.controls.username;
    const passwordControl = this.loginForm.controls.password;
    return usernameControl.invalid || !usernameControl.touched || passwordControl.invalid || !passwordControl.touched;
  }
}

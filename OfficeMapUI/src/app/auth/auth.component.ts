import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.scss']
})
export class AuthComponent implements OnInit {
  public loginForm: FormGroup;
  private isVisible = false;

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
  private isControlInvalid(controlName: string): boolean {
    const control = this.loginForm.controls[controlName];
    return control.invalid && control.touched;
  }

  // Shows/hides password
  private showHidePassword() {
    this.isVisible = !this.isVisible;
  }
}

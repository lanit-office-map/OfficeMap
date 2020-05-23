import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-top-menu',
  templateUrl: './top-menu.component.html',
  styleUrls: ['./top-menu.component.scss']
})
export class TopMenuComponent implements OnInit {
  public searchForm: FormGroup;

  constructor() {
    this.createSearchForm();
  }

  ngOnInit(): void {
  }

  // Creates a form for search
  private createSearchForm(): void {
    this.searchForm = new FormGroup({
      searchInput: new FormControl(null, Validators.required),
    });
  }

  // Checks if control's input is invalid
  private isControlInvalid(controlName: string): boolean {
    const control = this.searchForm.controls[controlName];
    return !control.touched || control.invalid;
  }
}

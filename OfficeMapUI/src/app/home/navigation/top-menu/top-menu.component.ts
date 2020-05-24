import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MapNameService } from '../../../map-name.service';

@Component({
  selector: 'app-top-menu',
  templateUrl: './top-menu.component.html',
  styleUrls: ['./top-menu.component.scss']
})
export class TopMenuComponent implements OnInit {

  private searchForm: FormGroup;
  private mapName: string;

  constructor(private mapNameService: MapNameService) {
    this.createSearchForm();
  }

  ngOnInit(): void {
    this.mapNameService.mapName.subscribe(mapName => this.mapName = mapName);
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

import { Component, OnInit } from '@angular/core';
import { OfficesService } from '../../../offices.service';

@Component({
  selector: 'app-left-menu',
  templateUrl: './left-menu.component.html',
  styleUrls: ['./left-menu.component.scss']
})
export class LeftMenuComponent implements OnInit {

  public offices;

  constructor(private officesService: OfficesService) {
  }

  ngOnInit(): void {
    this.officesService.getOffices().subscribe(
      offices => {
        this.offices = offices;
      }
    );
  }
}

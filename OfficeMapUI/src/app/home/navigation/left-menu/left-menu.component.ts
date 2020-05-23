import { Component, OnInit } from '@angular/core';
import { OfficesService } from '../../../offices.service';

@Component({
  selector: 'app-left-menu',
  templateUrl: './left-menu.component.html',
  styleUrls: ['./left-menu.component.scss']
})
export class LeftMenuComponent implements OnInit {

  public offices: JSON[];

  constructor(private officesService: OfficesService) {
    this.offices = new Array<JSON>();
  }

  ngOnInit(): void {
    this.officesService.getOffices().subscribe(
      office => {
        this.offices.push(office);
      }
    );
  }

}

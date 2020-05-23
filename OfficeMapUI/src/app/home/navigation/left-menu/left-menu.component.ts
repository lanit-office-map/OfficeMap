import { Component, OnInit } from '@angular/core';
import { OfficesService } from '../../../offices.service';

@Component({
  selector: 'app-left-menu',
  templateUrl: './left-menu.component.html',
  styleUrls: ['./left-menu.component.scss']
})
export class LeftMenuComponent implements OnInit {

  public offices;
//  public streets: string[] = [];

  constructor(private officesService: OfficesService) {
  }

  ngOnInit(): void {
    this.officesService.getOffices().subscribe(
      offices => {
        this.offices = offices;
        // const officesArray = JSON.parse(JSON.stringify(this.offices));
        // for (const office of officesArray) {
        //   this.streets.push(office.street);
        // }
      }
    );
  }
}

// interface Office {
//   guid: string;
//   officeId: number;
//   city: string;
//   street: string;
//   house: string;
//   building: string;
//   phoneNumber: string;
// }

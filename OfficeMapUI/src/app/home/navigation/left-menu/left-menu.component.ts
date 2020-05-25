import { Component, OnInit } from '@angular/core';
import { OfficesService } from '../../../offices.service';
import { MapNameService } from '../../../map-name.service';

@Component({
  selector: 'app-left-menu',
  templateUrl: './left-menu.component.html',
  styleUrls: ['./left-menu.component.scss']
})
export class LeftMenuComponent implements OnInit {

  public offices;

  constructor(private officesService: OfficesService, private mapNameService: MapNameService) {
  }

  ngOnInit(): void {
    this.officesService.getOffices().subscribe(
      offices => {
        this.offices = offices;
      }
    );
  }

  // Passes map's name after button's click
  public onClick(mapName: string): void {
    this.mapNameService.passMapName(mapName);
  }
}

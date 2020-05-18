import { Component, OnInit } from '@angular/core';
import { MapsService } from '../../maps.service';
import { ViewChild, ElementRef } from '@angular/core';

@Component({
  selector: 'app-map-view',
  templateUrl: './map-view.component.html',
  styleUrls: ['./map-view.component.scss']
})
export class MapViewComponent implements OnInit {

  @ViewChild('dataContainer') dataContainer: ElementRef;
  public maps;
  public testhtml;
  constructor(private mapsService: MapsService) {
  }
  // l1, l2 - уровни вложенности помещений, пока работаем с двумя
  public mapBuilder() {
    this.testhtml = '<svg width="915" height="715">';
    this.maps.forEach((elementL1) => {
      if (!!elementL1.map) {
        this.testhtml += elementL1.map.content;
        // this.testhtml += '<h1>added l1 content</h1> ';
      }
      if (!!elementL1.spaces) {
        elementL1.spaces.forEach((elementL2) => {
          this.testhtml += elementL2.map.content;
          // this.testhtml += '<h2>added l2 content</h2> ';
        });
      }
    });
    this.testhtml += '</svg>';
    console.log(this.testhtml);
    this.dataContainer.nativeElement.innerHTML = this.testhtml;
  }

  public clickFromMap() {
    console.log('click from map');
  }

  ngOnInit(): void {
    this.mapsService.getMaps(1).subscribe(maps => {
      this.maps = maps;
      this.mapBuilder();
    });
  }
}

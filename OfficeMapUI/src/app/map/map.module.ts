import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MapRoutingModule } from './map-routing.module';
import { MapViewComponent } from './map-view/map-view.component';
import { ModalBookComponent } from './modal-book/modal-book.component';
import { ModalUserComponent } from './modal-user/modal-user.component';
import {MapsService} from '../maps.service';


@NgModule({
  declarations: [
    MapViewComponent,
    ModalBookComponent,
    ModalUserComponent,
  ],
  imports: [
    CommonModule,
    MapRoutingModule
  ],
  providers: [
    MapsService
  ]
})
export class MapModule { }

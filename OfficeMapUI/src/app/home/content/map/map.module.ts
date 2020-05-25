import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MapRoutingModule } from './map-routing.module';
import { MapViewComponent } from './map-view/map-view.component';
import { ModalBookComponent } from './modal-book/modal-book.component';
import { ModalUserComponent } from './modal-user/modal-user.component';


@NgModule({
  declarations: [
    MapViewComponent,
    ModalBookComponent,
    ModalUserComponent,
  ],
  exports: [
    MapViewComponent
  ],
  imports: [
    CommonModule,
    MapRoutingModule
  ]
})
export class MapModule { }

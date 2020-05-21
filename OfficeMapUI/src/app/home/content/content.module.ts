import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ContentRoutingModule } from './content-routing.module';
import { AdminModule } from './admin/admin.module';
import { MapModule } from './map/map.module';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    ContentRoutingModule,
    AdminModule,
    MapModule
  ]
})
export class ContentModule { }

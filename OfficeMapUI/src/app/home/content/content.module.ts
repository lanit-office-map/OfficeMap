import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ContentRoutingModule } from './content-routing.module';
import { AdminModule } from './admin/admin.module';
import { MapModule } from './map/map.module';
import { ContentComponent } from './content.component';


@NgModule({
  declarations: [
    ContentComponent
  ],
  exports: [
    ContentComponent
  ],
  imports: [
    CommonModule,
    ContentRoutingModule,
    AdminModule,
    MapModule
  ]
})
export class ContentModule { }

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminRoutingModule } from './admin-routing.module';
import { EmployeeListComponent } from './employee-list/employee-list.component';
import { MapEditorComponent } from './map-editor/map-editor.component';
import { OfficeListComponent } from './office-list/office-list.component';


@NgModule({
  declarations: [
    EmployeeListComponent,
    MapEditorComponent,
    OfficeListComponent,
  ],
  imports: [
    CommonModule,
    AdminRoutingModule
  ]
})
export class AdminModule { }

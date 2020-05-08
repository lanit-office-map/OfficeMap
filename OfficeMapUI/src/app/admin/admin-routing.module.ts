import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NotFoundComponent } from '../not-found/not-found.component';
import { EmployeeListComponent } from './employee-list/employee-list.component';
import { OfficeListComponent } from './office-list/office-list.component';
import {MapEditorComponent} from './map-editor/map-editor.component';


const routes: Routes = [
  { path: '', component: NotFoundComponent },
  { path: 'employees', component: EmployeeListComponent },
  { path: 'offices',
    component: OfficeListComponent,
    children: [{ path: ':id', component: MapEditorComponent }]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }

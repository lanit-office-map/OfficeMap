import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { MapViewComponent } from './map-view/map-view.component';
import { ModalBookComponent } from './modal-book/modal-book.component';
import { ModalUserComponent } from './modal-user/modal-user.component';
import { SettingsComponent } from './settings/settings.component';
import { LeftMenuComponent } from './left-menu/left-menu.component';
import { OfficeListComponent } from './office-list/office-list.component';
import { EmployeeListComponent } from './employee-list/employee-list.component';
import { MapEditorComponent } from './map-editor/map-editor.component';

const routes: Routes = [
  { path: 'home', component: LeftMenuComponent },
  { path: 'map/:id', component: MapViewComponent },
  { path: 'map/:id/book_room/:roomId', component: ModalBookComponent },
  { path: 'map/:id/user_info/:userId', component: ModalUserComponent },
  { path: 'settings', component: SettingsComponent },
  { path: 'offices', component: OfficeListComponent },
  { path: 'users', component: EmployeeListComponent },
  { path: 'editor/:id', component: MapEditorComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

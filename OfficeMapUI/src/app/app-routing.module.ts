import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { MapViewComponent } from './map/map-view/map-view.component';
import { ModalBookComponent } from './map/modal-book/modal-book.component';
import { ModalUserComponent } from './map/modal-user/modal-user.component';
import { SettingsComponent } from './navigation/settings/settings.component';
import { LeftMenuComponent } from './navigation/left-menu/left-menu.component';
import { OfficeListComponent } from './office-list/office-list.component';
import { EmployeeListComponent } from './employee-list/employee-list.component';
import { MapEditorComponent } from './map/map-editor/map-editor.component';
import { AuthComponent } from './auth/auth.component';
import { NotFoundComponent } from './not-found/not-found.component';

const routes: Routes = [
  { path: '', component: AuthComponent },
  { path: 'home', component: LeftMenuComponent },
  { path: 'map/:id', component: MapViewComponent },
  { path: 'map/:id/book_room/:roomId', component: ModalBookComponent },
  { path: 'map/:id/user_info/:userId', component: ModalUserComponent },
  { path: 'settings', component: SettingsComponent },
  { path: 'offices', component: OfficeListComponent },
  { path: 'users', component: EmployeeListComponent },
  { path: 'editor/:id', component: MapEditorComponent },
  { path: '**', component: NotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

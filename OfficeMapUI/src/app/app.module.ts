import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { RouterModule } from '@angular/router';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AuthComponent } from './authorization/auth/auth.component';
import { LeftMenuComponent } from './navigation/left-menu/left-menu.component';
import { TopMenuComponent } from './navigation/top-menu/top-menu.component';
import { SearchComponent } from './search/search.component';
import { OfficeListComponent } from './admin/office-list/office-list.component';
import { EmployeeListComponent } from './admin/employee-list/employee-list.component';
import { MapViewComponent } from './map/map-view/map-view.component';
import { MapEditorComponent } from './admin/map-editor/map-editor.component';
import { ModalUserComponent } from './map/modal-user/modal-user.component';
import { ModalBookComponent } from './map/modal-book/modal-book.component';
import { SettingsComponent } from './navigation/settings/settings.component';
import { NotFoundComponent } from './not-found/not-found.component';
import {AdminModule} from './admin/admin.module';
import {MapModule} from './map/map.module';
import {AuthorizationModule} from './authorization/authorization.module';

@NgModule({
  declarations: [
    AppComponent,
    AuthComponent,
    LeftMenuComponent,
    TopMenuComponent,
    SearchComponent,
    OfficeListComponent,
    EmployeeListComponent,
    MapViewComponent,
    MapEditorComponent,
    ModalUserComponent,
    ModalBookComponent,
    SettingsComponent,
    NotFoundComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    AdminModule,
    MapModule,
    AuthorizationModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

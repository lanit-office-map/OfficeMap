import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { RouterModule } from '@angular/router';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AuthComponent } from './auth/auth.component';
import { LeftMenuComponent } from './left-menu/left-menu.component';
import { TopMenuComponent } from './top-menu/top-menu.component';
import { SearchComponent } from './search/search.component';
import { OfficeListComponent } from './office-list/office-list.component';
import { EmployeeListComponent } from './employee-list/employee-list.component';
import { MapViewComponent } from './map-view/map-view.component';
import { MapEditorComponent } from './map-editor/map-editor.component';
import { ModalUserComponent } from './modal-user/modal-user.component';
import { ModalBookComponent } from './modal-book/modal-book.component';
import { SettingsComponent } from './settings/settings.component';

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
    RouterModule,
    SettingsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

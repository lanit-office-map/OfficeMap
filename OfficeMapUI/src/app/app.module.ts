import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { RouterModule } from '@angular/router';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AuthComponent } from './auth/auth.component';
import { LeftMenuComponent } from './navigation/left-menu/left-menu.component';
import { TopMenuComponent } from './navigation/top-menu/top-menu.component';
import { SearchComponent } from './search/search.component';
import { OfficeListComponent } from './office-list/office-list.component';
import { EmployeeListComponent } from './employee-list/employee-list.component';
import { MapViewComponent } from './map-view/map-view.component';
import { MapEditorComponent } from './map-editor/map-editor.component';
import { ModalUserComponent } from './modal-user/modal-user.component';
import { ModalBookComponent } from './modal-book/modal-book.component';
import { SettingsComponent } from './navigation/settings/settings.component';
import { NotFoundComponent } from './not-found/not-found.component';

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
    RouterModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

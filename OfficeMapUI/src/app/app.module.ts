import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterModule } from '@angular/router';
import { MatSidenavModule } from '@angular/material/sidenav';

import { AppRoutingModule } from './app-routing.module';
import { AdminModule } from './home/content/admin/admin.module';
import { MapModule } from './home/content/map/map.module';
import { AuthorizationModule } from './authorization/authorization.module';
import { HomeComponent } from './home/home.component';
import { ContentModule } from './home/content/content.module';
import { NavigationModule } from './home/navigation/navigation.module';
import { HomeModule } from './home/home.module';

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LeftMenuComponent } from './home/navigation/left-menu/left-menu.component';
import { TopMenuComponent } from './home/navigation/top-menu/top-menu.component';
import { SettingsComponent } from './home/navigation/settings/settings.component';
import { NotFoundComponent } from './not-found/not-found.component';


@NgModule({
  declarations: [
    AppComponent,
    LeftMenuComponent,
    TopMenuComponent,
    SettingsComponent,
    NotFoundComponent,
    HomeComponent
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
    AuthorizationModule,
    MatExpansionModule,
    MatToolbarModule,
    MatSidenavModule,
    ContentModule,
    NavigationModule,
    HomeModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

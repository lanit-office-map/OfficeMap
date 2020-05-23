import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { NavigationRoutingModule } from './navigation-routing.module';
import { LeftMenuComponent } from './left-menu/left-menu.component';
import { TopMenuComponent } from './top-menu/top-menu.component';
import { SettingsComponent } from './settings/settings.component';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { ReactiveFormsModule } from '@angular/forms';
import { OfficesService } from '../../offices.service';


@NgModule({
  declarations: [
    LeftMenuComponent,
    TopMenuComponent,
    SettingsComponent
  ],
  exports: [
    TopMenuComponent,
    LeftMenuComponent
  ],
  imports: [
    CommonModule,
    NavigationRoutingModule,
    MatSidenavModule,
    MatExpansionModule,
    MatToolbarModule,
    MatButtonModule,
    ReactiveFormsModule
  ],
  providers: [
    OfficesService
  ]
})
export class NavigationModule { }

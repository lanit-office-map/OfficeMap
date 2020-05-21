import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { NavigationRoutingModule } from './navigation-routing.module';
import { LeftMenuComponent } from './left-menu/left-menu.component';
import { TopMenuComponent } from './top-menu/top-menu.component';
import { SettingsComponent } from './settings/settings.component';


@NgModule({
  declarations: [
    LeftMenuComponent,
    TopMenuComponent,
    SettingsComponent
  ],
  exports: [
    TopMenuComponent
  ],
  imports: [
    CommonModule,
    NavigationRoutingModule
  ]
})
export class NavigationModule { }

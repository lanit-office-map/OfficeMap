import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HomeRoutingModule } from './home-routing.module';
import { NavigationModule } from './navigation/navigation.module';
import { ContentModule } from './content/content.module';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    HomeRoutingModule,
    NavigationModule,
    ContentModule
  ]
})
export class HomeModule { }

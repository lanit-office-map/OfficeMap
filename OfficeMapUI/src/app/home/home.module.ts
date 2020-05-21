import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HomeRoutingModule } from './home-routing.module';
import { NavigationModule } from './navigation/navigation.module';
import { ContentModule } from './content/content.module';
import { HttpClientModule } from '@angular/common/http';
import { HomeComponent } from './home.component';


@NgModule({
  declarations: [
    HomeComponent
  ],
  imports: [
    CommonModule,
    HttpClientModule,
    HomeRoutingModule,
    NavigationModule,
    ContentModule
  ]
})
export class HomeModule { }

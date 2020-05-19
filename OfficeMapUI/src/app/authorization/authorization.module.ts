import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthorizationRoutingModule } from './authorization-routing.module';
import { AuthCallbackComponent } from './auth-callback/auth-callback.component';
import {HttpClientModule} from '@angular/common/http';


@NgModule({
  declarations: [
    AuthCallbackComponent,
  ],
  imports: [
    CommonModule,
    AuthorizationRoutingModule,
    HttpClientModule
  ]
})
export class AuthorizationModule { }

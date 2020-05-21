import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AuthorizationRoutingModule } from './authorization-routing.module';
import { AuthCallbackComponent } from './auth-callback/auth-callback.component';
import { AuthorizationComponent } from './authorization.component';


@NgModule({
  declarations: [
    AuthCallbackComponent,
    AuthorizationComponent,
  ],
  imports: [
    CommonModule,
    AuthorizationRoutingModule
  ]
})
export class AuthorizationModule { }

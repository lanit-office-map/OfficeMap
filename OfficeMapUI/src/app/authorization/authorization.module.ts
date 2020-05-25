import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AuthorizationRoutingModule } from './authorization-routing.module';
import { AuthCallbackComponent } from './auth-callback/auth-callback.component';
import { AuthorizationComponent } from './authorization.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';


@NgModule({
  declarations: [
    AuthCallbackComponent,
    AuthorizationComponent,
  ],
  imports: [
    CommonModule,
    AuthorizationRoutingModule,
    ReactiveFormsModule,
    MatButtonModule
  ]
})
export class AuthorizationModule { }

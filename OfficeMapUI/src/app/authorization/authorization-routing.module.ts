import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { NotFoundComponent } from '../not-found/not-found.component';
import { AuthCallbackComponent } from './auth-callback/auth-callback.component';


const routes: Routes = [
  { path: '', component: AuthCallbackComponent },
  { path: '**', component: NotFoundComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthorizationRoutingModule { }

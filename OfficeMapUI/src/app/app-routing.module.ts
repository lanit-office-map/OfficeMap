import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { NotFoundComponent } from './not-found/not-found.component';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { AuthorizationComponent } from './authorization/authorization.component';


const routes: Routes = [
//  { path: 'home', component: HomeComponent },
  { path: '', component: AppComponent },
  { path: 'auth', component: AuthorizationComponent },
  { path: 'home', loadChildren: () => import('./home/home.module').then(m => m.HomeModule) },
//  { path: '', redirectTo: '', pathMatch: 'full'},
  { path: '**', component: NotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

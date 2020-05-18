import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { NotFoundComponent } from './not-found/not-found.component';
import { SearchComponent } from './search/search.component';
import { SettingsComponent } from './navigation/settings/settings.component';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';

const routes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: '', component: AppComponent },
  { path: 'search', component: SearchComponent },
  { path: 'settings', component: SettingsComponent},
  { path: 'auth', loadChildren: './authorization/authorization.module#AuthorizationModule' },
  { path: 'map', loadChildren: './map/map.module#MapModule'},
  { path: 'admin', loadChildren: './admin/admin.module#AdminModule'},
  { path: '**', component: NotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

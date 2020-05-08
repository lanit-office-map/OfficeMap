import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { LeftMenuComponent } from './navigation/left-menu/left-menu.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { SearchComponent } from './search/search.component';
import { SettingsComponent } from './navigation/settings/settings.component';

const routes: Routes = [
  { path: 'home', component: LeftMenuComponent },
  { path: '', component: LeftMenuComponent },
  { path: 'search', component: SearchComponent },
  { path: 'settings', component: SettingsComponent},
  { path: 'auth', loadChildren: './authorization/authorization.module#AuthorizationModule' },
  { path: 'map', loadChildren: './map/map.module#MapModule'},
  { path: 'admin', loadChildren: './admin/admin.module#AdminModule'},
  { path: '**', component: NotFoundComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { SettingsComponent } from './navigation/settings/settings.component';
import { HomeComponent } from './home.component';


const routes: Routes = [
  { path: '', component: HomeComponent },
//  { path: 'settings', loadChildren: () => import('./navigation/settings/settings.module').then(m => m.HomeModule) },
  { path: 'settings', component: SettingsComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HomeRoutingModule { }

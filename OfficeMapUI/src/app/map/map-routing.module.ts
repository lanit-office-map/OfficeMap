import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {NotFoundComponent} from '../not-found/not-found.component';
import {ModalBookComponent} from './modal-book/modal-book.component';
import {ModalUserComponent} from './modal-user/modal-user.component';
import {MapViewComponent} from './map-view/map-view.component';


const routes: Routes = [
  { path: '', component: NotFoundComponent },
  {
    path: ':id',
    component: MapViewComponent,
    children: [{
      path: 'book_room',
      redirectTo: '',
      pathMatch: 'full',
      children: [{path: ':roomId', component: ModalBookComponent}]
    }, {
      path: 'user_info',
      redirectTo: '',
      pathMatch: 'full',
      children: [{path: ':userId', component: ModalUserComponent}]
    }]
  },
  { path: '**', component: NotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MapRoutingModule { }

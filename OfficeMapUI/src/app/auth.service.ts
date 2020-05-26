import { Injectable } from '@angular/core';
import { UserManager, UserManagerSettings, User, WebStorageStateStore } from 'oidc-client';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  // tslint:disable-next-line:variable-name
  private _authNavStatusSource = new BehaviorSubject<boolean>(false);
  private manager = new UserManager(getClientSettings());
  private user: User | null;
  constructor() {
    this.manager.getUser().then(user => {
       this.user = user;
       this._authNavStatusSource.next(this.isAuthenticated());
    });
  }

  isAuthenticated(): boolean {
    return this.user != null && !this.user.expired;
  }

  login() {
    return this.manager.signinRedirect();
  }

  async completeAuthentication() {
    this.user = await this.manager.signinRedirectCallback();
    this._authNavStatusSource.next(this.isAuthenticated());
}

async signout() {
    await this.manager.signoutRedirect();
  }
}



export function getClientSettings(): UserManagerSettings {
  return {
      authority: 'http://localhost:5000',
      client_id: 'angular.client',
      redirect_uri: 'http://localhost:4200/auth',
      post_logout_redirect_uri: 'http://localhost:4200/home',
      response_type: 'id_token token',
      scope: 'openid email offline_access UserService OfficeService SpaceService WorkplaceService',
      userStore: new WebStorageStateStore({ store: window.localStorage })
  };
}

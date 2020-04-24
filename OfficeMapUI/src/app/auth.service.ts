import { Injectable } from '@angular/core';
import { UserManager, UserManagerSettings, User, WebStorageStateStore } from 'oidc-client';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs'; 

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private _authNavStatusSource = new BehaviorSubject<boolean>(false);
  private manager = new UserManager(getClientSettings());
  private user: User | null;
  constructor(private http: HttpClient) {    
       
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
      client_id: 'service.client',
      redirect_uri: 'http://localhost:4200/auth-callback',   
      response_type:"id_token token",
      scope:"openid email offline_access officemapapis" ,
      userStore: new WebStorageStateStore({ store: window.localStorage })    
  };
}

import { AuthConfig } from 'angular-oauth2-oidc';

export const authConfig: AuthConfig = {
  issuer: 'http://localhost:5000',
  redirectUri: window.location.origin + '/index.html',
  clientId: 'service.client',
  responseType: 'id_token token',
  scope: 'openid email offline_access officemapapis'
};
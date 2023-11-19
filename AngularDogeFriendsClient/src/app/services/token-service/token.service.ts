import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  private readonly ACCESS_TOKEN_KEY = 'access_token';
  private readonly REFRESH_TOKEN_KEY = 'refresh_token';
  private readonly USERNAME_KEY = 'username';
  private readonly ROLES_KEY = 'roles';

  constructor() { }

  public saveTokens(accessToken: string, refreshToken: string): void {
    localStorage.setItem(this.ACCESS_TOKEN_KEY, accessToken);
    localStorage.setItem(this.REFRESH_TOKEN_KEY, refreshToken);
  }

  public saveUserInfoFromAccessToken(): void {
    const accessToken = localStorage.getItem(this.ACCESS_TOKEN_KEY);
    if (accessToken) {
      const tokenPayload = this.decodeTokenPayload(accessToken);
      if (tokenPayload) {
        localStorage.setItem(this.USERNAME_KEY, tokenPayload.username);
        localStorage.setItem(this.ROLES_KEY, JSON.stringify(tokenPayload.roles));
      }
    }
  }

  public getUsername(): string | null {
    return localStorage.getItem(this.USERNAME_KEY);
  }

  public getRoles(): string[] {
    const roles = localStorage.getItem(this.ROLES_KEY);
    return roles ? JSON.parse(roles) : [];
  }

  private decodeTokenPayload(token: string): any {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function(c) {
      return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));
    return JSON.parse(jsonPayload);
  }
}

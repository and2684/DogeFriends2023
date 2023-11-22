import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  private readonly ACCESS_TOKEN_KEY = 'access_token';
  private readonly REFRESH_TOKEN_KEY = 'refresh_token';
  private readonly USERNAME_KEY = 'username';
  private readonly ROLES_KEY = 'roles';
  private usernameSource = new BehaviorSubject<string | null>(null);
  username$ = this.usernameSource.asObservable();

  constructor(private router: Router) {
    const storedUsername = this.getUsername();
    this.usernameSource = new BehaviorSubject<string | null>(storedUsername);
    this.username$ = this.usernameSource.asObservable();
  }

  public saveTokens(accessToken: string, refreshToken: string): void {
    localStorage.setItem(this.ACCESS_TOKEN_KEY, accessToken);
    localStorage.setItem(this.REFRESH_TOKEN_KEY, refreshToken);
  }

  public saveUserInfoFromAccessToken(): void {
    const accessToken = localStorage.getItem(this.ACCESS_TOKEN_KEY);
    if (accessToken) {
      const tokenPayload = this.decodeTokenPayload(accessToken);
      if (tokenPayload) {
        const username = tokenPayload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];
        localStorage.setItem(this.USERNAME_KEY, username);
        this.usernameSource.next(username);

        const rolesArray = tokenPayload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
        localStorage.setItem(this.ROLES_KEY, JSON.stringify(rolesArray));

        if (username) {
          this.router.navigate([`/user/${username}`]);
        }
      }
    }
  }

  public getUsername(): string | null {
    let username = localStorage.getItem(this.USERNAME_KEY);
    console.log('Получено имя пользователя из TokenService: ' + username);

    return username ? username : null;
  }

  public getRoles(): string[] {
    const roles = localStorage.getItem(this.ROLES_KEY);
    return roles ? JSON.parse(roles) : [];
  }

  private decodeTokenPayload(token: string): any {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function (c) {
      return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));
    return JSON.parse(jsonPayload);
  }
}

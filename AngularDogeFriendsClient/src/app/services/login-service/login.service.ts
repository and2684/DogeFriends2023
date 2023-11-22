import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { LoginDto } from '../../models/LoginDto';
import { UserLoginResponseDto } from '../../models/UserLoginResponseDto';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  url: string = 'https://localhost:7275/';

  constructor(private http: HttpClient) { }

  login(loginData: LoginDto): Observable<UserLoginResponseDto> {
    let response = this.http.post<UserLoginResponseDto>(this.url + 'api/users/login', loginData);
    return response;
  }
}

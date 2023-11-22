import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserInfoDto } from 'src/app/models/UserInfoDto';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  url: string = 'https://localhost:7275/api/users';

  constructor(private http: HttpClient) { }

  getUserById(id: number) {
    let user = this.http.get<UserInfoDto>(`${this.url}/${id}`);
    return user;
  }

  getUserByUsername(username: string){
    let user = this.http.get<UserInfoDto>(`${this.url}/${username}`);
    return user;
  }
}

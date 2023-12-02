import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserInfoDto } from 'src/app/models/UserInfoDto';

@Injectable({
  providedIn: 'root'
})
export class FriendsService {

  url: string = 'https://localhost:7275/api/Friendships';

  constructor(private http: HttpClient) { }

  // Друзья
  getUserFriends(userId: number) {
    var params = new HttpParams().set('userId', userId);
    return this.http.get<UserInfoDto[]>(`${this.url}/friends`, { params: params });
  }

  // Подписчики
  getUserSubs(userId: number) {
    var params = new HttpParams().set('userId', userId);

    return this.http.get<UserInfoDto[]>(`${this.url}/subs`, { params: params });
  }

  // Подписки
  getUserSubscriptions(userId: number) {
    var params = new HttpParams().set('userId', userId);
    return this.http.get<UserInfoDto[]>(`${this.url}/subscriptions`, { params: params });
  }

  // Подписаться
  sub(userId: number, friendId: number) {
    var params = new HttpParams().set('userId', userId).set('friendId', friendId);
    return this.http.post(`${this.url}/sub`, { params: params });
  }

  // Отписаться
  unsub(userId: number, friendId: number) {
    var params = new HttpParams().set('userId', userId).set('friendId', friendId);
    return this.http.delete(`${this.url}/unsub`, { params: params });
  }
}

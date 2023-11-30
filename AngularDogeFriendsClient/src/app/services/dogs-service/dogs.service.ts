import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DogDto } from 'src/app/models/DogDto';

@Injectable({
  providedIn: 'root'
})
export class DogsService {

  url: string = 'https://localhost:7275/api/dogs';

  constructor(private http: HttpClient) { }

  getUserDogs(username: string) {
    let params = new HttpParams().set('username', username);
    return this.http.get<DogDto[]>(`${this.url}/user`, { params: params });
  }
}

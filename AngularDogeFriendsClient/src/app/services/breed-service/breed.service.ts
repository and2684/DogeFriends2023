import { IBreed } from 'src/app/models/Breeds';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';


@Injectable({
  providedIn: 'root'
})
export class BreedService {

  url: string = 'https://localhost:7275/api/Breeds'

  constructor(private http: HttpClient) { }

  getBreeds() {
    let breeds = this.http.get<IBreed[]>(this.url);
    return breeds;
  }

  getBreed(id: number) {
    let breed = this.http.get<IBreed>(`${this.url}/${id}`);
    return breed;
  }
}

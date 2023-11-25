import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IBreedGroup, ICoat, ISize } from 'src/app/models/Directory';

@Injectable({
  providedIn: 'root'
})
// Сервис для получения инфы из справочников
export class DirectoryService {

  url: string = 'https://localhost:7275/api'

  constructor(private http: HttpClient) { }

  getCoats() {
    let coats = this.http.get<ICoat[]>(`${this.url}/coats`);
    return coats;
  }

  getSizes() {
    let sizes = this.http.get<ISize[]>(`${this.url}/sizes`);
    return sizes;
  }

  getBreedGroups() {
    let breedGroups = this.http.get<IBreedGroup[]>(`${this.url}/breeds/breedgroups`);
    return breedGroups;
  }
}

import { IBreed, IBreedUpdate } from 'src/app/models/Breeds';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';


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

  updateBreed(id: number, updatedBreed: IBreedUpdate) {
    var updateUrl = `${this.url}/${id}`;
    //console.log(updateUrl);

    var jsonBreed = JSON.stringify(updatedBreed);
    //console.log(jsonBreed);

    var res = this.http.put(updateUrl, jsonBreed, {headers: {'Content-Type': 'application/json'}});
    return res;
  }

  deleteCascadeBreed(id: number)  {
    var res = this.http.delete(`${this.url}/deletecascade/${id}`);
    return res;
  }
}

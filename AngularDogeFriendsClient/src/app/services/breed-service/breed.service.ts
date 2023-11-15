import { Images } from './../../models/images';
import { ImageService } from './../image-service/image.service';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IBreed } from 'src/app/models/breeds';
import { Observable, forkJoin, lastValueFrom, map, switchMap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BreedService {

  url: string = 'https://localhost:7275/api/Breeds'

  constructor(private http: HttpClient, private imageService: ImageService) { }

  /*
  async getBreeds() {
    try {
        let breeds = await lastValueFrom(this.http.get<IBreed[]>(this.url));
        for (let breed of breeds!) {
            var image = await lastValueFrom(this.imageService.getMainImage(breed.externalId, 'Breed'));
            breed.images! = [image!];
        }
        return breeds;
    } catch (error) {
        console.error('Произошла ошибка при получении пород:', error);
        throw error;
    }
}
*/

  getBreeds() {
    let breeds = this.http.get<IBreed[]>(this.url);
    return breeds;
  }

  getBreed(id: number) {
    let breed = this.http.get<IBreed>(`${this.url}/${id}`);
    return breed;
  }
}

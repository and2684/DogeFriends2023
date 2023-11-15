import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IImage } from 'src/app/models/images';

@Injectable({
  providedIn: 'root'
})
export class ImageService {

  url: string = 'https://localhost:5201/api/images'

  constructor(private http: HttpClient) { }

  getAllImages(uid: string, entityName: string) {
    const headers = new HttpHeaders()
      .set('UID', uid)
      .set('EntityName', entityName);

    return this.http.get<IImage[]>(`${this.url}/getall`, { headers });
  }

  getMainImage(uid: string, entityName: string) {
    const headers = new HttpHeaders()
      .set('UID', uid)
      .set('EntityName', entityName);

    return this.http.get<IImage>(`${this.url}/getmain`, { headers });
  }
}

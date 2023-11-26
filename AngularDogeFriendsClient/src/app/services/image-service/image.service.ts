import { firstValueFrom } from 'rxjs';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IImage } from 'src/app/models/Images';

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

  setMainImage(uid: string, entityName: string, imageId: string) {
    const params = new HttpParams()
      .set('uid', uid)
      .set('entityname', entityName)
      .set('imageId', imageId);

    return this.http.post(`${this.url}/setmain`, {}, { params: params });
  }

  removeImage(uid: string, entityName: string, imageId: string) {
    const params = new HttpParams()
      .set('uid', uid)
      .set('entityname', entityName)
      .set('imageId', imageId);

    return this.http.delete(`${this.url}/remove`, { params: params });
  }

  addImage(addImage: IImage) {
    var content = JSON.stringify(addImage);

    return this.http.post<string>(`${this.url}/add`, content, {
      headers: { 'Content-Type': 'application/json' },
      responseType: 'text' as 'json'
    });
  }

  async selectImageFile(): Promise<string> {
    return new Promise((resolve, reject) => {
      const input = document.createElement('input');
      input.type = 'file';
      input.accept = 'image/*';
      input.onchange = (event) => {
        const file = (event.target as HTMLInputElement).files?.[0];
        const reader = new FileReader();
        reader.onloadend = () => resolve(reader.result as string);
        reader.onerror = reject;
        reader.readAsDataURL(file!);
      };
      input.click();
    });
  }
}

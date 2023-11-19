import { BreedService } from './../../services/breed-service/breed.service';
import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { IBreed } from 'src/app/models/breeds';
import { IImage } from 'src/app/models/Images';
import { ImageService } from 'src/app/services/image-service/image.service';

@Component({
  selector: 'app-breed-details',
  templateUrl: './breed-details.component.html',
  styleUrls: ['./breed-details.component.css'],
})
export class BreedDetailsComponent implements OnInit {
deleteImage(_t33: any) {
throw new Error('Method not implemented.');
}
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private breedService: BreedService,
    private imageService: ImageService,
    private dialog: MatDialogRef<BreedDetailsComponent> // Внедряем MatDialogRef
  ) {}

  breed: IBreed;

  imageObject: Array<object> = [];

  ngOnInit() {
    this.breedService.getBreed(this.data.breedId).subscribe((breed) => {
      this.breed = breed;

      this.imageService
        .getAllImages(this.breed.externalId, 'Breed')
        .subscribe((images: IImage[]) => {
          this.breed.images = images;

          // Преобразуем каждый объект изображения в формат imageObject
          this.imageObject = this.breed.images.map((image: IImage) => ({
            image: image.base64Data,
            thumbImage: image.base64Data,
            title: '',
            alt: ''
          }));
        });
    });
  }


  closeDialog() {
    this.dialog.close();
  }
}

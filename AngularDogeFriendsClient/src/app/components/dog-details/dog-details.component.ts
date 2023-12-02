import { Component, OnInit, ViewEncapsulation, Inject } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { firstValueFrom } from 'rxjs';
import { IBreed } from 'src/app/models/Breeds';
import { DogDtoDetailed } from 'src/app/models/DogDto';
import { IImage } from 'src/app/models/Images';
import { BreedService } from 'src/app/services/breed-service/breed.service';
import { ImageService } from 'src/app/services/image-service/image.service';

@Component({
  selector: 'app-dog-details',
  templateUrl: './dog-details.component.html',
  styleUrls: ['./dog-details.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class DogDetailsComponent implements OnInit {

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private imageService: ImageService,
    private breedService: BreedService,
    private dialog: MatDialogRef<DogDetailsComponent>,
    private formBuilder: FormBuilder) {
  }

  dog: DogDtoDetailed = this.data.dog;
  breed: IBreed;
  imageObject: Array<object> = [];

  async ngOnInit() {
    this.dog.images = await firstValueFrom(this.imageService.getAllImages(this.dog.externalId, 'Dog'));

    this.imageObject = this.dog.images.map((image: IImage) => ({
      image: image.base64Data,
      thumbImage: image.base64Data,
      title: '',
      alt: ''
    }));

    this.breed = await firstValueFrom(this.breedService.getBreed(this.dog.dogBreedId));
    if (!!this.breed) {
      this.dog.coat = this.breed.coat;
      this.dog.size = this.breed.size;
      this.dog.breedDescription = this.breed.description;
      this.dog.breedGroups = this.breed.breedGroups;
    }

    console.log(this.dog);
  }

}

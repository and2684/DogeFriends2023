import { Images } from './../../models/images';
import { BreedService } from './../../services/breed-service/breed.service';
import { Component, OnInit } from '@angular/core';
import { Subscription, forkJoin, of } from 'rxjs';
import { IBreed } from 'src/app/models/breeds';
import { ImageService } from 'src/app/services/image-service/image.service';

@Component({
  selector: 'app-breeds',
  templateUrl: './breeds.component.html',
  styleUrls: ['./breeds.component.css']
})
export class BreedsComponent implements OnInit {
  breeds: IBreed[];
  breedSubscription!: Subscription;

  constructor(private breedService: BreedService, private imageService: ImageService) { }

  ngOnInit() {
    this.breedSubscription = this.breedService.getBreeds().subscribe((breeds) => {
      this.breeds = breeds;
      const imageRequests = this.breeds.map(breed => this.imageService.getMainImage(breed.externalId, 'Breed'));

      forkJoin(imageRequests).subscribe(images => {
        images.forEach((image, index) => {
          this.breeds[index].images = [image];
        });
      });
    });
  }

  getMainImage(breed: IBreed): string | undefined {
    if (breed.images) {
      const mainImage = breed.images.find(image => image.isMain);
      return mainImage ? mainImage.base64Data : undefined;
    }
    return undefined;
  }

  ngOnDestroy() {
    this.breedSubscription.unsubscribe();
  }
}

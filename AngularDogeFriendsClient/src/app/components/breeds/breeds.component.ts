import { TokenService } from './../../services/token-service/token.service';
import { BreedService } from './../../services/breed-service/breed.service';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Subscription, forkJoin } from 'rxjs';
import { IBreed } from 'src/app/models/Breeds';
import { ImageService } from 'src/app/services/image-service/image.service';
import { BreedDetailsComponent } from '../breed-details/breed-details.component';
import { DirectoryService } from 'src/app/services/directory-service/directory.service';
import { IBreedGroup, ICoat, ISize } from 'src/app/models/Directory';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-breeds',
  templateUrl: './breeds.component.html',
  styleUrls: ['./breeds.component.css']
})
export class BreedsComponent implements OnInit {
  breeds: IBreed[];
  allbreeds: IBreed[];

  breedSubscription!: Subscription;
  isUserContentManager = false;

  coats: ICoat[];
  sizes: ISize[];
  breedGroups: IBreedGroup[];

  selectedCoats: string[];
  selectedSizes: string[];
  selectedBreedGroups: string[];

  breedForm: FormGroup;

  constructor(private breedService: BreedService,
    private imageService: ImageService,
    private dialog: MatDialog,
    private tokenService: TokenService,
    private directoryService: DirectoryService) {}

  ngOnInit() {
    this.breedForm = new FormGroup({
      'breedname': new FormControl('')
    });

    this.checkForContentManagerRole();
    console.log('userIsContentManager = ' + this.isUserContentManager);
    this.fillDirectories();
  }

  getMainImage(breed: IBreed): string | undefined {
    if (breed.images) {
      const mainImage = breed.images.find(image => image.isMain);
      return mainImage ? mainImage.base64Data : undefined;
    }
    return undefined;
  }

  // openDetails(breedId: any, isUserContentManager: boolean, coats: ICoat[], sizes: ISize[], breedGroups: IBreedGroup[]): void {
  //   this.dialog.open(BreedDetailsComponent, {
  //     width: '700px',
  //     data: { breedId: breedId,
  //             isUserContentManager: this.isUserContentManager,
  //             coats: this.coats,
  //             sizes: this.sizes,
  //             breedGroups: this.breedGroups
  //           }
  //   })
  // }

  openDetails(breedId: any, isUserContentManager: boolean, coats: ICoat[], sizes: ISize[], breedGroups: IBreedGroup[]): void {
    const dialogRef = this.dialog.open(BreedDetailsComponent, {
      width: '700px',
      data: {
        breedId: breedId,
        isUserContentManager: isUserContentManager,
        coats: coats,
        sizes: sizes,
        breedGroups: breedGroups
      }
    });

    dialogRef.backdropClick().subscribe(result => {
      if (dialogRef.componentInstance.isBreedUpdated) {
        var updatedBreed = dialogRef.componentInstance.breed;
        var breedIndex = this.breeds.findIndex(breed => breed.id === updatedBreed.id);

        if (breedIndex !== -1) {
          this.breeds[breedIndex].name = updatedBreed.name;
          this.breeds[breedIndex].breedGroups = updatedBreed.breedGroups;
          console.log(this.breeds[breedIndex]);
        }
      }
    });
  }

  checkForContentManagerRole() {
    const roles = this.tokenService.getRoles();
    this.isUserContentManager = roles.includes('ContentManager');
  }

  fillDirectories() {
    const coats$ = this.directoryService.getCoats();
    const sizes$ = this.directoryService.getSizes();
    const breedGroups$ = this.directoryService.getBreedGroups();

    forkJoin({ coats: coats$, sizes: sizes$, breedGroups: breedGroups$ }).subscribe({
      next: (data: any) => {
        this.coats = data.coats;
        this.sizes = data.sizes;
        this.breedGroups = data.breedGroups;

        console.log('Справочники успешно загружены:', this.coats, this.sizes, this.breedGroups);
      },
      error: (error: any) => {
        console.error('Ошибка при загрузке справочников:', error);
      }
    });

    this.breedSubscription = this.breedService.getBreeds().subscribe((breeds) => {
      this.breeds = breeds;
      this.allbreeds = breeds;
      const imageRequests = this.breeds.map(breed => this.imageService.getMainImage(breed.externalId, 'Breed'));

      forkJoin(imageRequests).subscribe(images => {
        images.forEach((image, index) => {
          this.breeds[index].images = [image];
        });
      });
    });

    this.selectedCoats = [];
    this.selectedSizes = [];
    this.selectedBreedGroups = [];

    if (this.breedForm.get('breedname')) {
      this.breedForm.get('breedname')?.setValue('');
    }
  }

  filterBreeds() {
    console.log('Фильтрация...');

    console.log('Выбранные виды шерсти:', this.selectedCoats ? this.selectedCoats : null);
    console.log('Выбранные размеры:', this.selectedSizes ? this.selectedSizes : null);
    console.log('Выбранные группы пород:', this.selectedBreedGroups ? this.selectedBreedGroups : null);

    let filteredBreeds = this.allbreeds;

    if (this.selectedCoats && this.selectedCoats.length > 0) {
      filteredBreeds = filteredBreeds.filter(breed => this.selectedCoats.includes(breed.coat));
    }

    if (this.selectedSizes && this.selectedSizes.length > 0) {
      filteredBreeds = filteredBreeds.filter(breed => this.selectedSizes.includes(breed.size));
    }

    if (this.selectedBreedGroups && this.selectedBreedGroups.length > 0) {
      filteredBreeds = filteredBreeds.filter(breed => this.selectedBreedGroups.includes(breed.breedGroups));
    }

    if (this.breedForm.get('breedname')) {
      const searchValue = this.breedForm.get('breedname')!.value.toLowerCase();

      filteredBreeds = filteredBreeds.filter(breed => breed.name.toLowerCase().includes(searchValue)
      );
    }

    this.breeds = filteredBreeds;
  }

  areFiltersEmpty(): boolean {
    var filters = (
      (!this.selectedBreedGroups || this.selectedBreedGroups.length === 0) &&
      (!this.selectedCoats || this.selectedCoats.length === 0) &&
      (!this.selectedSizes || this.selectedSizes.length === 0)
    );

    if (this.breedForm.get('breedname')) {
      filters = (filters) && (this.breedForm.get('breedname')!.value == '')
    }

    return filters;
  }

  ngOnDestroy() {
    this.breedSubscription.unsubscribe();
  }
}

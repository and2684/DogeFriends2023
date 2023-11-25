import { BreedService } from './../../services/breed-service/breed.service';
import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { IBreed, IBreedUpdate } from 'src/app/models/Breeds';
import { IImage } from 'src/app/models/Images';
import { ImageService } from 'src/app/services/image-service/image.service';
import { ViewEncapsulation } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { firstValueFrom } from 'rxjs';
import { IBreedGroup, ICoat, ISize } from 'src/app/models/Directory';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-breed-details',
  templateUrl: './breed-details.component.html',
  styleUrls: ['./breed-details.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class BreedDetailsComponent implements OnInit {

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private breedService: BreedService,
    private imageService: ImageService,
    private dialog: MatDialogRef<BreedDetailsComponent>, // Внедряем MatDialogRef
    private router: Router
    ) {}

    breed: IBreed;
    isUserContentManager: boolean;
    updatedBreed: IBreedUpdate;
    breedChangeForm: FormGroup;
    isBreedUpdated: boolean = false;

    coats: ICoat[] = [];
    sizes: ISize[] = [];
    breedGroups: IBreedGroup[] = [];

    imageObject: Array<object> = [] ;

    async ngOnInit() {
      this.breed = await firstValueFrom(this.breedService.getBreed(this.data.breedId));

      this.breed.images = await firstValueFrom(this.imageService.getAllImages(this.breed.externalId, 'Breed'));

      this.imageObject = this.breed.images.map((image: IImage) => ({
        image: image.base64Data,
        thumbImage: image.base64Data,
        title: '',
        alt: ''
      }));

      this.isUserContentManager = this.data.isUserContentManager;
      this.updatedBreed = {
        name: this.breed.name,
        description: this.breed.description,
        coat: this.breed.coat,
        size: this.breed.size,
        breedGroups: this.breed.breedGroups
      };

      this.breedChangeForm = new FormGroup({
        'name': new FormControl(this.updatedBreed.name, [Validators.required]),
        'description': new FormControl(this.updatedBreed.description, [Validators.required]),
        'coat': new FormControl(this.updatedBreed.coat, [Validators.required]),
        'size': new FormControl(this.updatedBreed.size, [Validators.required]),
        'breedGroups': new FormControl(this.updatedBreed.breedGroups.split(',').map(group => group.trim()), [Validators.required]),
      });

      this.coats = this.data.coats;
      this.sizes = this.data.sizes;
      this.breedGroups = this.data.breedGroups;

      console.log(this.coats, this.sizes, this.breedGroups);
    }

    async onSubmit() {
      this.updatedBreed.name = this.breedChangeForm.get('name')!.value;
      this.updatedBreed.description = this.breedChangeForm.get('description')!.value;
      this.updatedBreed.coat = this.breedChangeForm.get('coat')!.value;
      this.updatedBreed.size = this.breedChangeForm.get('size')!.value;
      this.updatedBreed.breedGroups = this.breedChangeForm.get('breedGroups')!.value.join(', ');

      console.log(this.updatedBreed)

      var res = await firstValueFrom(this.breedService.updateBreed(this.breed.id, this.updatedBreed));
      console.log(res);

      if (res)
      {
        this.breed.name = this.updatedBreed.name;
        this.breed.description = this.updatedBreed.description;
        this.breed.coat = this.updatedBreed.coat;
        this.breed.size = this.updatedBreed.size;
        this.breed.breedGroups = this.updatedBreed.breedGroups;
        this.isBreedUpdated = true;
      }
    }

    closeDialog() {
      this.dialog.close({ isBreedUpdated: this.isBreedUpdated });
    }
}

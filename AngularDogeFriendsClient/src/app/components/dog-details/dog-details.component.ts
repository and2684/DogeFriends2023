import { Component, OnInit, ViewEncapsulation, Inject } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { DogDtoDetailed } from 'src/app/models/DogDto';
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
    private dialog: MatDialogRef<DogDetailsComponent>,
    private formBuilder: FormBuilder) {
  }

  dog: DogDtoDetailed = this.data.dog;

  async ngOnInit() {
    console.log(this.dog);

    //this.dog.images = await firstValueFrom(this.imageService.getAllImages(this.dog.externalId, 'Dog'));
  }

}

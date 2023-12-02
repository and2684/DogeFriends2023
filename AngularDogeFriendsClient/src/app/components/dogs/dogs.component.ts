import { firstValueFrom } from 'rxjs';
import { UserInfoDto } from 'src/app/models/UserInfoDto';
import { DogsService } from './../../services/dogs-service/dogs.service';
import { Component, OnInit, SimpleChanges } from '@angular/core';
import { DogDto } from 'src/app/models/DogDto';
import { Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TokenService } from 'src/app/services/token-service/token.service';
import { MatDialog } from '@angular/material/dialog';
import { DogDetailsComponent } from '../dog-details/dog-details.component';

@Component({
  selector: 'app-dogs',
  templateUrl: './dogs.component.html',
  styleUrls: ['./dogs.component.css']
})
export class DogsComponent implements OnInit {
  @Input() user: UserInfoDto; // получение данных от родительского компонента
  @Input() hideAddButton: boolean;
  dogs: DogDto[];
  dogsLoaded: boolean = false;

  constructor(private dogsService: DogsService,
    private tokenService: TokenService,
    private route: ActivatedRoute,
    private dialog: MatDialog) { }

  usernameFromParams: string | null;
  mypage = false;

  async ngOnInit() {
    this.usernameFromParams = this.route.snapshot.params['username'];
    this.mypage = this.tokenService.getUsername() === this.usernameFromParams;
  }

  ngOnChanges(changes: SimpleChanges) {
    if ('user' in changes) {
      const currentUser = changes['user'].currentValue;
      if (currentUser) {
        this.loadUserDogs(currentUser.username);
      }
    }
  }

  async loadUserDogs(username: string) {
    this.dogs = await firstValueFrom(this.dogsService.getUserDogs(username));
    this.dogsLoaded = true;
  }

  onDogImageClick(dog: DogDto) {
    const dialogRef = this.dialog.open(DogDetailsComponent, {
      data: { dog },
      width: '700px',
    });

    dialogRef.backdropClick().subscribe(result => {
      // if (dialogRef.componentInstance.isBreedUpdated) {
      //   var updatedBreed = dialogRef.componentInstance.breed;
      //   var breedIndex = this.breeds.findIndex(breed => breed.id === updatedBreed.id);

      //   if (breedIndex !== -1) {
      //     this.breeds[breedIndex].name = updatedBreed.name;
      //     this.breeds[breedIndex].breedGroups = updatedBreed.breedGroups;
      //     this.breeds[breedIndex].images = updatedBreed.images;
      //     console.log(this.breeds[breedIndex]);
      //   }
      // }
    });
  }

  onAddDogClick() {
    console.log('dog added');
  }
}

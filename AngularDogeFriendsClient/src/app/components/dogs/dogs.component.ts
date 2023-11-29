import { UserInfoDto } from 'src/app/models/UserInfoDto';
import { DogsService } from './../../services/dogs-service/dogs.service';
import { Component, OnInit } from '@angular/core';
import { DogDto } from 'src/app/models/DogDto';
import { UserService } from 'src/app/services/user-service/user.service';
import { Input } from '@angular/core';

@Component({
  selector: 'app-dogs',
  templateUrl: './dogs.component.html',
  styleUrls: ['./dogs.component.css']
})
export class DogsComponent implements OnInit {
  @Input() user: UserInfoDto; // получение данных от родительского компонента
  dogs: DogDto[];

  constructor(private dogsService: DogsService,
              private userService: UserService) { }


  async ngOnInit() {



  }

}

import { Component, Input, NgModule, OnInit } from '@angular/core';
import { UserInfoDto } from 'src/app/models/UserInfoDto';

@Component({
  selector: 'app-friends',
  templateUrl: './friends.component.html',
  styleUrls: ['./friends.component.css']
})
export class FriendsComponent implements OnInit {
  @Input() user: UserInfoDto; // получение данных от родительского компонента
  constructor() { }

  username: string = 'and2684';

  ngOnInit() {
  }

}

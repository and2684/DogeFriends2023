import { Component, EventEmitter, Input, NgModule, OnInit, Output } from '@angular/core';
import { UserInfoDto } from 'src/app/models/UserInfoDto';

@Component({
  selector: 'app-friends',
  templateUrl: './friends.component.html',
  styleUrls: ['./friends.component.css']
})
export class FriendsComponent implements OnInit {
  @Input() user: UserInfoDto; // получение данных от родительского компонента
  @Output() redirectToFriendEvent = new EventEmitter<string>();
  constructor() { }

  username: string = 'and2684';

  ngOnInit() {
  }

  handleRedirectToFriend(username: string) {
    this.redirectToFriendEvent.emit(this.username);
  }

}

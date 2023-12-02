import { Component, Output } from '@angular/core';
import { UserInfoDto } from 'src/app/models/UserInfoDto';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent {
  @Output () user: UserInfoDto;

  constructor() {}

  ngOnInit() {
    console.log('user.component ngOnInit fired');
  }

  handleUserEvent(user: UserInfoDto) {
    this.user = user;
  }
}

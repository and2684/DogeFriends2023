import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { UserInfoDto } from 'src/app/models/UserInfoDto';
import { TokenService } from 'src/app/services/token-service/token.service';
import { UserService } from 'src/app/services/user-service/user.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent {
  user: UserInfoDto;
  username: string;
  usersubscription: Subscription;

  constructor(private userService: UserService, private tokenService: TokenService) {}

  ngOnInit() {
      this.username = this.tokenService.getUsername()!;
      console.log('Имя пользователя в user.component: ' + this.username);
      this.usersubscription = this.userService.getUserByUsername(this.username).subscribe((user) => {this.user = user;});
  }

  ngOnDestroy() {
    this.usersubscription.unsubscribe();
  }
}

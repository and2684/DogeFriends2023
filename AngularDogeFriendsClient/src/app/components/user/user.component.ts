import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
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
  usernameFromParams: string | null;
  usersubscription: Subscription;
  mypage = false;

  constructor(private userService: UserService,
              private tokenService: TokenService,
              private route: ActivatedRoute) {}

  ngOnInit() {
    this.route.params.subscribe(params => {this.usernameFromParams = params['username']});

    this.mypage = this.tokenService.getUsername() === this.usernameFromParams;
    if (this.usernameFromParams)
      this.usersubscription = this.userService.getUserByUsername(this.usernameFromParams).subscribe((user) => {this.user = user;});
  }

  ngOnDestroy() {
    this.usersubscription.unsubscribe();
  }
}

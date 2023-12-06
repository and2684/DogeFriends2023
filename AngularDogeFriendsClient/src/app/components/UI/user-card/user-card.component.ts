import { Component, Input, OnInit } from '@angular/core';
import { UserInfoDtoWithMainImage } from 'src/app/models/UserInfoDto';
import { TokenService } from 'src/app/services/token-service/token.service';


@Component({
  selector: 'app-user-card',
  templateUrl: './user-card.component.html',
  styleUrls: ['./user-card.component.css']
})
export class UserCardComponent implements OnInit {
  @Input() user: UserInfoDtoWithMainImage; // получение данных от родительского компонента
  tokenUsername: string | null = null;

  constructor(private tokenService: TokenService) { }

  async ngOnInit() {
    this.tokenUsername = this.tokenService.getUsername();
  }
}

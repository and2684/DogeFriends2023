import { ImageService } from 'src/app/services/image-service/image.service';
import { Component, Input, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { UserInfoDto } from 'src/app/models/UserInfoDto';
import { TokenService } from 'src/app/services/token-service/token.service';
import { UserService } from 'src/app/services/user-service/user.service';
import { Output, EventEmitter } from '@angular/core';
import { IImage } from 'src/app/models/Images';


@Component({
  selector: 'app-user-card',
  templateUrl: './user-card.component.html',
  styleUrls: ['./user-card.component.css']
})
export class UserCardComponent implements OnInit {
  @Output() userEvent = new EventEmitter<UserInfoDto>();
  user: UserInfoDto | null = null;
  mainImage: IImage;

  @Input() username: string; // Получение имени пользователя для редиректа


  usernameFromParams: string | null;
  mypage = false;

  constructor(private userService: UserService,
    private tokenService: TokenService,
    private route: ActivatedRoute,
    private imageService: ImageService) { }

  async ngOnInit() {
    this.usernameFromParams = this.route.snapshot.params['username'];
    this.mypage = this.tokenService.getUsername() === this.usernameFromParams;

    this.user = await firstValueFrom(this.userService.getUserByUsername(this.usernameFromParams!));
    this.mainImage = await firstValueFrom(this.imageService.getMainImage(this.user.externalId, 'User'));

    this.userEvent.emit(this.user!);
  }
}

import { Component, EventEmitter, Output } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { IImage } from 'src/app/models/Images';
import { UserInfoDto, UserInfoDtoWithMainImage } from 'src/app/models/UserInfoDto';
import { ImageService } from 'src/app/services/image-service/image.service';
import { UserService } from 'src/app/services/user-service/user.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent {
  @Output () user: UserInfoDtoWithMainImage;
  @Output() userEvent = new EventEmitter<UserInfoDtoWithMainImage>();

  usernameFromParams: string | null;
  mainImage: IImage;

  constructor(private userService: UserService,
    private route: ActivatedRoute,
    private imageService: ImageService) {}

  async ngOnInit() {
    this.usernameFromParams = this.route.snapshot.params['username'];
    this.user = await firstValueFrom(this.userService.getUserByUsername(this.usernameFromParams!));
    this.mainImage = await firstValueFrom(this.imageService.getMainImage(this.user.externalId, 'User'));
    this.user.mainImage = this.mainImage.base64Data;

    this.userEvent.emit(this.user!);
  }

  handleUserEvent(user: UserInfoDto) {
    this.user = user;
  }
}

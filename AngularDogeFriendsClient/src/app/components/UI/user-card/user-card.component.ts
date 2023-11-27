import { ImageService } from 'src/app/services/image-service/image.service';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable, Subscription, firstValueFrom } from 'rxjs';
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
export class UserCardComponent implements OnInit, OnDestroy {
  @Output() userEvent = new EventEmitter<UserInfoDto>();
  user: UserInfoDto;
  mainImage: IImage | null = null;

  routeSubscription: Subscription;

  usernameFromParams: string | null;
  mypage = false;
  dataLoaded: boolean = false;

  constructor(private userService: UserService,
    private tokenService: TokenService,
    private route: ActivatedRoute,
    private imageService: ImageService) { }

  async ngOnInit() {
    console.log('ngOnInit fired');

    this.routeSubscription = this.route.params.subscribe(params => {
      this.usernameFromParams = params['username'];
    });

    this.mypage = this.tokenService.getUsername() === this.usernameFromParams;
    if (this.usernameFromParams)
      this.user = await firstValueFrom(this.userService.getUserByUsername(this.usernameFromParams));
    this.userEvent.emit(this.user!);
    console.log('user:' + this.user!.showname);

    this.loadMainImage();
    this.dataLoaded = true;
  }

  async loadMainImage() {
    try {
      this.mainImage = await firstValueFrom(this.imageService.getMainImage(this.user!.externalId, 'User'));
    } catch (error) {
      console.error('Error loading main image:', error);
    }
  }

  ngOnDestroy() {
    this.routeSubscription.unsubscribe();
  }

}

import { firstValueFrom } from 'rxjs';
import { Component, EventEmitter, Input, OnInit, Output, SimpleChanges, ViewChild } from '@angular/core';
import { UserInfoDto, UserInfoDtoWithMainImage } from 'src/app/models/UserInfoDto';
import { FriendsService } from 'src/app/services/friends-service/friends.service';
import { ImageService } from 'src/app/services/image-service/image.service';
import { DogsPopupComponent } from '../UI/dogs-popup/dogs-popup.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-friends',
  templateUrl: './friends.component.html',
  styleUrls: ['./friends.component.css']
})
export class FriendsComponent implements OnInit {
  @Input() user: UserInfoDto; // получение данных от родительского компонента
  @Output() userEvent = new EventEmitter<UserInfoDto>();
  @ViewChild(DogsPopupComponent) dogsPopup: DogsPopupComponent;

  constructor(private friendsService: FriendsService,
    private imageService: ImageService,
    private router: Router) {}

  friends: UserInfoDtoWithMainImage[] = [];
  subs: UserInfoDtoWithMainImage[] = [];
  subscriptions: UserInfoDtoWithMainImage[] = [];

  showDogsPopup = false;
  popupUser: UserInfoDto;

  async ngOnChanges(changes: SimpleChanges) {
    if (changes['user'] && changes['user'].currentValue) {
      await this.loadUserDetails();
    }
  }

  async loadUserDetails() {
    // Друзья
    try {
      this.friends = await firstValueFrom(this.friendsService.getUserFriends(this.user.id));
    } catch (error) {
      this.friends = [];
    }

    if (this.friends) {
      this.friends.forEach(async friend => {
        var image = await firstValueFrom(this.imageService.getMainImage(friend.externalId, 'User'));
        if (image)
          friend.mainImage = image.base64Data;
      });
    }

    // Подписчики
    try {
      this.subs = await firstValueFrom(this.friendsService.getUserSubs(this.user.id));
    } catch (error) {
      this.subs = [];
    }

    if (this.subs) {
      this.subs.forEach(async sub => {
        var image = await firstValueFrom(this.imageService.getMainImage(sub.externalId, 'User'));
        if (image)
          sub.mainImage = image.base64Data;
      });
    }

    // Подписки
    try {
      this.subscriptions = await firstValueFrom(this.friendsService.getUserSubscriptions(this.user.id));
    } catch (error) {
      this.subs = [];
    }

    if (this.subscriptions) {
      this.subscriptions.forEach(async subscription => {
        var image = await firstValueFrom(this.imageService.getMainImage(subscription.externalId, 'User'));
        if (image)
          subscription.mainImage = image.base64Data;
      });
    }
  }

  ngOnInit() {

  }

  showDogs(user: UserInfoDto, event: MouseEvent) {
    this.popupUser = user;
    this.showDogsPopup = true;

    setTimeout(() => {
      var dogsPopupElement = document.querySelector('.dogs-popup') as HTMLElement;
      if (dogsPopupElement) {
        dogsPopupElement.style.position = 'absolute';
        dogsPopupElement.style.left = event.clientX + 10 + 'px';
        dogsPopupElement.style.top = event.clientY + 30 + 'px';
        dogsPopupElement.classList.add('show');
      }
    }, 10);
  }

  hideDogs() {
    var dogsPopupElement = document.querySelector('.dogs-popup') as HTMLElement;
    if (dogsPopupElement) {
      dogsPopupElement.classList.remove('show');
    }
    //this.showDogsPopup = false;
  }

  sendUser(user: UserInfoDto) {
    this.router.navigate(['user', user.username])
    this.userEvent.emit(user);
  }

  // Не используется, для тестов!
  routeToFriend(routeUsername: string) {
    this.router.navigate(['user', routeUsername]).then(() => {
      window.location.reload();
    });
  }
}


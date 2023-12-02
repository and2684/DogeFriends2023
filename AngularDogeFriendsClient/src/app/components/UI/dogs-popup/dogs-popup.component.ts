import { Component, Input, OnInit } from '@angular/core';
import { UserInfoDto } from 'src/app/models/UserInfoDto';

@Component({
  selector: 'app-dogs-popup',
  template: `
    <div class="dogs-popup">
    <app-dogs [user]="user" [hideAddButton]="true"></app-dogs>
    </div>
  `,
  styleUrls: ['./dogs-popup.component.css']
})
export class DogsPopupComponent implements OnInit {
  @Input() user: UserInfoDto;
  @Input() hideAddButton: boolean;

  ngOnInit() {

  }
}

import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { TokenService } from 'src/app/services/token-service/token.service';

@Component({
  selector: 'app-left-panel',
  templateUrl: './left-panel.component.html',
  styleUrls: ['./left-panel.component.css']
})
export class LeftPanelComponent implements OnInit {
  username: string | null;
  private usernameSubscription: Subscription;

  constructor(private tokenService: TokenService) { }

  ngOnInit() {
    //this.username = this.tokenService.getUsername();
    this.usernameSubscription = this.tokenService.username$.subscribe(username => {
      this.username = username;
    });
  }

  ngOnDestroy() {
    this.usernameSubscription.unsubscribe();
  }
}

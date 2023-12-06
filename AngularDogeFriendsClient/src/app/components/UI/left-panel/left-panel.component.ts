import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
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

  constructor(private tokenService: TokenService, private router: Router) { }

  ngOnInit() {
    //this.username = this.tokenService.getUsername();
    this.usernameSubscription = this.tokenService.username$.subscribe(username => {
      this.username = username;
    });
  }

  ngOnDestroy() {
    this.usernameSubscription.unsubscribe();
  }

  routeToUser(routeUsername: string) {
    this.router.navigate(['user', routeUsername]).then(() => {
      window.location.reload();
    });
  }
}

import { TokenService } from './../../services/token-service/token.service';
import { Component, OnInit } from '@angular/core';
import { LoginService } from 'src/app/services/login-service/login.service';
import { LoginDto } from '../../models/LoginDto'
import { catchError, throwError } from 'rxjs';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  hide = true;

  loginData: LoginDto = {
    username: '',
    password: ''
  };

  constructor(private loginService: LoginService, private tokenService: TokenService) { }

  ngOnInit() {
  }

  onSubmit(): void {
    this.loginService.login(this.loginData)
      .pipe(
        catchError(error => {
          console.error('Ошибка входа:', error);

          // Вывод сообщения об ошибке или другая логика обработки ошибки
          return throwError(() => new Error(error));
        })
      )
      .subscribe(response => {
        // Обработка успешного входа
        console.log('Успешный вход:', response);
        this.tokenService.saveTokens(response.accessToken, response.refreshToken);
        this.tokenService.saveUserInfoFromAccessToken();
      });
  }
}

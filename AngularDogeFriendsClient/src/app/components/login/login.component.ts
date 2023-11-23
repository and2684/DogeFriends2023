import { TokenService } from './../../services/token-service/token.service';
import { Component, OnInit } from '@angular/core';
import { LoginService } from 'src/app/services/login-service/login.service';
import { LoginDto } from '../../models/LoginDto'
import { catchError, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;

  hide = true;

  loginData: LoginDto = {
    username: '',
    password: ''
  };

  constructor(private loginService: LoginService, private tokenService: TokenService, private router: Router) { }

  ngOnInit() {
    this.loginForm = new FormGroup({
      'username': new FormControl('', [Validators.required, Validators.pattern(/^.{4,}$/)]),
      'password': new FormControl('', [Validators.required, Validators.pattern(/^.{4,}$/)])
    });
  }

  onSubmit(): void {
    this.loginData.username = this.loginForm.get('username')!.value;
    this.loginData.password = this.loginForm.get('password')!.value;

    this.loginService.login(this.loginData)
      .pipe(
        catchError(error => {
          console.error('Ошибка входа:', error);
          return throwError(() => new Error(error));
        })
      )
      .subscribe(response => {
        console.log('Успешный вход:', response);
        this.tokenService.saveTokens(response.accessToken, response.refreshToken);
        this.tokenService.saveUserInfoFromAccessToken();
      });
  }
}

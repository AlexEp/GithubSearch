import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';

import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { Login } from '../models/login.model';
import { UserRegister } from '../models/userRegister.model';


@Injectable()
export class AuthService {
  baseUrl = environment.apiUrl + 'auth/';
  jwtHelper = new JwtHelperService();
  private decodedToken: any;

  constructor(private http: HttpClient,   private router: Router,) { }

  login(model: Login) {
    return this.http.post(this.baseUrl + 'login', model).pipe(
      map((response: any) => {
        const user = response;
        if (user) {
          localStorage.setItem('token', user.token);
          this.decodedToken = this.jwtHelper.decodeToken(user.token);
          localStorage.setItem('decodedToken', JSON.stringify(this.decodedToken));
        }
      })
    );
  }

  register(user: UserRegister) {
    return this.http.post(this.baseUrl + 'register', user);
  }

  getClames(){
    if(!this.decodedToken){
      var decodedToken = localStorage.getItem('decodedToken');

      if(!decodedToken)  {
        this. logout();
      }
      this.decodedToken = JSON.parse(decodedToken);
    }

    return this.decodedToken;

  }

  isLoggedIn() : boolean {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }

  logout(){
    localStorage.removeItem('token');
    localStorage.removeItem('decodedToken');
    this.decodedToken = null;
    this.router.navigate(['/login']);
  }
}

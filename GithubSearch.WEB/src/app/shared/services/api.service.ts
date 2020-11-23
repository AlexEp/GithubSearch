import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';

import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { Login } from '../models/login.model';
import { UserRegister } from '../models/userRegister.model';
import { Repository } from '../models/repository.model';


@Injectable()
export class APIService {
  baseUrl = environment.apiUrl;
  jwtHelper = new JwtHelperService();


  constructor(private http: HttpClient) {}

  getRepositories(query : string): Observable<Repository[]> {

      return this.http.get<Repository[]>(this.baseUrl + `repositories/search/${query}`);
  }

}

import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';


import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { HomeComponent } from './components/home/home.component';
import { RepositoriesComponent } from './components/repositories/repositories.component';

import { AppRoutingModule } from './app-routing.module';
import { AuthGuard } from './shared/guards/auth.guard';
import { AuthService } from './shared/services/auth.service';
import { AlertService } from './shared/services/alert.service';
import { AuthInterceptor } from './shared/services/auth.interceptor';
import { NavBarComponent } from './components/nav-bar/nav-bar.component';
import { APIService } from './shared/services/api.service';

export const appRoutes: Routes = [
  {path: '', component: HomeComponent,  canActivate: [AuthGuard],
    children: [    
      { path: '', pathMatch: 'full', redirectTo: 'repositories'},
      { path: 'repositories', component: RepositoriesComponent },
        ]},
  { path: 'login', component: LoginComponent },
  {path: '**', redirectTo: '', pathMatch: 'full'},
];

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent,
    RepositoriesComponent,
    NavBarComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    AppRoutingModule,
    RouterModule.forRoot(appRoutes),
    NgbModule,
  ],
  providers: [ 
    AuthService,
    AlertService,
    APIService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }],
  bootstrap: [AppComponent]
})
export class AppModule { }

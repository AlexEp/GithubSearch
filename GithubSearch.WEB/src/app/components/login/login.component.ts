import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AlertService } from 'src/app/shared/services/alert.service';

import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { AuthService } from 'src/app/shared/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  loginForm : FormGroup



  constructor(private authService: AuthService, 
    private alert: AlertService,
    private router: Router,
    private fb: FormBuilder) { }
    model: any = {};
    
  ngOnInit(): void {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]});
  }

  loginSubmit() {

    this.authService.login(this.loginForm.value).subscribe(next => {
      this.alert.success('Login succeeded');
    }, error => {
      if(error?.status == 400 && error?.error?.message) {
        this.alert.error(error.error.message);
      }
      else{
        this.alert.error("Login failed");
      }

    }, () => {
      this.router.navigate(['/home']);
    });
  }

}

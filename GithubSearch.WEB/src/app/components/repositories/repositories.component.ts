import { Component, OnInit } from '@angular/core';
import { Repository } from 'src/app/shared/models/repository.model';
import { AlertService } from 'src/app/shared/services/alert.service';
import { APIService } from 'src/app/shared/services/api.service';
import { AuthService } from 'src/app/shared/services/auth.service';

@Component({
  selector: 'app-repositories',
  templateUrl: './repositories.component.html',
  styleUrls: ['./repositories.component.scss']
})
export class RepositoriesComponent implements OnInit {
  repositories: Repository[];

  constructor(
    private authService: AuthService, 
    private apiService: APIService,
    private alert: AlertService) { }

  ngOnInit(): void {
    this.apiService.getRepositories("linux").subscribe(replay => {
        this.repositories = replay;
    })
  }

  getUserName() {
    return this.authService.getClames()?.UserName;
  }

}

import { Component, OnInit } from '@angular/core';
import { StorageService } from '../../shared/services/storage.service';
import { interval } from 'rxjs';
import { Router } from '@angular/router';
import { HttpService } from '../../shared/services/http.service';

@Component({
  selector: 'app-menubar-client',
  templateUrl: './menubar-client.component.html',
  styleUrl: './menubar-client.component.css'
})
export class MenubarClientComponent implements OnInit {

  difference = "Pozostało minut: 5";

  constructor(private storage: StorageService, private router: Router, private http: HttpService) { }

  ngOnInit() {
    interval(1000).subscribe(() => {
      var token = localStorage.getItem('token');
      if (token) {
        const decoded = JSON.parse(atob(token.split('.')[1]));
        const date = new Date(0);
        date.setUTCSeconds(decoded.exp);
        const time = date.getTime() - Date.now();
        if (time >= 60000)
          this.difference = "Pozostało minut: " + Math.round(time / 60000);
        else
          this.difference = "Pozostało sekund: " + Math.round(time / 1000);
        if (time <= 0) {
          this.logout(); 
        }
      }
    });
  }

  logout() {
    this.storage.removeItem('token');
    this.router.navigate(['/']);
  }

  getNewToken() {
    this.http.getNewToken().subscribe({
      next: (data: any) => {
        var result = data.token;
        console.log("data: " + result);
        this.storage.setItem('token', result);
      },
      error: (error: any) => {
        console.log(error.message);
      }
    });
    
  }

}

import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Observable, fromEvent } from 'rxjs';
import { StorageService } from '../shared/services/storage.service';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {

  constructor(private http: HttpClient, private storage: StorageService) { }

  ngOnInit() {
    this.storage.watchStorage().subscribe((data) => {
      if (data) {
        console.log(data);
        this.token = localStorage.getItem('token');
      }
    })
  }

  token = localStorage.getItem('token');

  updateToken() {
    console.log(this.token);
    this.token = localStorage.getItem('token');
    console.log(this.token);
  }

  title = 'bankapp.client';
}

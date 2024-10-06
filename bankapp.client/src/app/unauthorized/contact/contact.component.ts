import { Component, OnInit } from '@angular/core';
import { StorageService } from '../../shared/services/storage.service';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrl: './contact.component.css'
})
export class ContactComponent implements OnInit {
  constructor(private storage: StorageService) { }

  ngOnInit() {
    this.storage.removeItem('token');
  }
}

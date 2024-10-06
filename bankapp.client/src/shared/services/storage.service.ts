import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';



interface StorageEvent {
  key: string;
  value?: string;
  action: 'set' | 'remove';
}

@Injectable({
  providedIn: 'root'
})
export class StorageService {

  constructor() { }

  private storageSubject = new BehaviorSubject<StorageEvent | null>(null);

  watchStorage() {
    return this.storageSubject.asObservable();
  }

  setItem(key: string, value: string) {
    localStorage.setItem(key, value);

    this.storageSubject.next({ key, value, action: 'set' });
  }

  removeItem(key: string) {
    localStorage.removeItem(key);
    this.storageSubject.next({ key, action: 'remove' });
  }

}

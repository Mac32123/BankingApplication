import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { HttpService } from '../../../shared/services/http.service';
import { LoginData } from '../../../shared/models/loginData';
import { Router } from '@angular/router';
import { StorageService } from '../../../shared/services/storage.service';

@Component({
  selector: 'app-liign',
  templateUrl: './liign.component.html',
  styleUrl: './liign.component.css'
})

export class LiignComponent {
  loginForm = new FormGroup({
    accountNumber: new FormControl('', [Validators.required, Validators.pattern("^[0][1-9]\\d{9}$|^[1-9]\\d{9}$")]),
    pin: new FormControl('', [Validators.required])
  });

  errorMessage = "";
  loading = false;

  constructor(private http: HttpService, private router: Router, private storage: StorageService) { }


  login() {
    this.loading = true;
    var accNum = this.loginForm.get("accountNumber");
    var accNumVal = accNum ? accNum.value : "";
    var pin = this.loginForm.get("pin");
    var pinVal = pin ? pin.value : "";

    var loginData = new LoginData(accNumVal, pinVal);
    var result: any;
    this.http.login(loginData).subscribe({
      next: (data: any) => {
        result = data.token;
        console.log("data: " + result);
        this.loading = false;
        this.loginForm.reset();
        this.storage.setItem('token', result);
        this.router.navigate(["finanse"]);
      },
      error: (error: any) => {
        if (error.status == 401) this.errorMessage = "Niepoprawne dane logowania!";
        else if (Math.floor(error.status / 100) == 5) this.errorMessage = "Błąd serwera!";
        else this.errorMessage = "Niezidentyfikowany błąd!";
        console.log(error.message);
        this.loading = false;
        this.loginForm.reset();
      }
    });
    
  }


}

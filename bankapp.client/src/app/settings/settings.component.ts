import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { checkPasswords } from './checkPassword';
import { HttpService } from '../../shared/services/http.service';
import { updateData } from '../../shared/models/updateData';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrl: './settings.component.css'
})
export class SettingsComponent implements OnInit {

  constructor(private router: Router, private http: HttpService) { }

  ngOnInit() {
    var token = localStorage.getItem('token');
    if (!token) {
      this.router.navigate(['']);
    }
  }

  settingsForm = new FormGroup({
    email: new FormControl('', [Validators.email]),
    phoneNumber: new FormControl('', [Validators.pattern("^(\\+(\\d){2})?(\\d{9})$")]),
    newPassword: new FormControl('', [Validators.minLength(8), Validators.maxLength(20), Validators.pattern("(^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$)|(^.{0,7}$)|(^.{21,}$)")]),
    confirmPassword: new FormControl('', [checkPasswords])
  });

  errorMessage = "";
  successMessage = "";
  loading = false;

  check() {
    this.settingsForm.controls.confirmPassword.setValidators(this.settingsForm.get('newPassword')?.value ? [Validators.required, checkPasswords] : [checkPasswords]);
    this.settingsForm.controls.confirmPassword.updateValueAndValidity();
  }

  send() {
    this.loading = true;
    let update: updateData;
    let email = this.settingsForm.get("email");
    let emailVal = email ? email.value : "";
    let phoneNumber = this.settingsForm.get("phoneNumber");
    let phoneNumberVal = phoneNumber ? phoneNumber.value : "";
    let newPassword = this.settingsForm.get("newPassword");
    let newPasswordVal = newPassword ? newPassword.value : "";
    var confirmPassword = this.settingsForm.get("confirmPassword");
    var confirmPasswordVal = confirmPassword ? confirmPassword.value : "";
    update = new updateData(phoneNumberVal, emailVal, newPasswordVal, confirmPasswordVal)
    this.http.updateAccount(update).subscribe({
      next: (data: any) => {
        this.loading = false;
        this.errorMessage = "";
        this.successMessage = "Pomyślnie zaktualizowano dane użytkownika!";
        this.settingsForm.reset();
      },
      error: (error: any) => {
        this.successMessage = "";
          if (error.status = 409) this.errorMessage = error.error.message;
          else if (Math.floor(error.status / 100) == 5) this.errorMessage = "Błąd serwera!";
          else this.errorMessage = "Niezidentyfikowany błąd!";
          console.log(error.message);
          this.loading = false;
        }
    })
  }

}

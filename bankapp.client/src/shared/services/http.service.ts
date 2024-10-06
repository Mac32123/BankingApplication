import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs';
import { LoginData } from '../models/loginData';
import { updateData } from '../models/updateData';

@Injectable({
  providedIn: 'root'
})
export class HttpService {

  constructor(private http: HttpClient) { }

  private getStandardOptions(): any {
    return {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };
  }


  login(loginData: LoginData) {
    let options = this.getStandardOptions();
    let body = JSON.stringify(loginData);
    return this.http.post("https://localhost:7277/api/Accounts/authenticate", body, options);
  }

  getAccountData() {
    let options = this.getStandardOptions();
    return this.http.get("https://localhost:7277/api/Accounts/get_current", options);
  }


  getNewToken() {
    let options = this.getStandardOptions();
    return this.http.get("https://localhost:7277/api/Accounts/renewToken", options);
  }

  checkAccountNumber(actualAccountNumber: string) {
    let options = this.getStandardOptions();
    options.params = new HttpParams();

    //options.params.append('actualAccountNumber', actualAccountNumber);    //naprawić jutro
    //https:/localhost:7277/api/Accounts/check_account_number?actualAccountNumber=" + actualAccountNumber
    return this.http.get("https://localhost:7277/api/Accounts/check_account_number?actualAccountNumber=" + actualAccountNumber, options)
  }

  makeTransfer(actualAccountNumber: string, amount: number, title: string) {
    let options = this.getStandardOptions();
    options.params = new HttpParams();

    options.params.append('ToAcount', actualAccountNumber, 'Amount', amount);    //naprawić jutro
    return this.http.post("https://localhost:7277/api/Transactions/make_transfer?ToAccount=" + actualAccountNumber + "&Amount=" + amount + "&title=" + title, {}, options);
  }

  getTransactions(toFrom: string, startingDate?: Date, endingDate?: Date)
  {
    let options = this.getStandardOptions();

    let startingString = startingDate ? "&startingDate=" : "";
    let endingString = endingDate ? "&endingDate=" : "";

    let sDateString = startingDate ? startingDate.toISOString() : "";
    let eDateString = endingDate ? endingDate.toISOString() : "";

    return this.http.get("https://localhost:7277/api/Transactions/get_transaction?toFrom=" + toFrom + startingString + sDateString + endingString + eDateString, options)
  }

  updateAccount(data: updateData) {

    let options = this.getStandardOptions();
    let body = JSON.stringify(data);

    return this.http.post("https://localhost:7277/api/Accounts/update", body, options );

  }


}

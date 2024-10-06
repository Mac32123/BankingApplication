import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HttpService } from '../../shared/services/http.service';

@Component({
  selector: 'app-finance',
  templateUrl: './finance.component.html',
  styleUrl: './finance.component.css'
})
export class FinanceComponent implements OnInit {

  constructor(private router: Router, private http: HttpService) { }

  ngOnInit() {
    var token = localStorage.getItem('token');
    if (!token) {
      this.router.navigate(['']);
    }

    this.http.getAccountData().subscribe((data: any) => {
      this.accountNumber = this.processAccountNumber(data.actualAccountNumber)
      this.amount = data.currentAccountBalance
        this.name = data.accountName
        }
      )
  }

  accountNumber: string = "";
  amount = 0;
  name = ""

  processAccountNumber(accountNumber: string) {
    var newString = accountNumber.substring(0, 2);
    newString += " ";
    for (let i = 0; i < 6; i++) {
      newString += accountNumber.substring(i * 4 + 2, i * 4 + 6);
      newString += " ";
    }
    return newString;
  }


}

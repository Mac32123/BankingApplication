import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { HttpService } from '../../shared/services/http.service';
import { LoginData } from '../../shared/models/loginData';
import { Router } from '@angular/router';
import { StorageService } from '../../shared/services/storage.service';
import { MatDialog } from '@angular/material/dialog';
import { TransactionDialogComponent } from './transaction-dialog/transaction-dialog.component';

@Component({
  selector: 'app-transaction',
  templateUrl: './transaction.component.html',
  styleUrl: './transaction.component.css'
})
export class TransactionComponent implements OnInit {

  constructor(private http: HttpService, private router: Router, private storage: StorageService, private dialog: MatDialog) { }

  ngOnInit() {
    var token = localStorage.getItem('token');
    if (!token) {
      this.router.navigate(['']);
    }

    this.http.getAccountData().subscribe((data: any) => {
      this.accountNumber = this.processAccountNumber(data.actualAccountNumber)
      this.amount = data.currentAccountBalance
    }
    )
  }

  accountNumber = ""
  amount = 0;
  transactionForm = new FormGroup({
    Receiver: new FormControl('', [Validators.required, Validators.maxLength(80)]),
    toAccount: new FormControl('', [Validators.required, Validators.maxLength(26), Validators.minLength(26)]),
    amount: new FormControl('', [Validators.required, Validators.min(0.01), Validators.max(125000)]),
    title: new FormControl('Przelew', [Validators.required, Validators.maxLength(140)])
  });

  errorMessage = "";
  loading = false;

  

  proceedeAccountNumber() {
    var toAccount = this.transactionForm.get("toAccount");
    var toAccountVal = toAccount ? toAccount.value! : "";
    toAccountVal = toAccountVal.replaceAll(" ", "");
    if (toAccount)
      toAccount.setValue(toAccountVal);

    //var toAccountForm = new FormControl(toAccount);
    //console.log(toAccountForm);

    //this.transactionForm.setControl("toAccount", toAccount);
  }

  reset(counter: number, toAccountVal : string, receiverVal : string, amountVal : string, titleVal: string) {
    if (counter == 2) {
      this.dialog.open(TransactionDialogComponent, {
        data: {
          bankAccount: this.accountNumber,
          toAccount: this.processAccountNumber(toAccountVal),
          name: receiverVal,
          amount: amountVal,
          title: titleVal
        }
      });
    }
  }


  next() {
    var counter = 0;
    var allCounter = 0;
    this.errorMessage = "";
    this.loading = true;
    var toAccount = this.transactionForm.get("toAccount");
    var toAccountVal = toAccount ? toAccount.value : "";
    var receiver = this.transactionForm.get("Receiver");
    var receiverVal = receiver ? receiver.value : "";
    var amount = this.transactionForm.get("amount");
    var amountVal = amount ? amount.value : "";
    var title = this.transactionForm.get("title");
    var titleVal = title ? title.value : "";
    if (this.processAccountNumber(toAccountVal!) == this.accountNumber) {
      this.errorMessage = "Odbiorca i nadawca nie mogą być tacy sami!"
      this.loading = false;
      return
    }
    this.http.checkAccountNumber(toAccountVal!).subscribe({
      next: (data: any) => {
        counter++;
        allCounter++;
        if (allCounter == 2) this.loading = false;
        this.reset(counter, toAccountVal!, receiverVal!, amountVal!, titleVal!);
      },
      error: (error: any) => {
        allCounter++;
        if (error.status == 404) this.errorMessage += "Niepoprawny numer rachunku\n";
        else if (Math.floor(error.status / 100) == 5) this.errorMessage = "Błąd serwera!";
        else this.errorMessage = "Niezidentyfikowany błąd!";
        console.log(error.message);
        if (allCounter == 2)
        this.loading = false;
      }
    });

    this.http.getAccountData().subscribe({
      next: (data: any) => {
        allCounter++;
        if (Number(data.currentAccountBalance) < Number(amountVal!))
          this.errorMessage = "Niewystarczająca ilość środków na koncie: " + data.currentAccountBalance + " PLN";
        else counter++;
        if (allCounter == 2) this.loading = false;
        this.reset(counter, toAccountVal!, receiverVal!, amountVal!, titleVal!);
      },
      error: (error: any) => {
        allCounter++
        if (Math.floor(error.status / 100) == 5) this.errorMessage = "Błąd serwera!";
        else this.errorMessage = "Niezidentyfikowany błąd!";
        console.log(error.message);
        if (allCounter >= 2)
        this.loading = false;
      }
    });



  }

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

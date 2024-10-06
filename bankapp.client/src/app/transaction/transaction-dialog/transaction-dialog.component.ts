import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { HttpService } from '../../../shared/services/http.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-transaction-dialog',
  templateUrl: './transaction-dialog.component.html',
  styleUrl: './transaction-dialog.component.css'
})
export class TransactionDialogComponent {

  constructor(@Inject(MAT_DIALOG_DATA) public data: any, private http: HttpService, private router: Router, private dialogRef: MatDialogRef<TransactionDialogComponent>) { }

  errorMessage = "";

  send() {
    this.http.makeTransfer(this.data.toAccount.replaceAll(" ", "") , this.data.amount, this.data.title).subscribe({
      next: (data: any) => {
        this.router.navigate(['/success']);
        this.dialogRef.close();
      },
      error: (error: any) => {
        if (Math.floor(error.status / 100) == 4) {
          this.errorMessage = error.error.errors.title[0]
          console.log(error)
        }
        else if (Math.floor(error.status / 100) == 5) this.errorMessage = "Błąd serwera!";
        else this.errorMessage = "Niezidentyfikowany błąd!";
      }
    });
  }

}

import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { HttpService } from '../../shared/services/http.service';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { formatDate } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { HistoryDialogComponent } from './history-dialog/history-dialog.component';
import { Router } from '@angular/router';
import { MatSort, Sort, MatSortHeader } from '@angular/material/sort'

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrl: './history.component.css'
})
export class HistoryComponent implements OnInit {

  constructor(private http: HttpService, private dialog: MatDialog, private router: Router) { }

  ngOnInit() {
    var token = localStorage.getItem('token');
    if (!token) {
      this.router.navigate(['']);
    }
    this.http.getAccountData().subscribe((data : any) => {
      this.myName = data.accountName
      console.log(this.myName);
    })
    this.fetchNewData()
  }

  @ViewChild(MatTable) table!: MatTable<any>;
  @ViewChild(MatSort) sort! : MatSort;
  resp: any = [];
  dataSource: MatTableDataSource<any> = new MatTableDataSource();
  myName = "";
  displayedColumns = ['date', 'toFrom', 'title', 'amount'];
  errorMessage = "";

  months = ['Styczeń', 'Luty', 'Marzec', 'Kwiecień', 'Maj', 'Czerwiec', 'Lipiec', 'Sierpień', 'Wrzesień', 'Październik',
    'Listopad', 'Grudzień'];

  balance = "2";
  period = "3";
  loading = false;

  today = new Date();

  formatDate1(value : string, date : string) {
    return formatDate(value, date, 'en-US');
  }

  showDetails(row: any) {
    this.dialog.open(HistoryDialogComponent, {
      data: row
    });
  }

  fetchNewData() {
    this.dataSource = new MatTableDataSource();
    var startingDate;
    var endingDate;
    this.loading = true;

    switch (this.period) {
      case '0':
        startingDate = new Date();
        startingDate.setDate(startingDate.getDate() - 30);
        break;
      case '1':
        startingDate = new Date();
        startingDate.setDate(1);
        break;
      case '2':
        startingDate = new Date();
        startingDate.setMonth(startingDate.getMonth() - 1);
        startingDate.setDate(1);
        endingDate = new Date(startingDate);
        endingDate.setMonth(endingDate.getMonth() + 1);
        endingDate.setDate(endingDate.getDate() - 1);
        break;
      case '3':
        startingDate = new Date();
        startingDate.setMonth(0);
        startingDate.setDate(1);
        break;
      case '4':
        startingDate = new Date();
        startingDate.setFullYear(startingDate.getFullYear() - 1);
        startingDate.setMonth(0);
        startingDate.setDate(1);
        endingDate = new Date(startingDate);
        endingDate.setFullYear(endingDate.getFullYear() + 1);
        endingDate.setDate(endingDate.getDate() - 1);
        break;
      case '5':
        startingDate = new Date();
        startingDate.setMonth(startingDate.getMonth() - 12);
        break;
    }


    this.http.getTransactions(this.balance, startingDate, endingDate).subscribe({
      next: (data: any) => {
        var resp = data.map((s: any) => (
          {
            date: this.formatDate1(s.data.transactionDate, 'yyyy-MM-dd'),
            toFrom: s.fromName == this.myName ? s.toName : s.fromName,
            title: s.data.transactionTitle,
            amount: s.fromName == this.myName ? -s.data.transactionAmmount : s.data.transactionAmmount,
            transactionSourceAccount: s.data.transactionSourceAccount,
            fromName: s.fromName,
            transactionDestinationAccount: s.data.transactionDestinationAccount,
            toName: s.toName,
            accountBalance: s.data.accountBalance
          }
        ));
        console.log(data);
        
        this.dataSource = new MatTableDataSource(resp);
        this.dataSource.sort = this.sort;
        this.loading = false;
      },
      error: (error: any) => {
        if (Math.floor(error.status / 100) == 4) this.errorMessage = error.message;
        if (Math.floor(error.status / 100) == 5) this.errorMessage = "Błąd serwera!";
        else this.errorMessage = "Niezidentyfikowany błąd!";
      }
    });
  }

}

import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DialogComponent } from './dialog/dialog.component';
import { FormControl } from '@angular/forms'
import { StorageService } from '../../shared/services/storage.service';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrl: './main.component.css'
})
export class MainComponent implements OnInit {

  kwota = 500;
  raty = 3;
  readonly oprocentowanie = 14.79;
  rata = 0;
  RRSO = 15.83;
  koszt = 0;
  rataS = '';
  kosztS = '';

  constructor(public dialog: MatDialog, private storage: StorageService) { }

  ngOnInit() {
    this.kwotaChange();
    this.storage.removeItem('token');
  }

  openDialog() {
    this.dialog.open(DialogComponent);
  }

  numToString(num: number) {
    var str = num.toString();
    var pos = str.indexOf('.');
    var decimal = '';
    if (pos != -1) {
      decimal = str.substring(pos + 1);
      str = str.substring(0, pos);
    }
    var lenght = str.length;
    var offset = lenght % 3;
    var strings = [];
    if (offset != 0)
      strings.push(str.substring(0, offset))
    for (var i = 0; i < Math.floor(lenght / 3); i++) {
      strings.push(str.substring(offset + i * 3, offset + (i + 1) * 3));
    }

    str = strings.join(' ');
    if (pos != -1) {
      str += "," + decimal;
    }
    return str;
  }


  kwotaChange() {
    if (this.kwota == null || this.kwota == undefined) {
      this.kwota = 500;
    }
    else if (this.kwota < 500) {
      this.kwota = 500;
    }
    else if (this.kwota > 200000) {
      this.kwota = 200000;
    }
    if (this.raty == null || this.raty == undefined) {
      this.raty = 3;
    }
    else if (this.raty < 3) {
      this.raty = 3;
    }
    else if (this.raty > 120) {
      this.raty = 120;
    }

    var temp = 1 + (this.oprocentowanie / 1200);
    var temp2 = 0;
    for (var n = 1; n <= this.raty; n++) {
      temp2 += Math.pow(temp, -n);
    }

    this.rata = Number((this.kwota / temp2).toFixed(2));
    this.koszt = Number(((this.rata * this.raty) - this.kwota).toFixed(2));
    this.rataS = this.numToString(this.rata);
    this.kosztS = this.numToString(this.koszt);
  }

}

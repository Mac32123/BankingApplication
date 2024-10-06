import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainComponent } from './main/main.component';
import { FinanceComponent } from './finance/finance.component';
import { TransactionComponent } from './transaction/transaction.component';
import { TransactionSuccessComponent } from './transaction/transaction-success/transaction-success.component';
import { HistoryComponent } from './history/history.component';
import { SettingsComponent } from './settings/settings.component';
import { TestComponent } from './settings/test/test.component';
import { LiignComponent } from './unauthorized/liign/liign.component';
import { ContactComponent } from './unauthorized/contact/contact.component';

const routes: Routes = [
  { path: 'main', component: MainComponent },
  { path: 'contact', component: ContactComponent },
  { path: 'login', component: LiignComponent },
  { path: 'finanse', component: FinanceComponent },
  { path: 'przelewy', component: TransactionComponent },
  { path: 'success', component: TransactionSuccessComponent },
  { path: 'historia', component: HistoryComponent },
  { path: 'ustawienia', component: SettingsComponent },
  { path: '', component: MainComponent },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

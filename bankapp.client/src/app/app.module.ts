import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { MaterialModule } from './material/material.module';
import { MenubarComponent } from './menubar/menubar.component';
import { tokenInterceptor } from './token.interceptor';
import { MenubarClientComponent } from './menubar-client/menubar-client.component';
import { FinanceComponent } from './finance/finance.component';
import { TransactionComponent } from './transaction/transaction.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ValidationModule } from './validation/validation.module';
import { TransactionDialogComponent } from './transaction/transaction-dialog/transaction-dialog.component';
import { TransactionSuccessComponent } from './transaction/transaction-success/transaction-success.component';
import { HistoryComponent } from './history/history.component';
import { HistoryDialogComponent } from './history/history-dialog/history-dialog.component';
import { SettingsComponent } from './settings/settings.component';
import { TestComponent } from './settings/test/test.component';
import { MatSortModule } from '@angular/material/sort';
import { NgxSkeletonLoaderModule } from 'ngx-skeleton-loader'


@NgModule({
  declarations: [
    AppComponent,
    MenubarComponent,
    MenubarClientComponent,
    FinanceComponent,
    TransactionComponent,
    TransactionDialogComponent,
    TransactionSuccessComponent,
    HistoryComponent,
    HistoryDialogComponent,
    SettingsComponent,
    TestComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MaterialModule,
    ValidationModule,
    ReactiveFormsModule,
    MatSortModule,
    NgxSkeletonLoaderModule
  ],
  providers: [
    provideAnimationsAsync(),
    { provide: HTTP_INTERCEPTORS, useClass: tokenInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

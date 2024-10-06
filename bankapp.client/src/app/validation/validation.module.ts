import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ValidationMessageComponent } from './validation-messages/validation-message/validation-message.component';
import { ValidationMessagesComponent } from './validation-messages/validation-messages.component';



@NgModule({
  declarations: [
    ValidationMessageComponent,
    ValidationMessagesComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    ValidationMessageComponent,
    ValidationMessagesComponent
  ]
})
export class ValidationModule { }

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../material/material.module';
import { ContactComponent } from './contact/contact.component';
import { ReactiveFormsModule } from '@angular/forms';
import { ValidationModule } from '../validation/validation.module';
import { LiignComponent } from './liign/liign.component';



@NgModule({
  declarations: [
    ContactComponent,
    LiignComponent
  ],
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    ValidationModule
  ],
  exports: [
    ContactComponent,
    LiignComponent
  ]
})
export class UnauthorizedModule { }

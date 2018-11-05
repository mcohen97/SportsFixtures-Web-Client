import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import {MatPaginator, MatSort, MatTableDataSource, MatDialogRef, MAT_DIALOG_DATA, MatDialog, ErrorStateMatcher} from '@angular/material';
import { UsersComponent } from '../users.component';
import { Injectable } from "@angular/core";
import { Http, Response, RequestOptions, Headers } from "@angular/http"; 
import { Observable, throwError } from "rxjs";  
import { map, tap, catchError } from 'rxjs/operators'; 
import { Globals } from "src/app/globals";
import { User } from "src/app/classes/user";
import { ErrorResponse } from "src/app/classes/error";
import { Validators, FormControl, FormGroupDirective, NgForm, ValidationErrors, ValidatorFn, AbstractControl } from '@angular/forms';
import { UsersService } from 'src/app/services/users/users.service';
import { UserError } from 'src/app/classes/userError';
import { CustomValidators } from 'src/app/classes/custom-validators';

@Component({
  selector: 'app-user-dialog',
  templateUrl: './user-dialog.component.html',
  styleUrls: ['./user-dialog.component.css']
})
export class UserDialogComponent {

  userError = new UserError();
  errorFlags = [];
  updatePassword = true;
  usernameControl:FormControl;
  passwordControl:FormControl;
  passwordConfirmationControl:FormControl;
  nameControl:FormControl;
  surnameControl:FormControl;
  emailControl:FormControl;


  constructor(
    public dialogRef: MatDialogRef<UserDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data:UserDialogData,
    private globals:Globals, 
    private usersService: UsersService
  ) { 
    this.resetErrors();
    this.usernameControl = new FormControl();
    this.passwordControl = new FormControl();
    this.passwordConfirmationControl = new FormControl();
    this.nameControl = new FormControl();
    this.surnameControl = new FormControl();
    this.emailControl = new FormControl();
    this.updatePassword = this.data.isNewUser;
    if(!this.data.isNewUser){
      this.usernameControl.disable();
      this.passwordConfirmationControl.disable();
      this.passwordControl.disable();
    }
    this.setValidators();
  }

  matcher = new MyErrorStateMatcher();

  onNoClick():void {
    this.dialogRef.close();
  }

  onSaveClick():void{
    this.markControlsAsTouched();
    this.updateControls();
    if(this.allValid())
      this.data.isNewUser ? this.addUser(this.data.aUser) : this.updateUser(this.data.aUser);
  }
  allValid(): boolean {
    return (this.usernameControl.valid || this.usernameControl.disabled)
      && (this.passwordControl.valid || this.passwordControl.disabled)
      && (this.passwordConfirmationControl.valid || this.passwordConfirmationControl.disabled)
      && this.nameControl.valid
      && this.surnameControl.valid
      && this.emailControl.valid
  }

  addUser(newUser:User):void{
    this.usersService.addUser(newUser).subscribe(
      ((result:User) => {
        this.dialogRef.close(newUser);
      }),
      ((error:ErrorResponse) => this.handleError(error))
    );
  }

  updateUser(userEdited:User):void{
    this.usersService.modifyUser(userEdited).subscribe(
      ((result:User) => {
        this.dialogRef.close(userEdited);
      }),
      ((error:ErrorResponse) => this.handleError(error))
    );
  }

  handleError(error: ErrorResponse): void {    
    this.userError = <UserError> error.errorObject;
    this.checkErrors();
    this.setValidators();
    this.markControlsAsTouched();
    this.updateControls();
    this.resetErrors();
  }

  private updateControls() {
    this.usernameControl.updateValueAndValidity();
    this.passwordControl.updateValueAndValidity();
    this.passwordConfirmationControl.updateValueAndValidity();
    this.nameControl.updateValueAndValidity();
    this.surnameControl.updateValueAndValidity();
    this.emailControl.updateValueAndValidity();
  }

  private markControlsAsTouched() {
    this.usernameControl.markAsTouched();
    this.passwordControl.markAsTouched();
    this.passwordConfirmationControl.markAsTouched();
    this.emailControl.markAsTouched();
    this.nameControl.markAsTouched();
    this.surnameControl.markAsTouched();
  }

  private checkErrors() {
    this.errorFlags['usernameInput'] = this.userError.Username != undefined;
    this.errorFlags['passwordInput'] = this.userError.Password != undefined;
    this.errorFlags['passwordConfirmationInput'] = this.passwordConfirmationControl.value != this.passwordConfirmationControl.value;
    this.errorFlags['nameInput'] = this.userError.Name != undefined;
    this.errorFlags['surnameInput'] = this.userError.Surname != undefined;
    this.errorFlags['emailInput'] = this.userError.Email != undefined;
    this.errorFlags['errorMessageInput'] = this.userError.errorMessage != undefined;
  }

  private resetErrors(){
    this.errorFlags['usernameInput'] = false;
    this.errorFlags['passwordInput'] = false;
    this.errorFlags['nameInput'] = false;
    this.errorFlags['surnameInput'] = false;
    this.errorFlags['emailInput'] = false;
    this.errorFlags['errorMessageInput'] = false;
    this.errorFlags['passwordConfirmationInput'] = false;
  }

  private setValidators() {
    this.usernameControl.setValidators([
      this.existError("usernameInput"),
      this.existError("errorMessageInput"),
      Validators.required
    ]);
    this.passwordControl.setValidators([
      this.existError("passwordInput"),
      Validators.required
    ]);
    this.passwordConfirmationControl.setValidators([
      this.existError("passwordConfirmationInput"),
      Validators.required
    ]);
    this.nameControl.setValidators([
      this.existError("nameInput"),
      Validators.required
    ]);
    this.surnameControl.setValidators([
      this.existError("surnameInput"),
      Validators.required
    ]);
    this.emailControl.setValidators([
      this.existError("emailInput"),
      Validators.required,
      Validators.email
    ]);
  }

  private existError(keyError:string): ValidatorFn {
    return (control:AbstractControl) : ValidationErrors | null=>{
      if(this.errorFlags[keyError]){
        let temp = [];
        temp[keyError] = true;
        return temp;
      }
      else
        return null;
    }
  }

  removeError(control:AbstractControl, keyError:string):void{
    control.setErrors(null);
    this.errorFlags[keyError] = false;
    control.updateValueAndValidity();
  }

  togglePasswordEnabled():void{
    if(this.updatePassword){
      this.passwordControl.enable();
      this.passwordConfirmationControl.enable();
    }
    else{
      this.passwordControl.disable();
      this.passwordConfirmationControl.disable();
    }
  }

  checkPasswordConfirmation(){
    this.errorFlags['passwordConfirmationInput'] = this.passwordControl.value != this.passwordConfirmationControl.value;
    this.passwordConfirmationControl.markAsTouched();
    this.passwordConfirmationControl.updateValueAndValidity();
  }

}

export class MyErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null)   : boolean {
    const isSubmitted = form && form.submitted;
    return control && control.invalid && (control.dirty || control.touched);
  }
}

export interface UserDialogData{
  aUser: User;
  isNewUser: boolean;
  tiitle: string;
}
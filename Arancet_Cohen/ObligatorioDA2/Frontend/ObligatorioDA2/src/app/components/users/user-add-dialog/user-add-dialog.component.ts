import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import {MatPaginator, MatSort, MatTableDataSource, MatDialogRef, MAT_DIALOG_DATA, MatDialog, ErrorStateMatcher} from '@angular/material';
import { UsersComponent } from '.././users.component';
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
  selector: 'app-user-add-dialog',
  templateUrl: './user-add-dialog.component.html',
  styleUrls: ['./user-add-dialog.component.css']
})
export class UserAddDialogComponent {

  userError = new UserError();
  errorBooleans = [];
  
  usernameControl:FormControl;
  passwordControl:FormControl;
  nameControl:FormControl;
  surnameControl:FormControl;
  emailControl:FormControl;

  constructor(
    public dialogRef: MatDialogRef<UserAddDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data:User,
    private globals:Globals, 
    private usersService: UsersService
  ) { 
    this.resetErrors();
    this.usernameControl = new FormControl('',);
    this.passwordControl = new FormControl('',);
    this.nameControl = new FormControl('',);
    this.surnameControl = new FormControl('',);
    this.emailControl = new FormControl('',);
    this.setValidators();
  }

  matcher = new MyErrorStateMatcher();

  onNoClick():void {
    this.dialogRef.close();
  }

  onSaveClick():void{
    this.addUser(this.data);
  }

  addUser(newUser:User):void{
    this.usersService.addUser(newUser).subscribe(
      ((result:User) => {
        this.dialogRef.close(newUser);
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
    console.log(this.usernameControl.errors); 
  }

  private updateControls() {
    this.usernameControl.updateValueAndValidity();
    this.passwordControl.updateValueAndValidity();
    this.nameControl.updateValueAndValidity();
    this.surnameControl.updateValueAndValidity();
    this.emailControl.updateValueAndValidity();
  }

  private markControlsAsTouched() {
    this.usernameControl.markAsTouched();
    this.passwordControl.markAsTouched();
    this.emailControl.markAsTouched();
    this.nameControl.markAsTouched();
    this.surnameControl.markAsTouched();
  }

  private checkErrors() {
    this.errorBooleans['usernameInput'] = this.userError.Username != undefined;
    this.errorBooleans['passwordInput'] = this.userError.Password != undefined;
    this.errorBooleans['nameInput'] = this.userError.Name != undefined;
    this.errorBooleans['surnameInput'] = this.userError.Surname != undefined;
    this.errorBooleans['emailInput'] = this.userError.Email != undefined;
    this.errorBooleans['errorMessageInput'] = this.userError.errorMessage != undefined;
  }

  private resetErrors(){
    this.errorBooleans['usernameInput'] = false;
    this.errorBooleans['passwordInput'] = false;
    this.errorBooleans['nameInput'] = false;
    this.errorBooleans['surnameInput'] = false;
    this.errorBooleans['emailInput'] = false;
    this.errorBooleans['errorMessageInput'] = false;
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
      if(this.errorBooleans[keyError]){
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
    this.errorBooleans[keyError] = false;
    control.updateValueAndValidity();
  }

}

export class MyErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null)   : boolean {
    const isSubmitted = form && form.submitted;
    return control && control.invalid && (control.dirty || control.touched);
  }
}

export interface ErrorsBools{
  usernameError:boolean;
  passwordError:boolean;
  nameError:boolean;
  surnameError:boolean;
  emailError:boolean;
  errorMessage:boolean;
}
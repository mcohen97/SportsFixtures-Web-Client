import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import {MatPaginator, MatSort, MatTableDataSource, MatDialogRef, MAT_DIALOG_DATA, MatDialog, ErrorStateMatcher} from '@angular/material';
import { SportsComponent } from '../sports.component';
import { Injectable } from "@angular/core";
import { Http, Response, RequestOptions, Headers } from "@angular/http"; 
import { Observable, throwError } from "rxjs";  
import { map, tap, catchError } from 'rxjs/operators'; 
import { Globals } from "src/app/globals";
import { Sport } from "src/app/classes/sport";
import { ErrorResponse } from "src/app/classes/error";
import { Validators, FormControl, FormGroupDirective, NgForm, ValidationErrors, ValidatorFn, AbstractControl } from '@angular/forms';
import { SportsService } from 'src/app/services/sports/sports.service';
import { SportError } from 'src/app/classes/sportError';
import { CustomValidators } from 'src/app/classes/custom-validators';

@Component({
  selector: 'app-sport-dialog',
  templateUrl: './sport-dialog.component.html',
  styleUrls: ['./sport-dialog.component.css']
})
export class SportDialogComponent{

  sportError = new SportError();
  errorFlags = [];
  nameControl:FormControl;


  constructor(
    public dialogRef: MatDialogRef<SportDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data:SportDialogData,
    private sportsService: SportsService
  ) { 
    this.resetErrors();
    this.nameControl = new FormControl();
    this.setValidators();
  }

  matcher = new MyErrorStateMatcher();

  onNoClick():void {
    this.dialogRef.close();
  }

  onSaveClick():void{
    this.markControlsAsTouched();
    this.updateControls();
    this.addSport(this.data.aSport);
  }

  allValid(): boolean {
    return this.nameControl.valid;
  }

  addSport(elementToAdd:Sport):void{
    this.sportsService.addSport(elementToAdd).subscribe(
      ((result:Sport) => {
        this.dialogRef.close(elementToAdd);
      }),
      ((error:ErrorResponse) => this.handleError(error))
    );
  }


  handleError(error: ErrorResponse): void {    
    this.sportError = <SportError> error.errorObject;
    this.checkErrors();
    this.setValidators();
    this.markControlsAsTouched();
    this.updateControls();
    this.resetErrors();
  }

  private updateControls() {
    this.nameControl.updateValueAndValidity();
  }

  private markControlsAsTouched() {
    this.nameControl.markAsTouched();
  }

  private checkErrors() {
    this.errorFlags['nameInput'] = this.sportError.Name != undefined;
    this.errorFlags['errorMessageInput'] = this.sportError.errorMessage != undefined;
  }

  private resetErrors(){
    this.errorFlags['nameInput'] = false;
    this.errorFlags['errorMessageInput'] = false;
  }

  private setValidators() {
    this.nameControl.setValidators([
      this.existError("nameInput"),
      this.existError("errorMessageInput"),
      Validators.required
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

}

export class MyErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null)   : boolean {
    const isSubmitted = form && form.submitted;
    return control && control.invalid && (control.dirty || control.touched);
  }
}

export interface SportDialogData{
  aSport: Sport;
  tiitle: string;
}
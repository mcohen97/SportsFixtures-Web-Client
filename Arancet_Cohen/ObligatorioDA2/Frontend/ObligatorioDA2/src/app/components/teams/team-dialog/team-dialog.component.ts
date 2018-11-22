import { Component, OnInit, ViewChild, Inject, ElementRef } from '@angular/core';
import {MatPaginator, MatSort, MatTableDataSource, MatDialogRef, MAT_DIALOG_DATA, MatDialog, ErrorStateMatcher} from '@angular/material';
import { TeamsComponent } from '../teams.component';
import { Injectable } from "@angular/core";
import { Http, Response, RequestOptions, Headers } from "@angular/http"; 
import { Observable, throwError } from "rxjs";  
import { map, tap, catchError } from 'rxjs/operators'; 
import { Globals } from "src/app/globals";
import { Team } from "src/app/classes/team";
import { ErrorResponse } from "src/app/classes/error";
import { Validators, FormControl, FormGroupDirective, NgForm, ValidationErrors, ValidatorFn, AbstractControl } from '@angular/forms';
import { TeamsService } from 'src/app/services/teams/teams.service';
import { TeamError } from 'src/app/classes/teamError';
import { CustomValidators } from 'src/app/classes/custom-validators';

@Component({
  selector: 'app-team-dialog',
  templateUrl: './team-dialog.component.html',
  styleUrls: ['./team-dialog.component.css']
})
export class TeamDialogComponent {

  teamError = new TeamError();
  errorFlags = [];
  errorStatus = 200;
  nameControl:FormControl;
  sportNameControl:FormControl;
  photoControl:FormControl;
  files: any;
  selectedImage:any;


  constructor(
    public dialogRef: MatDialogRef<TeamDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data:TeamDialogData,
    private teamsService: TeamsService
  ) { 
    this.resetErrors();
    this.nameControl = new FormControl();
    this.sportNameControl = new FormControl();
    this.photoControl = new FormControl();
    if(!data.isNewTeam){
      this.nameControl.disable();
      this.sportNameControl.disable();
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
      this.data.isNewTeam ? this.addTeam(this.data.aTeam) : this.updateTeam(this.data.aTeam);
  }
  
  allValid(): boolean {
    return  (this.nameControl.valid || this.nameControl.disabled)
      && (this.sportNameControl.valid || this.sportNameControl.disabled)
      && this.photoControl.valid
  }

  addTeam(newTeam:Team):void{
    this.teamsService.addTeam(newTeam).subscribe(
      ((result:Team) => {
        newTeam.id = result.id;
        this.dialogRef.close(newTeam);
      }),
      ((error:ErrorResponse) => this.handleError(error))
    );
  }

  updateTeam(teamEdited:Team):void{
    this.teamsService.modifyTeam(teamEdited).subscribe(
      ((result:Team) => {
        this.dialogRef.close(teamEdited);
      }),
      ((error:ErrorResponse) => this.handleError(error))
    );
  }

  handleError(error: ErrorResponse): void {    
    this.teamError = <TeamError> error.errorObject;
    this.errorStatus = error.errorCode;
    this.checkErrors();
    this.setValidators();
    this.markControlsAsTouched();
    this.updateControls();
    this.resetErrors();
  }

  private updateControls() {
    this.nameControl.updateValueAndValidity();
    this.sportNameControl.updateValueAndValidity();
    this.photoControl.updateValueAndValidity();
  }

  private markControlsAsTouched() {
    this.photoControl.markAsTouched();
    this.nameControl.markAsTouched();
    this.sportNameControl.markAsTouched();
  }

  private checkErrors() {
    this.errorFlags['nameInput'] = this.teamError.Name != undefined;
    this.errorFlags['sportNameInput'] = this.teamError.SportName != undefined;
    this.errorFlags['photoInput'] = this.teamError.Photo != undefined;
    this.errorFlags['errorMessageNameInput'] = this.teamError.errorMessage && this.errorStatus == 400;
    this.errorFlags['errorMessageSportNameInput'] = this.teamError.errorMessage && this.errorStatus == 404;

  }

  private resetErrors(){
    this.errorFlags['sportNameInput'] = false;
    this.errorFlags['photoInput'] = false;
    this.errorFlags['nameInput'] = false;
    this.errorFlags['errorMessageNameInput'] = false;
    this.errorFlags['errorMessageSportNameInput'] = false;

  }

  private setValidators() {
    this.nameControl.setValidators([
      this.existError("nameInput"),
      this.existError("errorMessageNameInput"),
      Validators.required
    ]);
    this.sportNameControl.setValidators([
      this.existError("sportNameInput"),
      this.existError("errorMessageSportNameInput"),

      Validators.required
    ]);
    this.photoControl.setValidators([
      this.existError("photoInput"),
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

  getFiles(event) {
    var file: File = event.target.files[0];
    var reader = new FileReader();
    reader.onloadend = (e) => {
      var base64Image = <string> reader.result;
      this.data.aTeam.photo =  base64Image;
      console.log(this.data.aTeam.photo);
    }
    
    reader.readAsDataURL(file);

  }

  handleFileSelect(evt){
    var files = evt.target.files;
    var file = files[0];


  if (files && file) {
      var reader = new FileReader();

      reader.onload =this._handleReaderLoaded.bind(this);
      reader.readAsBinaryString(file);
  }
}



  _handleReaderLoaded(readerEvt) {
    var binaryString = readerEvt.target.result;
    this.data.aTeam.photo = btoa(binaryString);
  }

}

export class MyErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null)   : boolean {
    const isSubmitted = form && form.submitted;
    return control && control.invalid && (control.dirty || control.touched);
  }
}

export interface TeamDialogData{
  aTeam: Team;
  isNewTeam: boolean;
  title: string;
}
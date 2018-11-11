import { Component, OnInit, ViewChild, Inject, ElementRef } from '@angular/core';
import {MatPaginator, MatSort, MatTableDataSource, MatDialogRef, MAT_DIALOG_DATA, MatDialog, ErrorStateMatcher} from '@angular/material';
import { EncountersComponent } from '../encounters.component';
import { Injectable } from "@angular/core";
import { Http, Response, RequestOptions, Headers } from "@angular/http"; 
import { Observable, throwError } from "rxjs";  
import { map, tap, catchError } from 'rxjs/operators'; 
import { Globals } from "src/app/globals";
import { Encounter } from "src/app/classes/encounter";
import { ErrorResponse } from "src/app/classes/error";
import { Validators, FormControl, FormGroupDirective, NgForm, ValidationErrors, ValidatorFn, AbstractControl } from '@angular/forms';
import { EncountersService } from 'src/app/services/encounters/encounters.service';
import { EncounterError } from 'src/app/classes/encounterError';
import { CustomValidators } from 'src/app/classes/custom-validators';
import { SportsService } from 'src/app/services/sports/sports.service';
import { TeamsService } from 'src/app/services/teams/teams.service';
import { Sport } from 'src/app/classes/sport';
import { Team } from 'src/app/classes/team';

@Component({
  selector: 'app-encounter-result-dialog',
  templateUrl: './encounter-result-dialog.component.html',
  styleUrls: ['./encounter-result-dialog.component.css']
})
export class EncounterResultDialogComponent implements OnInit {
  ngOnInit(): void {
    
  }

  teams:Array<Team>;
  sports:Array<Sport>;
  encounterError = new EncounterError();
  errorFlags = [];
  errorStatus = 200;
  teamsIdsControl:FormControl;
  sportNameControl:FormControl;
  dateControl:FormControl;


  constructor(
    public dialogRef: MatDialogRef<EncounterResultDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data:EncounterDialogData,
    private encountersService: EncountersService,
    private sportsService:SportsService,
  ) { 
    this.getSports();
    this.resetErrors();
    this.teamsIdsControl = new FormControl();
    this.sportNameControl = new FormControl();
    this.dateControl = new FormControl();
    this.setValidators();
  }

  getTeams() {
    var sportName = this.sportNameControl.value;
    this.sportsService.getTeams(sportName).subscribe(
      ((teams:Array<Team>) => this.teams = teams),
      ((error:any) => this.handleError(error))
    );
  }

  getSports(){
    this.sportsService.getAllSports().subscribe(
      ((sports:Array<Sport>) => this.sports = sports),
      ((error:any) => this.handleError(error))
    );
  }

  matcher = new MyErrorStateMatcher();

  onNoClick():void {
    this.dialogRef.close();
  }

  onSaveClick():void{
    this.markControlsAsTouched();
    this.updateControls();
    if(this.allValid()){
      this.data.aEncounter.sportName = this.sportNameControl.value;
      this.data.aEncounter.teamIds = this.teamsIdsControl.value;
      this.data.aEncounter.date = this.dateControl.value;
      this.data.isNewEncounter ? this.addEncounter(this.data.aEncounter) : this.updateEncounter(this.data.aEncounter);
    }
  }
  allValid(): boolean {
    return  this.teamsIdsControl.valid
      && this.sportNameControl.valid
      && this.dateControl.valid
  }

  addEncounter(newEncounter:Encounter):void{
    this.encountersService.addEncounter(newEncounter).subscribe(
      ((result:Encounter) => {
        newEncounter.id = result.id;
        this.dialogRef.close(newEncounter);
      }),
      ((error:ErrorResponse) => this.handleError(error))
    );
  }

  updateEncounter(encounterEdited:Encounter):void{
    this.encountersService.modifyEncounter(encounterEdited).subscribe(
      ((result:Encounter) => {
        this.dialogRef.close(encounterEdited);
      }),
      ((error:ErrorResponse) => this.handleError(error))
    );
  }

  handleError(error: ErrorResponse): void {
    this.encounterError = <EncounterError> error.errorObject;
    this.errorStatus = error.errorCode;
    this.checkErrors();
    this.setValidators();
    this.markControlsAsTouched();
    this.updateControls();
    this.resetErrors();
  }

  private updateControls() {
    this.teamsIdsControl.updateValueAndValidity();
    this.sportNameControl.updateValueAndValidity();
    this.dateControl.updateValueAndValidity();
  }

  private markControlsAsTouched() {
    this.dateControl.markAsTouched();
    this.teamsIdsControl.markAsTouched();
    this.sportNameControl.markAsTouched();
  }

  private checkErrors() {
    this.errorFlags['sportNameInput'] = this.encounterError.SportName != undefined;
    this.errorFlags['dateInput'] = this.encounterError.Date != undefined;
    this.errorFlags['errorMessageInput'] = this.encounterError.errorMessage && this.errorStatus == 404;
    this.errorFlags['teamsInput'] = this.encounterError.errorMessage && this.errorStatus == 400;
  }

  private resetErrors(){
    this.errorFlags['sportNameInput'] = false;
    this.errorFlags['dateInput'] = false;
    this.errorFlags['errorMessageInput'] = false;
    this.errorFlags['teamsInput'] = false;

  }

  private setValidators() {
    this.teamsIdsControl.setValidators([
      this.existError("teamsInput"),
      Validators.required
    ]);
    this.sportNameControl.setValidators([
      this.existError("sportNameInput"),
      this.existError("errorMessageInput"),
      Validators.required
    ]);
    this.dateControl.setValidators([
      this.existError("dateInput"),
      Validators.required,
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

export interface EncounterDialogData{
  aEncounter: Encounter;
  isNewEncounter: boolean;
  tiitle: string;
}

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
import { FixturesService } from 'src/app/services/fixtures/fixture.service';
import { Fixture } from 'src/app/classes/fixture';
import { FixtureError } from 'src/app/classes/fixtureError';
import { GenericError } from 'src/app/classes/genericError';

@Component({
  selector: 'app-encounter-dialog',
  templateUrl: './encounter-dialog.component.html',
  styleUrls: ['./encounter-dialog.component.css']
})
export class EncounterDialogComponent implements OnInit{
  //fixtureError = new FixtureError();
  teams:Array<Team>;
  sports:Array<Sport>;
 //encounterError = new EncounterError();
  genericError = new GenericError();
  errorFlags = [];
  errorStatus = 200;
  teamsIdsControl:FormControl;
  sportNameControl:FormControl;
  dateControl:FormControl;  
  methodControl:FormControl;
  fixturesNames:Array<string>;
  selectedDate:Date;
  
  ngOnInit(): void {
    
  }


  constructor(
    public dialogRef: MatDialogRef<EncounterDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data:EncounterDialogData,
    private encountersService: EncountersService,
    private sportsService:SportsService,
    private fixturesService:FixturesService
  ) { 
    this.getSports();
    this.getFixtures();
    this.resetErrors();
    this.teamsIdsControl = new FormControl();
    this.sportNameControl = new FormControl();
    this.selectedDate = new Date(Date.now());
    this.dateControl = new FormControl(this.selectedDate);
    this.methodControl = new FormControl();
    if(!this.data.isNewEncounter){
      this.methodControl.setValue("single");
      this.methodControl.disable();
      this.sportNameControl.setValue(this.data.aEncounter.sportName);
      this.sportSelected();
      this.sportNameControl.disable();
      this.teamsIdsControl.setValue(this.data.aEncounter.teams.map(t => t.id));
    }
      
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

  getFixtures(){
    this.fixturesService.getFixturesNames().subscribe(
      ((fixtures:Array<string>) => this.fixturesNames = fixtures),
      ((error:any) => this.handleError(error))
    );
  }

  cleanFixtureName(fixture:string):string{
    var parts = fixture.split('.');
    return parts[parts.length-1];
  }

  matcher = new MyErrorStateMatcher();

  onNoClick():void {
    this.dialogRef.close();
  }

  onSaveClick():void{
    this.markControlsAsTouched();
    this.updateControls();
    if(this.allValid()){
      if(this.methodControl.value == "single"){
        this.data.aEncounter.sportName = this.sportNameControl.value;
        this.data.aEncounter.teamIds = this.teamsIdsControl.value;
        this.data.aEncounter.date = this.dateControl.value;
        this.data.isNewEncounter ? this.addEncounter(this.data.aEncounter) : this.updateEncounter(this.data.aEncounter);
      }else{
        var date = new Date(this.dateControl.value);
        var fixture = new Fixture(this.methodControl.value, date.getDate(), date.getMonth()+1, date.getFullYear(), this.sportNameControl.value);
        this.addFixture(fixture);
      }    
    }
  }

  allValid(): boolean {
    return  (this.teamsIdsControl.valid || this.teamsIdsControl.disabled)
      && (this.sportNameControl.valid || this.sportNameControl.disabled)
      && this.dateControl.valid
      && (this.methodControl.valid || this.methodControl.disabled)
  }

  addEncounter(newEncounter:Encounter):void{
    this.encountersService.addEncounter(newEncounter).subscribe(
      ((result:Encounter) => {
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

  addFixture(fixture:Fixture):void{
    this.fixturesService.addFixture(fixture).subscribe(
      ((result: Fixture) => this.dialogRef.close()),
      ((error:ErrorResponse) => this.handleError(error))
    )
  }

  handleError(error: ErrorResponse): void {
    this.genericError = <GenericError> error.errorObject;
    this.genericError.errorCode = error.errorCode;
    this.validate();
  }

 /* handleErrorFixture(error: ErrorResponse) : void {
    this.fixtureError = <FixtureError> error.errorObject;
    this.errorStatus = error.errorCode;
    this.validate();
    console.log(this.fixtureError.errorMessage);
  }*/

  private validate() {
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
    this.methodControl.updateValueAndValidity();
  }

  private markControlsAsTouched() {
    this.dateControl.markAsTouched();
    this.teamsIdsControl.markAsTouched();
    this.sportNameControl.markAsTouched();
    this.methodControl.markAsTouched();
  }

  private checkErrors() { //do error switch with every posible error message
    /*this.errorFlags['sportNameInput'] = this.encounterError.SportName != undefined;
    this.errorFlags['dateInput'] = this.encounterError.Date != undefined;
    this.errorFlags['errorMessageInput'] = this.encounterError.errorMessage && this.errorStatus == 404;
    this.errorFlags['teamsInput'] = this.encounterError.errorMessage && this.errorStatus == 400;*/

    if(this.genericError.errorMessage != undefined){
      if(this.genericError.errorCode == 404){
        this.errorFlags['sportNotFound'] = this.genericError.errorMessage == "Sport not found";
        this.errorFlags['teamsInput'] = this.genericError.errorMessage != "Sport not found"
      }
      if(this.genericError.errorCode == 400) {
        this.errorFlags['wrongTeams'] = this.genericError.errorMessage == "A match can't have less than 2 teams" || this.genericError.errorMessage == "The sport does not allow more than two teams";
        this.errorFlags['teamAlreadyHasMatch'] = !this.errorFlags['wrongTeams'];
      } 
    } else if (this.genericError.errorCode == 404){
      this.errorFlags['fixtureNotFound'] = this.methodControl.value != "single";
    }
   
  }   

  private resetErrors(){
    for(var key in this.errorFlags){
      this.errorFlags[key] = false;
    }
    /*
    this.errorFlags['teamsInput'] = false;
    this.errorFlags['sportNotFound'] = false;
    this.errorFlags['teamAlreadyHasMatch'] = false;
    this.errorFlags['lessThanTwoTeams'] = false;
    this.errorFlags['fixtureNotFound'] = false*/
  }

  private setValidators() {
    this.teamsIdsControl.setValidators([     
      this.existError("teamsInput"),
      this.existError("wrongTeams"),
      Validators.required
    ]);
    this.sportNameControl.setValidators([
      this.existError("sportNotFound"),
      Validators.required
    ]);
    this.dateControl.setValidators([
      this.existError("teamAlreadyHasMatch"),
      Validators.required,
    ]);
    this.methodControl.setValidators([
      this.existError("fixtureNotFound"),
      Validators.required,
    ])
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

  removeAllErrors(){
    this.resetErrors();
    this.teamsIdsControl.setErrors(null);
    this.sportNameControl.setErrors(null);
    this.dateControl.setErrors(null);
    this.methodControl.setErrors(null);
  }

  enableTeamSelect(methodSelected:string):void{
    methodSelected == "single" ? this.teamsIdsControl.enable() : this.teamsIdsControl.disable();
  }

  sportSelected(){
    this.getTeams();
    this.removeAllErrors();
    this.updateControls();
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
  title: string;
}
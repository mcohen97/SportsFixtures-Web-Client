import { Component, OnInit, ViewChild, Inject, ElementRef } from '@angular/core';
import {MatPaginator, MatSort, MatTableDataSource, MatDialogRef, MAT_DIALOG_DATA, MatDialog, ErrorStateMatcher} from '@angular/material';
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
import { UsersService } from 'src/app/services/users/users.service';
import { OkMessage } from 'src/app/classes/okMessage';

@Component({
  selector: 'app-follow-team-dialog',
  templateUrl: './follow-team-dialog.component.html',
  styleUrls: ['./follow-team-dialog.component.css']
})
export class FollowTeamDialogComponent implements OnInit{
  teams:Array<Team>;
  sports:Array<Sport>;
  genericError = new GenericError();
  errorFlags = [];
  errorStatus = 200;
  teamIdControl:FormControl;
  sportNameControl:FormControl;
  
  ngOnInit(): void {
    
  }


  constructor(
    public dialogRef: MatDialogRef<FollowTeamDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data:EncounterDialogData,
    private sportsService:SportsService,
    private usersService:UsersService
  ) { 
    this.getSports();
    this.resetErrors();
    this.teamIdControl = new FormControl();
    this.sportNameControl = new FormControl();
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
      this.followTeam();
    }
  }
  followTeam(){
    var selectedTeamId = this.teamIdControl.value;
    this.usersService.followTeam(selectedTeamId).subscribe(
      ((response:OkMessage) => this.dialogRef.close(OkMessage)),
      ((error:ErrorResponse) => this.handleError(error))
    );
  }

  allValid(): boolean {
    return  (this.teamIdControl.valid || this.teamIdControl.disabled)
      && this.sportNameControl.valid
  }

  handleError(error: ErrorResponse): void {
    this.genericError = <GenericError> error.errorObject;
    if(!this.genericError)
      this.genericError = new GenericError();
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
    this.teamIdControl.updateValueAndValidity();
    this.sportNameControl.updateValueAndValidity();
  }

  private markControlsAsTouched() {
    this.teamIdControl.markAsTouched();
    this.sportNameControl.markAsTouched();
  }

  private checkErrors() { //do error switch with every posible error message
    /*this.errorFlags['sportNameInput'] = this.encounterError.SportName != undefined;
    this.errorFlags['dateInput'] = this.encounterError.Date != undefined;
    this.errorFlags['errorMessageInput'] = this.encounterError.errorMessage && this.errorStatus == 404;
    this.errorFlags['teamsInput'] = this.encounterError.errorMessage && this.errorStatus == 400;*/

    if(this.genericError.errorMessage != undefined){
      if(this.genericError.errorCode == 404){
        this.errorFlags['teamNotFound'] = this.genericError.errorMessage == "Team not found";
      }
      if(this.genericError.errorCode == 400) {
        this.errorFlags['alreadyFollower'] = this.genericError.errorMessage == "User already follows team";
      } 
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
    this.teamIdControl.setValidators([     
      this.existError("teamNotFound"),
      this.existError("alreadyFollower"),
      Validators.required
    ]);
    this.sportNameControl.setValidators([
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

  removeAllErrors(){
    this.resetErrors();
    this.teamIdControl.setErrors(null);
    this.sportNameControl.setErrors(null);
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
  tiitle: string;
}
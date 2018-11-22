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
import { Standing } from 'src/app/classes/standing';
import { Result } from 'src/app/classes/result';

@Component({
  selector: 'app-encounter-result-dialog',
  templateUrl: './encounter-result-dialog.component.html',
  styleUrls: ['./encounter-result-dialog.component.css']
})
export class EncounterResultDialogComponent implements OnInit {
  ngOnInit(): void {
    
  }

  sportName:string;
  positions:Array<number>;
  positionsTaken:Array<boolean>;
  encounterError = new EncounterError();
  tempPreviousValue:number
  error = "";
  selectedWinner:number;

  constructor(
    public dialogRef: MatDialogRef<EncounterResultDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data:EncounterResultDialogData,
    private encountersService: EncountersService,
    private sportsService:SportsService,
    ) { 
      this.positionsTaken = new Array<boolean>(data.teams.length+1);
      this.positions = new Array<number>();
      for(var i = 1; i<=data.teams.length;i++){
        this.positions.push(i);
        this.positionsTaken[i] = false;
      }
  }

  getPositionsAvailable():Array<number>{
    var positionsAvailable = new Array<number>();
    for (let i = 1; i <= this.positions.length; i++) {
      if(!this.positionsTaken[i]){
        positionsAvailable.push(i);
      }      
    }
    return positionsAvailable;
  }

  updateAvailablePositions(){
    debugger;
    for (let i = 0; i < this.positionsTaken.length; i++) {
      this.positionsTaken[i] = false;      
    }
    var elems = document.getElementsByName("teamPosition");

    elems.forEach(element => {
      this.positionsTaken[Number.parseInt(element.innerText)] = true;
    });
    this.positions = this.getPositionsAvailable();
  }

  onNoClick():void {
    this.dialogRef.close();
  }

  onSaveClick():void{
    var standings = new Array<Standing>();
    if(!this.data.isTwoTeams){
      this.data.teams.forEach(team => {
        var selectedPosition = Number.parseInt(document.getElementById(team.name).innerText);
        var standing  = new Standing();
        standing.teamId = team.id;
        standing.position = selectedPosition;
        standings.push(standing);
      });
    }else{
      if (this.selectedWinner != 0){
        var winner = this.data.teams.find(t => t.id == this.selectedWinner);
        var looser = this.data.teams.find(t => t.id != this.selectedWinner);
        var winnerStd = new Standing();
        winnerStd.teamId = winner.id;
        winnerStd.position = 1;
        var looserStd = new Standing();
        looserStd.teamId = looser.id;
        looserStd.position = 2;
        standings.push(winnerStd);
        standings.push(looserStd);
      } else {
        standings.push(Standing.newStanding(this.data.teams[0].id, 1));
        standings.push(Standing.newStanding(this.data.teams[1].id, 1));
      }
    }
    var result = new Result();
    result.team_position = standings;
    this.encountersService.addResult(this.data.encounterId, result).subscribe(
      ((result:Encounter) => this.dialogRef.close(true)),
      ((error:ErrorResponse) => this.handleError(error))
    );
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
    this.error = error.errorMessage;
  }

}

export interface EncounterResultDialogData{
  encounterId:number;
  teams:Array<Team>;
  isTwoTeams:boolean;
  isNewResult:boolean;
  title: string;
}

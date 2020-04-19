import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { Encounter } from 'src/app/classes/encounter';
import { EncountersService } from 'src/app/services/encounters/encounters.service';
import { ErrorResponse } from 'src/app/classes/error';
import { EventDeckComponent } from './event-deck/event-deck.component';
import { TeamsService } from 'src/app/services/teams/teams.service';
import { Team } from 'src/app/classes/team';
import { SportsService } from 'src/app/services/sports/sports.service';
import { FormControl } from '@angular/forms';
import { yearsPerPage } from '@angular/material/datepicker/typings/multi-year-view';
import { MatDatepicker } from '@angular/material';
import { Sport } from 'src/app/classes/sport';
 

@Component({
  selector: 'app-calendar',
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.css']
})
export class CalendarComponent implements OnInit {

  ngOnInit() {
    this.dataRetrieved = false;
    this.actualMonth = new Date(Date.now());
    this.isLoading = true;
    this.encounters = new Array<Encounter>();
    this.getAllSports();
  }
  errorMessage:string;
  encountersBySport = [];
  tempEncounters:Array<Encounter>;
  actualMonth:Date;
  teams:Array<Team>;
  dataRetrieved:boolean;
  dateControl:FormControl;
  sports: Array<Sport>;
  encounters:Array<Encounter>;
  isLoading:boolean;
  
  constructor(private sportService:SportsService, private encountersService:EncountersService, private teamsService:TeamsService) {
    this.dateControl = new FormControl();
  }

  getAllSports(){
    this.sportService.getAllSports().subscribe(
      ((sports:Array<Sport>) => {
        /*this.sports = sports;
        this.sports.forEach(sport =>{
          this.getEncountersOfSport(sport.name);
        })*/
        this.sports = sports;
        this.getAllEncounters();
      }),
      ((error:ErrorResponse) => this.handleError(error)),
    )
  }

  getAllEncounters(){
    this.encountersService.getAllEncounters().subscribe(
      ((encounters:Array<Encounter>) => {
        this.encounters = encounters
        this.getTeams();
      }),
      ((error:any) => this.handleError(error))
    )
  }

  getTeams(){
    this.teamsService.getAllTeams().subscribe(
      ((teams:Array<Team>) =>{
        this.encounters.forEach(encounter => {
          encounter.teamIds.forEach(id => {
            if(!encounter.teams)
              encounter.teams = new Array<Team>();
            encounter.teams.push(teams.find(t => t.id == id));
          });
        });
        this.dataRetrieved = true;
        this.isLoading = false;
      }),
      ((error:any) => this.handleError(error))
    );
  }
  
  getEncountersOfSport(sportName:string){
    this.encountersService.getAllEncountersOfSport(sportName).subscribe(
      ((encounters:Array<Encounter>) => {
        this.setEncountersBySport(sportName, encounters);
        this.getTeamsNames(sportName);
      }),
      ((error:any) => this.handleError(error)),
    )
  }

  private getTeamsNames(sportName:string){
    this.sportService.getTeams(sportName).subscribe(
      ((teams:Array<Team>) => {
        this.teams = teams;
        this.assignNamesToEncounters(sportName);
      }),
      ((error:any) => {console.log(error)}),
    )
  }

  assignNamesToEncounters(sportName:string): void {
    this.encountersBySport[sportName].forEach(encounter => {
      encounter.teams = new Array<Team>();
      encounter.teamIds.forEach(id => {
        encounter.teams.push(this.teams.find(t => t.id == id));
      });
    });
    this.dataRetrieved = true;
    this.isLoading = false;
  }
   

  private setEncountersBySport(sportName:string, encounters:Array<Encounter>){
    this.encountersBySport[sportName]= new Array<Encounter>();
    encounters.forEach(encounter => {
      this.encountersBySport[sportName].push(Encounter.getClone(encounter));
    });
  }

  /*filterByMonthAndYear(encounters:Array<Encounter>, date: Date):Array<Encounter>{
   var result = [];
    if(encounters){
      encounters.map(e => {e.date = new Date(e.date)});
      result = encounters.filter(e => e.date.getMonth() == date.getMonth() && e.date.getFullYear() == date.getFullYear());
    }   
    return  result;
  }*/

  handleError(error:ErrorResponse){
    this.isLoading=false;
    this.errorMessage = error.errorMessage;
  }

  descOrder = (a, b) => {
    if(a.key < b.key) return b.key;
  }

  getEncounters(sportName:string):Array<Encounter>{
    return this.encounters.filter(e => e.sportName == sportName);
  }



  chosenMonthHandler(value:Date, datepicker: MatDatepicker<Date>) {
    this.actualMonth = value;
    datepicker.close();
    this.dateControl.setValue(this.actualMonth.getDate()+"/"+this.actualMonth.getMonth());
  }
}

import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import {MatPaginator, MatSort, MatTableDataSource, MatDialogRef, MAT_DIALOG_DATA, MatDialog} from '@angular/material';
import { Encounter } from 'src/app/classes/encounter';
import { Globals } from 'src/app/globals';
import { EncountersService } from 'src/app/services/encounters/encounters.service';
import { ConfirmationDialogComponent, DialogInfo } from '../confirmation-dialog/confirmation-dialog';
import { EncounterDialogComponent } from './encounter-dialog/encounter-dialog.component';
import { ErrorResponse } from 'src/app/classes/error';
import { ReConnector } from 'src/app/services/auth/reconnector';
import { ErrorObserver } from 'rxjs';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { Token } from 'src/app/classes/token';
import { AuthService } from 'src/app/services/auth/auth.service';
import { TeamsService } from 'src/app/services/teams/teams.service';
import { Team } from 'src/app/classes/team';
import { EncounterResultDialogComponent } from './encounter-result-dialog/encounter-result-dialog.component';
import { FormControl } from '@angular/forms';
import { SportsService } from 'src/app/services/sports/sports.service';
import { Sport } from 'src/app/classes/sport';

@Component({
  selector: 'encounters-list',
  templateUrl: './encounters.component.html',
  styleUrls: ['./encounters.component.css']
})
export class EncountersComponent implements OnInit {
  displayedColumns: string[] = ['id', 'sportName', 'teamIds', 'date', 'result', 'options'];
  dataSource:MatTableDataSource<Encounter>;
  @ViewChild(MatPaginator) paginator:MatPaginator;
  @ViewChild(MatSort) sort:MatSort;
  errorMessage: string;
  encounterEdited: Encounter;
  rowEdited: Encounter;
  isLoading = false;
  teams:Array<Team>;
  encounters:Array<Encounter>;
  teamFilter:FormControl;
  sportFilter:FormControl;
  sports: Array<Sport>;

  constructor(private sportsService:SportsService, private teamsService:TeamsService, private router: Router, private domSanitizer: DomSanitizer, private auth:AuthService ,private dialog:MatDialog, private encountersService:EncountersService) {
    this.teamFilter = new FormControl();
    this.sportFilter = new FormControl();
    this.getEncounters();
  }
  
  ngOnInit() {

  }

  public getEncounters():void{
    this.isLoading = true;
    this.encountersService.getAllEncounters().subscribe(
      ((data:Array<Encounter>) => {
        this.encounters = data;
        this.getTeamsNames();
        this.getSports();
      }),
      ((error:any) => this.handleEncounterError(error))
    )
  }

  private getTeamsNames(){
    this.teamsService.getAllTeams().subscribe(
      ((teams:Array<Team>) => this.teams = teams),
      ((error:any) => {console.log(error)}),
      (()=>this.assignNamesToEncounters())
    )
  }

  private getSports(){
    this.sportsService.getAllSports().subscribe(
      ((sports:Array<Sport>) => this.sports = sports),
      ((error:any) => {console.log(error)})
    )
  }

  assignNamesToEncounters(): void {
    this.encounters.forEach(encounter => {
      encounter.teams = new Array<Team>();
      encounter.teamIds.forEach(id => {
        encounter.teams.push(this.teams.find(t => t.id == id));
      });
    });
    this.updateTableData(this.encounters);
  }

  private updateTableData(encounters:Array<Encounter>){
    this.dataSource = new MatTableDataSource(encounters);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.isLoading = false;
  }

  sanitizeImage(base64Image:string):string{
    var path = 'data:image/jpg;base64,' + atob(base64Image)
    this.domSanitizer.bypassSecurityTrustUrl(path);
    return path
  }

  public getArrayOfNames(id:number):Array<string>{
    var encounter = this.encounters.find(t => t.id == id);
    return encounter.teams?Encounter.teamNames(encounter):[];
  }

  private handleEncounterError(error:ErrorResponse) {
    if(error.errorCode == 0 || error.errorCode == 401){
      this.auth.authenticate(Globals.getUsername(), Globals.getPassword()).subscribe(
        ((token:Token)=>Globals.setToken(token.token)),
        ((error:any) => this.router.navigate['login']),
        (()=> {
          this.isLoading = false;
          this.getEncounters();
        })
      )
    }
  }
  
  applyFilter(filterValue:string){
    this.dataSource.filter = filterValue.trim().toLowerCase();
    if(this.dataSource.paginator){
      this.dataSource.paginator.firstPage();
    }
  }

  applyTeamFilter(filterValue:string){
    this.noFilter();
    this.sportFilter.setValue("0");
    this.sportFilter.updateValueAndValidity();
    this.dataSource.filterPredicate = ((t,filter) => t.teams.some(t => t.id == Number.parseInt(filter)));
    this.dataSource.filter = filterValue;
    if(this.dataSource.paginator){
      this.dataSource.paginator.firstPage();
    }
  }

  applySportFilter(filterValue:string){
    this.noFilter();
    this.teamFilter.setValue("0");
    this.teamFilter.updateValueAndValidity();
    this.dataSource.filterPredicate = ((t,filter) => t.sportName == filter);
    this.dataSource.filter = filterValue;
    if(this.dataSource.paginator){
      this.dataSource.paginator.firstPage();
    }
  }

  noFilter(){
    this.dataSource.filter = "";
    if(this.dataSource.paginator){
      this.dataSource.paginator.firstPage();
    }
  }

  openEditDialog(aEncounter:Encounter):void{
    this.encounterEdited = Encounter.getClone(aEncounter);
    this.rowEdited = aEncounter;
    const dialogRef = this.dialog.open(EncounterDialogComponent, {
      width:'500px',
      data: {
        aEncounter: this.encounterEdited,
        title: "Edit encounter",
        isNewEncounter: false
      }
    });
    dialogRef.afterClosed().subscribe(
      ((result:Encounter) => {
        if(result!=undefined){
          this.rowEdited.sportName = result.sportName;
          this.rowEdited.id = result.id;
          this.rowEdited.date = result.date;
          this.rowEdited.sportName = result.sportName;
        }       
      })
    )
  }

  openDeleteDialog(aEncounter:Encounter):void{
    var confirmation:Boolean;
    confirmation = false;
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      width:'500px',
      data: {
        confirmation: confirmation,
        title: "Delete " + aEncounter.id,
        message: "This operation needs confirmation. It can not be undone."
      }
    });
    dialogRef.afterClosed().subscribe(
      ((dialgoResponse:DialogInfo) => {
        if(dialgoResponse.confirmation)
          this.performDelete(aEncounter);
      })
    )
  }

  performDelete(aEncounter: Encounter): void {
    this.encountersService.deleteEncounter(aEncounter.id).subscribe(
      ((result:any) => this.updateTableData(this.dataSource.data.filter((t:Encounter)=>t.id != aEncounter.id))),
      ((error:any) => this.handleDeleteError(error, aEncounter))
    );
  }

  handleDeleteError(error: ErrorResponse, aEncounter:Encounter): void {
    if(error.errorCode == 0 || error.errorCode == 401){
      this.auth.authenticate(Globals.getUsername(), Globals.getPassword()).subscribe(
        ((token:Token)=>Globals.setToken(token.token)),
        ((error:any) => this.router.navigate['login']),
        (()=> {
          this.isLoading = false;
          this.performDelete(aEncounter);
        })
      )
    }
  }

  openAddDialog():void{
    var encounter = new Encounter("", false, new Date(Date.now()));
    const dialogRef = this.dialog.open(EncounterDialogComponent, {
      width:'500px',
      data: {
        aEncounter: encounter,
        title: "Add new encounter",
        isNewEncounter: true
      }
    });
    dialogRef.afterClosed().subscribe(
      (() => {
        this.getEncounters();
      })
    )
  }

  openEditResultDialog(aEncounter:Encounter):void{
    this.encounterEdited = Encounter.getClone(this.encounters.find(e => e.id == aEncounter.id));
    this.rowEdited = aEncounter;
    const dialogRef = this.dialog.open(EncounterResultDialogComponent, {
      width:'500px',
      data: {
        encounterId:this.encounterEdited.id,
        teams:this.encounterEdited.teams,
        isNewResult: !this.encounterEdited.hasResult,
        isTwoTeams: this.encounterEdited.teams.length == 2,
        title: "Set result"
      }
    });
    dialogRef.afterClosed().subscribe(
      ((result:boolean) => {
        if(result!=undefined){
          this.getEncounters();
        }       
      })
    )
  }

  performAdd(newEncounter:Encounter):void{
    this.getEncounters();
  }

  resultOf(encounter:Encounter):string{
    var result = "";
    if(encounter.hasResult){
      if(encounter.winnerId && encounter.winnerId != 0)
        result = "Winner: " + this.teams.find(t => t.id == encounter.winnerId).name;
      if(encounter.team_Position && encounter.team_Position.length != 0){
        encounter.team_Position.forEach(standing => {
          result += this.teams.find(t => t.id == standing.teamId).name + " - " + standing.points + ", ";
        });
      };
    }
    else
      result = "No result yet";
    return result;
  }
}



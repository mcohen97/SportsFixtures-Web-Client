import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import {MatPaginator, MatSort, MatTableDataSource, MatDialogRef, MAT_DIALOG_DATA, MatDialog} from '@angular/material';
import { Sport } from 'src/app/classes/sport';
import { Globals } from 'src/app/globals';
import { SportsService } from 'src/app/services/sports/sports.service';
import { ConfirmationDialogComponent, DialogInfo } from '../confirmation-dialog/confirmation-dialog';
import { ErrorResponse } from 'src/app/classes/error';
import { ReConnector } from 'src/app/services/auth/reconnector';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth/auth.service';
import { Token } from 'src/app/classes/token';
import { FormControl } from '@angular/forms';
import { Team } from 'src/app/classes/team';
import { TablePosition } from 'src/app/classes/table-position';

@Component({
  selector: 'app-sport-table',
  templateUrl: './sport-table.component.html',
  styleUrls: ['./sport-table.component.css']
})
export class SportTableComponent implements OnInit {

  displayedColumns: string[] = ['photo', 'team', 'points'];
  dataSource:MatTableDataSource<Team>;
  @ViewChild(MatPaginator) paginator:MatPaginator;
  @ViewChild(MatSort) sort:MatSort;
  errorMessage: string;
  isLoading = false;
  sportSelect:FormControl;
  sports:Array<Sport>;
  selectedSportTeams: Array<Team>;

  constructor(private router:Router, private auth:AuthService, private reconnector:ReConnector ,private dialog:MatDialog, private sportsService:SportsService) {
    this.isLoading = true;
    this.sportSelect = new FormControl();
    this.getSports();
  }

  ngOnInit() {
  }

  public getSports():void{
    this.isLoading = true;
    this.sportsService.getAllSports().subscribe(
      ((data:Array<Sport>) => this.sports = data),
      ((error:ErrorResponse) => this.handleSportError(error)),
      (() => this.isLoading = false)
    )
  }

  showSelectedSport(sportname:string){
    this.isLoading = true;
    this.sportsService.getTeams(sportname).subscribe(
      ((teams:Array<Team>) => {
        this.selectedSportTeams = teams;
        this.getSportTable(sportname);
      }),
      ((error:any) => this.handleError(error)),
      (() => this.isLoading = false)
    )
  }

  getSportTable(sportname:string){
    this.isLoading = true;
    this.sportsService.getSportTable(sportname).subscribe(
      ((points:Array<TablePosition>) => this.assignPointsToTeams(points)),
      ((error:any) => this.handleError(error)),
      (() => this.isLoading = false)
    )
  }

  assignPointsToTeams(points: TablePosition[]): void {
    this.isLoading = true;
    this.selectedSportTeams.forEach(team => {
      team.points = points.find(p => p.teamId == team.id).points;
    });
    this.updateTableData(this.selectedSportTeams);
  }

  private updateTableData(teams:Array<Team>){
    this.dataSource = new MatTableDataSource(teams);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.isLoading = false;
  }

  private handleSportError(error:ErrorResponse) {
    if(error.errorCode == 0 || error.errorCode == 401){
      this.auth.authenticate(Globals.getUsername(), Globals.getPassword()).subscribe(
        ((token:Token)=>Globals.setToken(token.token)),
        ((error:any) => this.router.navigate['login']),
        (()=> {
          this.isLoading = false;
          this.getSports();
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


  handleError(error: ErrorResponse): void {
    if(error.errorCode == 0 || error.errorCode == 401){
      this.auth.authenticate(Globals.getUsername(), Globals.getPassword()).subscribe(
        ((token:Token)=>Globals.setToken(token.token)),
        ((error:any) => this.router.navigate['login']),
        (()=> {
          this.isLoading = false;
        })
      )
    }
  }

}
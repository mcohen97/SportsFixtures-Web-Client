import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import {MatPaginator, MatSort, MatTableDataSource, MatDialogRef, MAT_DIALOG_DATA, MatDialog} from '@angular/material';
import { Team } from 'src/app/classes/team';
import { Globals } from 'src/app/globals';
import { TeamsService } from 'src/app/services/teams/teams.service';
import { ConfirmationDialogComponent, DialogInfo } from '../confirmation-dialog/confirmation-dialog';
import { TeamDialogComponent } from './team-dialog/team-dialog.component';
import { ErrorResponse } from 'src/app/classes/error';
import { ReConnector } from 'src/app/services/auth/reconnector';
import { ErrorObserver } from 'rxjs';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { Router } from '@angular/router';

@Component({
  selector: 'teams-list',
  templateUrl: './teams.component.html',
  styleUrls: ['./teams.component.css']
})
export class TeamsComponent implements OnInit {
  displayedColumns: string[] = ['id', 'name', 'sportName', 'photo','options'];
  dataSource:MatTableDataSource<Team>;
  @ViewChild(MatPaginator) paginator:MatPaginator;
  @ViewChild(MatSort) sort:MatSort;
  errorMessage: string;
  teamEdited: Team;
  rowEdited: Team;
  isLoading = false;

  constructor(private router: Router, private domSanitizer: DomSanitizer, private reconnector:ReConnector ,private dialog:MatDialog, private teamsService:TeamsService) {
    this.getTeams();
  }
  
  ngOnInit() {

  }

  public getTeams():void{
    this.isLoading = true;
    this.teamsService.getAllTeams().subscribe(
      ((data:Array<Team>) => this.updateTableData(data)),
      ((error:any) => this.handleTeamError(error))
    )
  }

  private updateTableData(teams:Array<Team>){
    this.dataSource = new MatTableDataSource(teams);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.isLoading = false;
  }

  sanitizeImage(base64Image:string):string{
    var path = 'data:image/jpg;base64,' + atob(base64Image)
    this.domSanitizer.bypassSecurityTrustUrl(path);
    return path
  }

  private handleTeamError(error:ErrorResponse) {
    console.log(error);
    if(error.errorCode == 0 || error.errorCode == 401){
      var resultByRef = {result: false};
      this.reconnector.tryReconnect(resultByRef);
      while(!resultByRef.result && this.reconnector.tryCount <= 20){
        //wait
      }
      if(resultByRef.result)
        this.getTeams();
      else
        this.router.navigate(['login']);
    }
    this.isLoading = false;
  
  }
  
  applyFilter(filterValue:string){
    this.dataSource.filter = filterValue.trim().toLowerCase();
    if(this.dataSource.paginator){
      this.dataSource.paginator.firstPage();
    }
  }

  openEditDialog(aTeam:Team):void{
    this.teamEdited = Team.getClone(aTeam);
    this.rowEdited = aTeam;
    const dialogRef = this.dialog.open(TeamDialogComponent, {
      width:'500px',
      data: {
        aTeam: this.teamEdited,
        title: "Edit team",
        isNewTeam: false
      }
    });
    dialogRef.afterClosed().subscribe(
      ((result:Team) => {
        if(result!=undefined){
          this.rowEdited.name = result.name;
          this.rowEdited.id = result.id;
          this.rowEdited.photo = result.photo;
          this.rowEdited.sportName = result.sportName;
        }       
      })
    )
  }

  openDeleteDialog(aTeam:Team):void{
    var confirmation:Boolean;
    confirmation = false;
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      width:'500px',
      data: {
        confirmation: confirmation,
        title: "Delete " + aTeam.id,
        message: "This operation needs confirmation. It can not be undone."
      }
    });
    dialogRef.afterClosed().subscribe(
      ((dialgoResponse:DialogInfo) => {
        if(dialgoResponse.confirmation)
          this.performDelete(aTeam);
      })
    )
  }
  performDelete(aTeam: Team): void {
    this.teamsService.deleteTeam(aTeam.id).subscribe(
      ((result:any) => this.updateTableData(this.dataSource.data.filter((t:Team)=>t.id != aTeam.id))),
      ((error:any) => this.handleDeleteError(error, aTeam))
    );
  }
  handleDeleteError(error: ErrorResponse, aTeam:Team): void {
    console.log(error);
    if(error.errorCode == 0 || error.errorCode == 401){
      var resultByRef = {result: false};
      this.reconnector.tryReconnect(resultByRef);
      while(!resultByRef.result && this.reconnector.tryCount <= 20){
        //wait
      }
      if(resultByRef.result)
        this.performDelete(aTeam);
      else
        this.router.navigate(['login']);
    }
    this.isLoading = false;
  
  }

  openAddDialog():void{
    var team = new Team("","");
    const dialogRef = this.dialog.open(TeamDialogComponent, {
      width:'500px',
      data: {
        aTeam: team,
        title: "Add new team",
        isNewTeam: true
      }
    });
    dialogRef.afterClosed().subscribe(
      ((newTeam:Team) => {
        if(newTeam != undefined)
          this.performAdd(newTeam);
      })
    )
  }

  performAdd(newTeam:Team):void{
    this.dataSource.data.push(newTeam);
    this.dataSource._updateChangeSubscription();
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }
}



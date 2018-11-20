import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import {MatPaginator, MatSort, MatTableDataSource, MatDialogRef, MAT_DIALOG_DATA, MatDialog} from '@angular/material';
import { Team } from 'src/app/classes/team';
import { Globals } from 'src/app/globals';
import { TeamsService } from 'src/app/services/teams/teams.service';
import { ConfirmationDialogComponent, DialogInfo } from '../confirmation-dialog/confirmation-dialog';
import { FollowTeamDialogComponent } from './follow-team-dialog/follow-team-dialog.component';
import { ErrorResponse } from 'src/app/classes/error';
import { ReConnector } from 'src/app/services/auth/reconnector';
import { ErrorObserver } from 'rxjs';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { Token } from 'src/app/classes/token';
import { OkMessage } from 'src/app/classes/okMessage';
import { UsersService } from 'src/app/services/users/users.service';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'teams-follower-list',
  templateUrl: './teams-follower.component.html',
  styleUrls: ['./teams-follower.component.css']
})
export class TeamsFollowerComponent implements OnInit {
  displayedColumns: string[] = ['id', 'name', 'sportName', 'photo', 'options'];
  dataSource:MatTableDataSource<Team>;
  @ViewChild(MatPaginator) paginator:MatPaginator;
  @ViewChild(MatSort) sort:MatSort;
  errorMessage: string;
  teamEdited: Team;
  rowEdited: Team;
  isLoading = false;
  teamsFollowed:Array<Team>;
  teams:Array<Team>;

  constructor(
    private auth:AuthService,
    private router: Router, 
    private domSanitizer: DomSanitizer, 
    private dialog:MatDialog,
    private usersService:UsersService ,
    private teamsService:TeamsService
  ){
    this.teamsFollowed = new Array<Team>(); 
    this.getFollowedTeams();
  }
  
  ngOnInit() {

  }

  public getFollowedTeams():void{
    this.isLoading = true;
    this.usersService.getFollowedTeams(Globals.getUsername()).subscribe(
      ((data:Array<Team>) => {
        this.teamsFollowed = data;
        this.getTeams();
      }),
      ((error:any) => this.handleTeamError(error))
    )
  }

  public getTeams():void{
    this.isLoading = true;
    this.teamsService.getAllTeams().subscribe(
      ((teams:Array<Team>) => {
        this.teams = teams;
        this.setFollowed();
        this.updateTableData(this.teams);
      })
    )
  }

  private setFollowed():void{
    this.teams.forEach(team => {
      team.followed = this.teamsFollowed.some(t => t.id == team.id);
    });
  }

  private updateTableData(teams:Array<Team>){
    this.dataSource = new MatTableDataSource(teams);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.isLoading = false;
  }

  private handleTeamError(error:ErrorResponse) {
    if(error.errorCode == 0 || error.errorCode == 401){
      this.auth.authenticate(Globals.getUsername(), Globals.getPassword()).subscribe(
        ((token:Token)=>Globals.setToken(token.token)),
        ((error:any) => this.router.navigate['login']),
        (()=> {
          this.isLoading = false;
          this.getTeams();
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

  
  openDeleteDialog(aTeam:Team):void{
    var confirmation:Boolean;
    confirmation = false;
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      width:'500px',
      data: {
        confirmation: confirmation,
        title: "Delete " + aTeam.name + " (" + aTeam.sportName + ")",
        message: "This operation needs confirmation."
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
    this.usersService.unfollowTeam(aTeam.id).subscribe(
      ((result:any) => this.updateTableData(this.dataSource.data.filter((t:Team)=>t.id != aTeam.id))),
      ((error:any) => this.handleDeleteError(error, aTeam))
    );
  }

  handleDeleteError(error: ErrorResponse, aTeam:Team): void {
    if(error.errorCode == 0 || error.errorCode == 401){
      this.auth.authenticate(Globals.getUsername(), Globals.getPassword()).subscribe(
        ((token:Token)=>Globals.setToken(token.token)),
        ((error:any) => this.router.navigate['login']),
        (()=> {
          this.isLoading = false;
          this.performDelete(aTeam);
        })
      )
    }
  }

  openFollowDialog():void{
    const dialogRef = this.dialog.open(FollowTeamDialogComponent, {
      width:'500px',
    });
    dialogRef.afterClosed().subscribe(
      ((followed:OkMessage) => {
        if(followed != undefined)
          console.log(followed.okMessage)
          this.getTeams();
      })
    )
  }

  followTeam(team:Team):void{
    this.usersService.followTeam(team.id).subscribe(
      ((ok:OkMessage) => this.updateFollowButton(team)),
      ((error:any) => this.handleTeamError(error))
    )
  }

  unfollowTeam(team:Team):void{
    this.usersService.unfollowTeam(team.id).subscribe(
      ((ok:OkMessage) => this.updateFollowButton(team)),
      ((error:any) => this.handleTeamError(error))
    )
  }

  updateFollowButton(team:Team){
    team.followed = !team.followed;
  }

}


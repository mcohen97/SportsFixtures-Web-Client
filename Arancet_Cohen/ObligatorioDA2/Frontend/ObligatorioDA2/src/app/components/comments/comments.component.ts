import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import {MatPaginator, MatSort, MatTableDataSource, MatDialogRef, MAT_DIALOG_DATA, MatDialog, ShowOnDirtyErrorStateMatcher} from '@angular/material';
import { Comment } from 'src/app/classes/comment';
import { Globals } from 'src/app/globals';
import { CommentsService } from 'src/app/services/comments/comments.service';
import { ConfirmationDialogComponent, DialogInfo } from '../confirmation-dialog/confirmation-dialog';
import { CommentDialogComponent } from './comment-dialog/comment-dialog.component';
import { ErrorResponse } from 'src/app/classes/error';
import { ReConnector } from 'src/app/services/auth/reconnector';
import { ErrorObserver } from 'rxjs';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { Token } from 'src/app/classes/token';
import { UsersService } from 'src/app/services/users/users.service';
import { Team } from 'src/app/classes/team';
import { AuthService } from 'src/app/services/auth/auth.service';
import { FormControl } from '@angular/forms';
import { EncountersService } from 'src/app/services/encounters/encounters.service';
import { TeamsService } from 'src/app/services/teams/teams.service';
import { Encounter } from 'src/app/classes/encounter';

@Component({
  selector: 'comments-list',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.css']
})
export class CommentsComponent implements OnInit {
  displayedColumns: string[] = ['makerUsername', 'text'];
  dataSource:MatTableDataSource<Comment>;
  @ViewChild(MatPaginator) paginator:MatPaginator;
  @ViewChild(MatSort) sort:MatSort;
  errorMessage: string;
  isLoading = false;
  teamsFollowed : Array<Team>;
  encountersShown: Array<Encounter>;
  selectedTeamId:number;
  encountersByTeamId = [];
  commentsByEncounterId = [];
  teamFilter: FormControl;

  constructor(private teamsService:TeamsService, private encountersService:EncountersService, private auth:AuthService, private usersService: UsersService, private router: Router, private domSanitizer: DomSanitizer, private reconnector:ReConnector ,private dialog:MatDialog, private commentsService:CommentsService) {
    this.isLoading = true;
    this.getFollowedTeams();
  }
  
  ngOnInit() {

  }

  public getFollowedTeams(){
    this.isLoading = true;
    var username = Globals.getUsername();
    this.usersService.getFollowedTeams(username).subscribe(
      ((teams:Array<Team>) => this.loadTeams(teams)),
      ((error:any) => this.handleError(error)),
      (() => this.isLoading = false)  
    )
  }

  loadTeams(teams: Array<Team>): void {
    this.teamsFollowed = teams;
  }

  loadComments(comments: Comment[], encounterId: number): void {
    this.commentsByEncounterId[encounterId] = comments;
  }

  handleError(error: ErrorResponse): void {
    if(error.errorCode == 0 || error.errorCode == 401){
      this.auth.authenticate(Globals.getUsername(), Globals.getPassword()).subscribe(
        ((token:Token)=>Globals.setToken(token.token)),
        ((error:any) => this.router.navigate['login']),
        (()=> {
          this.isLoading = false;
          this.errorMessage = "Cant get data due to authentication problems";
        })
      )
    }else{
      this.errorMessage = "There was an error, refresh the page and try again"
      this.isLoading = false;
    }
  }

  openAddDialog():void{
    const dialogRef = this.dialog.open(CommentDialogComponent, {
      width:'500px',
      data: {
        title: "Add new comment",
        isNewComment: true
      }
    });
    dialogRef.afterClosed().subscribe(
      ((newComment:Comment) => {
        if(newComment != undefined)
          this.performAdd(newComment);
      })
    )
  }

  performAdd(newComment:Comment):void{
    this.dataSource.data.push(newComment);
    this.dataSource._updateChangeSubscription();
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  showEncounters(teamId:number){
    if(teamId != 0){
      this.selectedTeamId = teamId;
      this.getEncountersOfTeam(teamId);
    }

  }

  getEncountersOfTeam(teamId:number){
    this.isLoading = true;
    this.encountersService.getEncountersOfTeam(teamId).subscribe(
      ((encounters:Array<Encounter>) => this.encountersShown = encounters),
      ((error:any) => this.handleError(error)),
      (() => this.getCommentsOfEncounters(this.encountersShown))
    )
  }

  getCommentsOfEncounters(encounters:Array<Encounter>){
    encounters.forEach((encounter:Encounter) =>{
      this.commentsService.getEncounterComments(encounter.id).subscribe(
        ((comments:Array<Comment>) => this.loadComments(comments, encounter.id)),
        ((error:any) => this.handleError(error)),
        (() => this.isLoading = false)
      )
    })
  }

  cleanDate(encounter:Encounter):string{
    var date = new Date(encounter.date);
    return date.getDate()+"/"+(date.getMonth()+1)+"/"+date.getFullYear();
  }

}


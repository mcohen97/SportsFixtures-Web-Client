import { Component, OnInit, ViewChild, Inject, Input, OnChanges } from '@angular/core';
import {MatPaginator, MatSort, MatTableDataSource, MatDialogRef, MAT_DIALOG_DATA, MatDialog} from '@angular/material';
import { Comment } from 'src/app/classes/comment';
import { Globals } from 'src/app/globals';
import { SportsService } from 'src/app/services/sports/sports.service';
import { ErrorResponse } from 'src/app/classes/error';
import { ReConnector } from 'src/app/services/auth/reconnector';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth/auth.service';
import { Token } from 'src/app/classes/token';
import { CommentDialogComponent } from '../comment-dialog/comment-dialog.component';
import { Encounter } from 'src/app/classes/encounter';
import { CommentsService } from 'src/app/services/comments/comments.service';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'comments-table',
  templateUrl: './comments-table.component.html',
  styleUrls: ['./comments-table.component.css']
})
export class CommentsTableComponent implements OnInit, OnChanges{
 
  constructor( private router:Router, private auth:AuthService, private dialog:MatDialog, private commentsService:CommentsService) { 
    if(this.encounter){
      this.commentControl = new FormControl();
      this.getComments(this.encounter);
      var ref = setInterval(()=> {
        this.getComments(this.encounter); },3000); 
      Globals.addInterval(ref);
    }
  }

  ngOnInit() {
    if(this.encounter){
      this.commentControl = new FormControl();
      this.getComments(this.encounter);
      var ref = setInterval(()=> {
        this.getComments(this.encounter); },3000); 
      Globals.addInterval(ref);
    }
  }

  ngOnChanges() {
    if(this.encounter){
      this.commentControl = new FormControl();
      this.getComments(this.encounter);
      var ref = setInterval(()=> {
        this.getComments(this.encounter); },3000); 
      Globals.addInterval(ref); 
    }
  }


  displayedColumns: string[] = ['makerUsername', 'text'];
  dataSource:MatTableDataSource<Comment>;
  @ViewChild(MatSort) sort:MatSort;
  @Input() encounter:Encounter;
  commentControl:FormControl;

  getComments(encounter:Encounter){
    this.commentsService.getEncounterComments(encounter.id).subscribe(
      ((comments:Array<Comment>) => this.updateTableData(comments)),
      ((error:any) => this.handleError(error))
    )
  }

  private handleError(error:ErrorResponse) {
    if(error.errorCode == 0 || error.errorCode == 401){
      this.auth.authenticate(Globals.getUsername(), Globals.getPassword()).subscribe(
        ((token:Token)=>Globals.setToken(token.token)),
        ((error:any) => this.router.navigate['login']),
        (()=> {
          this.getComments(this.encounter);
        })
      )
    }
  }

  private updateTableData(comments:Array<Comment>){
    if(!this.dataSource){
      this.dataSource = new MatTableDataSource(comments);
      this.dataSource.sort = this.sort;
    }else{
      this.dataSource.data = comments;
    }
  }

  applyFilter(filterValue:string){
    this.dataSource.filter = filterValue.trim().toLowerCase();
    if(this.dataSource.paginator){
      this.dataSource.paginator.firstPage();
    }
  }

  openAddDialog():void{
    const dialogRef = this.dialog.open(CommentDialogComponent, {
      width:'500px',
      data: {
        title: "Comment",
        encounterId: this.encounter.id
      }
    });
    dialogRef.afterClosed().subscribe(
      ((newComment:Comment) => {
        if(newComment != undefined)
          this.performAdd(newComment);
      })
    )
  }

  comment(){
    var text = this.commentControl.value;
    if(text && text != ""){
      var newComment = new Comment(text);
      newComment.makerUsername = Globals.getUsername();
      this.addComment(newComment, this.encounter.id);
      this.commentControl.setValue("");
      this.commentControl.updateValueAndValidity();
    }
      
  }


  performAdd(newComment:Comment):void{
    this.dataSource.data.push(newComment);
    this.dataSource._updateChangeSubscription();
    this.dataSource.sort = this.sort;
  }

  addComment(newComment:Comment, matchId:number):void{
    this.commentsService.addComment(newComment, matchId).subscribe(
      ((result:Comment) => {
        this.getComments(this.encounter);
      }),
      ((error:ErrorResponse) => this.handleError(error))
    );
  }
}

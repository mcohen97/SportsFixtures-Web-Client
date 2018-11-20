import { Component, OnInit, ViewChild, Inject, Input } from '@angular/core';
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

@Component({
  selector: 'comments-table',
  templateUrl: './comments-table.component.html',
  styleUrls: ['./comments-table.component.css']
})
export class CommentsTableComponent implements OnInit {

  constructor( private router:Router, private auth:AuthService, private dialog:MatDialog, private commentsService:CommentsService) { 

  }

  ngOnInit() {
    this.getComments(this.encounter);
  }

  displayedColumns: string[] = ['makerUsername', 'text'];
  dataSource:MatTableDataSource<Comment>;
  @ViewChild(MatPaginator) paginator:MatPaginator;
  @ViewChild(MatSort) sort:MatSort;
  @Input() encounter:Encounter;

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
    this.dataSource = new MatTableDataSource(comments);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
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

  performAdd(newComment:Comment):void{
    this.dataSource.data.push(newComment);
    this.dataSource._updateChangeSubscription();
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

}

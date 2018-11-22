import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import {MatPaginator, MatSort, MatTableDataSource, MatDialogRef, MAT_DIALOG_DATA, MatDialog} from '@angular/material';
import { Sport } from 'src/app/classes/sport';
import { Globals } from 'src/app/globals';
import { SportsService } from 'src/app/services/sports/sports.service';
import { ConfirmationDialogComponent, DialogInfo } from '../confirmation-dialog/confirmation-dialog';
import { SportDialogComponent } from './sport-dialog/sport-dialog.component';
import { ErrorResponse } from 'src/app/classes/error';
import { ReConnector } from 'src/app/services/auth/reconnector';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth/auth.service';
import { Token } from 'src/app/classes/token';

@Component({
  selector: 'app-sports',
  templateUrl: './sports.component.html',
  styleUrls: ['./sports.component.css']
})
export class SportsComponent implements OnInit {

  displayedColumns: string[] = ['name', 'isTwoTeams', 'options'];
  dataSource:MatTableDataSource<Sport>;
  @ViewChild(MatPaginator) paginator:MatPaginator;
  @ViewChild(MatSort) sort:MatSort;
  errorMessage: string;
  sportEdited: Sport;
  rowEdited: Sport;
  isLoading = false;

  constructor(private router:Router, private auth:AuthService, private reconnector:ReConnector ,private dialog:MatDialog, private sportsService:SportsService) {
    this.isLoading = true;
    this.getSports();
    var ref = setInterval(()=> {
      this.getSports(); },3000);
    Globals.addInterval(ref);
  }

  ngOnInit() {
  }

  public getSports():void{
    this.sportsService.getAllSports().subscribe(
      ((data:Array<Sport>) => this.updateTableData(data)),
      ((error:ErrorResponse) => this.handleSportError(error))
    )
  }

  private updateTableData(sports:Array<Sport>){
    if(!this.dataSource){
      this.dataSource = new MatTableDataSource(sports);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
      this.isLoading = false;
    }
    else{
      this.dataSource.data = sports;
    }
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

  openDeleteDialog(aSport:Sport):void{
    var confirmation:Boolean;
    confirmation = false;
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      width:'500px',
      data: {
        confirmation: confirmation,
        title: "Delete " + aSport.name,
        message: "This operation needs confirmation. All data asociated with this sport will be lost. It can not be undone."
      }
    });
    dialogRef.afterClosed().subscribe(
      ((dialgoResponse:DialogInfo) => {
        if(dialgoResponse.confirmation)
          this.performDelete(aSport);
      })
    )
  }

  performDelete(aSport: Sport): void {
    this.sportsService.deleteSport(aSport.name).subscribe(
      ((result:any) => this.updateTableData(this.dataSource.data.filter((s:Sport)=>s.name != aSport.name))),
      ((error:any) => this.handleDeleteError(error, aSport))
    );
  }

  handleDeleteError(error: ErrorResponse, aSport:Sport): void {
    if(error.errorCode == 0 || error.errorCode == 401){
      this.auth.authenticate(Globals.getUsername(), Globals.getPassword()).subscribe(
        ((token:Token)=>Globals.setToken(token.token)),
        ((error:any) => this.router.navigate['login']),
        (()=> {
          this.isLoading = false;
          this.performDelete(aSport);
        })
      )
    }
  }

  openAddDialog():void{
    var sport = new Sport("");
    const dialogRef = this.dialog.open(SportDialogComponent, {
      width:'500px',
      data: {
        aSport: sport,
        title: "Add new sport",
        isNew: true
      }
    });
    dialogRef.afterClosed().subscribe(
      ((newSport:Sport) => {
        if(newSport != undefined)
          this.performAdd(newSport);
      })
    )
  }

  performAdd(newSport:Sport):void{
    this.dataSource.data.push(newSport);
    this.dataSource._updateChangeSubscription();
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }
}

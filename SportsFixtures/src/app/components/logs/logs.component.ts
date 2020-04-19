import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import {MatPaginator, MatSort, MatTableDataSource, MatDialogRef, MAT_DIALOG_DATA, MatDialog} from '@angular/material';
import { Log } from 'src/app/classes/log';
import { Globals } from 'src/app/globals';
import { LogsService } from 'src/app/services/logs/logs.service';
import { ConfirmationDialogComponent, DialogInfo } from '../confirmation-dialog/confirmation-dialog';
import { ErrorResponse } from 'src/app/classes/error';
import { ReConnector } from 'src/app/services/auth/reconnector';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth/auth.service';
import { Token } from 'src/app/classes/token';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-logs',
  templateUrl: './logs.component.html',
  styleUrls: ['./logs.component.css']
})
export class LogsComponent implements OnInit {

  displayedColumns: string[] = ['id', 'logType', 'message', 'username', 'date'];
  dataSource:MatTableDataSource<Log>;
  @ViewChild(MatPaginator) paginator:MatPaginator;
  @ViewChild(MatSort) sort:MatSort;
  errorMessage: string;
  isLoading = false;
  fromDateControl:FormControl;
  toDateControl:FormControl;
  logs:Array<Log>;
  filterValue:string;

  constructor(private router:Router, private auth:AuthService, private reconnector:ReConnector ,private dialog:MatDialog, private logsService:LogsService) {
    this.fromDateControl = new FormControl();
    this.toDateControl = new FormControl();
    this.filterValue = "";
    this.isLoading = true;
    this.getLogs();
    var ref = setInterval(()=> {
      this.getLogs();
    },3000);
    Globals.addInterval(ref);
  }

  ngOnInit() {
  }

  public getLogs():void{
    this.logsService.getAllLogs().subscribe(
      ((data:Array<Log>) => {
        this.updateTableData(data)
        this.logs = data;
      }),
      ((error:ErrorResponse) => this.handleLogError(error))
    )
  }

  private updateTableData(logs:Array<Log>){
    if(!this.dataSource){
      this.dataSource = new MatTableDataSource(logs);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
      this.isLoading = false;
    }
    else
      this.dataSource.data = logs;
  }

  private handleLogError(error:ErrorResponse) {
    if(error.errorCode == 0 || error.errorCode == 401){
      this.auth.authenticate(Globals.getUsername(), Globals.getPassword()).subscribe(
        ((token:Token)=>Globals.setToken(token.token)),
        ((error:any) => this.router.navigate['login']),
        (()=> {
          this.isLoading = false;
          this.getLogs();
        })
      )
    }
  }
  
  applyFilter(filterValue:string){
    this.filterValue = filterValue;
    this.dataSource.filter = filterValue.trim().toLowerCase();
    if(this.dataSource.paginator){
      this.dataSource.paginator.firstPage();
    }
  }

  applyDateFilter(){
    var result = this.logs.filter((l:Log) => ((new Date(l.date)) >= this.fromDateControl.value || !this.fromDateControl.value) && ((new Date(l.date)) <= this.toDateControl.value || !this.toDateControl.value))
    this.updateTableData(result);
    this.applyFilter(this.filterValue);
  }
}

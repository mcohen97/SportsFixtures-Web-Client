import { Component, OnInit, ViewChild } from '@angular/core';
import {MatPaginator, MatSort, MatTableDataSource} from '@angular/material';
import { User } from 'src/app/classes/user';
import { Globals } from 'src/app/globals';
import { UsersService } from 'src/app/services/users/users.service';

@Component({
  selector: 'users-list',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  displayedColumns: string[] = ['username', 'name', 'surname', 'email'];
  dataSource:MatTableDataSource<User>;
  @ViewChild(MatPaginator) paginator:MatPaginator;
  @ViewChild(MatSort) sort:MatSort;
  errorMessage: string;
  errorLoadingUsers: boolean;

  constructor(private globals:Globals, private usersService:UsersService) {
    this.getUsers();
  }
  
  ngOnInit() {

  }

  private getUsers(){
    this.usersService.getAllUsers().subscribe(
      ((data:Array<User>) => this.successfulUsersGetter(data)),
      ((error:any) => this.handleError(error))
    )
  }

  private successfulUsersGetter(users:Array<User>){
    this.dataSource = new MatTableDataSource(users);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }
  private handleError(error:{errorMessage:string}) {
    this.errorMessage = error.errorMessage;
    this.errorLoadingUsers = true;
    console.log(this.errorMessage);
  }
  
  applyFilter(filterValue:string){
    this.dataSource.filter = filterValue.trim().toLowerCase();
    if(this.dataSource.paginator){
      this.dataSource.paginator.firstPage();
    }
  }


}

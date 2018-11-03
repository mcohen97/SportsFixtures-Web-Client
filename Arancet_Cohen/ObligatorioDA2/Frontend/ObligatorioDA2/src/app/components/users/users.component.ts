import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import {MatPaginator, MatSort, MatTableDataSource, MatDialogRef, MAT_DIALOG_DATA, MatDialog} from '@angular/material';
import { User } from 'src/app/classes/user';
import { Globals } from 'src/app/globals';
import { UsersService } from 'src/app/services/users/users.service';
import { UserEditDialogComponent } from './user-edit-dialog';
import { ConfirmationDialogComponent, DialogInfo } from '../confirmation-dialog/confirmation-dialog';
import { UserAddDialogComponent } from './user-add-dialog/user-add-dialog.component';
import { ErrorResponse } from 'src/app/classes/error';

@Component({
  selector: 'users-list',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  displayedColumns: string[] = ['username', 'name', 'surname', 'email', 'options'];
  dataSource:MatTableDataSource<User>;
  @ViewChild(MatPaginator) paginator:MatPaginator;
  @ViewChild(MatSort) sort:MatSort;
  errorMessage: string;
  userEdited: User;
  rowEdited: User;

  constructor(private dialog:MatDialog, private globals:Globals, private usersService:UsersService) {
    this.getUsers();
  }
  
  ngOnInit() {

  }

  private getUsers(){
    this.usersService.getAllUsers().subscribe(
      ((data:Array<User>) => this.updateTableData(data)),
      ((error:any) => this.handleError(error))
    )
  }

  private updateTableData(users:Array<User>){
    this.dataSource = new MatTableDataSource(users);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  private handleError(error:ErrorResponse) {
  }
  
  applyFilter(filterValue:string){
    this.dataSource.filter = filterValue.trim().toLowerCase();
    if(this.dataSource.paginator){
      this.dataSource.paginator.firstPage();
    }
  }

  openEditDialog(aUser:User):void{
    this.userEdited = User.getClone(aUser);
    this.rowEdited = aUser;
    const dialogRef = this.dialog.open(UserEditDialogComponent, {
      width:'500px',
      data: this.userEdited
    });
    dialogRef.afterClosed().subscribe(
      ((result:User) => {
        this.rowEdited.name = result.name;
        this.rowEdited.surname = result.surname;
        this.rowEdited.email = result.email;
      })
    )
  }

  openDeleteDialog(aUser:User):void{
    var confirmation:Boolean;
    confirmation = false;
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      width:'500px',
      data: {
        confirmation: confirmation,
        title: "Delete " + aUser.username,
        message: "This operation needs confirmation. It can not be undone."
      }
    });
    dialogRef.afterClosed().subscribe(
      ((dialgoResponse:DialogInfo) => {
        if(dialgoResponse.confirmation)
          this.performDelete(aUser);
      })
    )
  }
  performDelete(aUser: User): void {
    this.usersService.deleteUser(aUser.username).subscribe(
      ((result:any) => this.updateTableData(this.dataSource.data.filter((u:User)=>u.username != aUser.username))),
      ((error:any) => this.handleError(error))
    );
  }

  openAddDialog():void{
    var user:User;
    const dialogRef = this.dialog.open(UserAddDialogComponent, {
      width:'500px',
      data: {
        data: user,
      }
    });
    dialogRef.afterClosed().subscribe(
      ((newUser:User) => {
        if(newUser != undefined)
          this.performAdd(newUser);
      })
    )
  }

  performAdd(newUser:User):void{
    this.dataSource.data.push(newUser);
    this.dataSource._updateChangeSubscription();
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }
}



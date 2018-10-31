import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import {MatPaginator, MatSort, MatTableDataSource, MatDialogRef, MAT_DIALOG_DATA, MatDialog} from '@angular/material';
import { User } from 'src/app/classes/user';
import { Globals } from 'src/app/globals';
import { UsersService } from 'src/app/services/users/users.service';
import { UserEditDialog } from './user-edit-dialog';
import { ConfirmationDialog, DialogInfo } from '../confirmation-dialog/confirmation-dialog';

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
  errorLoadingUsers: boolean;
  userEdited: User;
  rowEdited: User;

  constructor(private dialog:MatDialog, private globals:Globals, private usersService:UsersService) {
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

  openEditDialog(aUser:User):void{
    this.userEdited = User.getClone(aUser);
    this.rowEdited = aUser;
    const dialogRef = this.dialog.open(UserEditDialog, {
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
    const dialogRef = this.dialog.open(ConfirmationDialog, {
      width:'500px',
      data: {
        confirmation: confirmation,
        title: "Delete " + aUser.username,
        message: "Confirm that you want to delete this user. This action can not be undone."
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
      ((result:any) => {this.dataSource = new MatTableDataSource(this.dataSource.data.filter((u:User)=>u.username != aUser.username))}),
      ((error:any) => this.handleError(error))
    );
  }
}



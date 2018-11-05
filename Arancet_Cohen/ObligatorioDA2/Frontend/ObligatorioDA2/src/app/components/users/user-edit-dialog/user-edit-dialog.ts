import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import {MatPaginator, MatSort, MatTableDataSource, MatDialogRef, MAT_DIALOG_DATA, MatDialog} from '@angular/material';
import { User } from 'src/app/classes/user';
import { UsersComponent } from '../users.component';

@Component({
    selector: 'dialog-user-edit',
    templateUrl: 'dialog-user-edit.html',
    styleUrls: ['dialog-user-edit.css']
  })
  export class UserEditDialogComponent {
  
    constructor(
      public dialogRef: MatDialogRef<UserEditDialogComponent>,
      @Inject(MAT_DIALOG_DATA) public data: User) {}
  
    onNoClick(): void {
      this.dialogRef.close();
    }
  
  }
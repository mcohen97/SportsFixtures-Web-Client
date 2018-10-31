import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import {MatPaginator, MatSort, MatTableDataSource, MatDialogRef, MAT_DIALOG_DATA, MatDialog} from '@angular/material';
import { User } from 'src/app/classes/user';
import { UsersComponent } from '../users/users.component';

@Component({
    selector: 'confirmation-dialog',
    templateUrl: 'confirmation-dialog.html',
    styleUrls: ['confirmation-dialog.css']
  })
  export class ConfirmationDialog {
  
    constructor(
      public dialogRef: MatDialogRef<ConfirmationDialog>,
      @Inject(MAT_DIALOG_DATA) public data:DialogInfo
      ) {}
  
    onCancelClick(): void {
      this.data.confirmation = false;
      this.dialogRef.close();
    }

    onConfirmClick():void{
      this.data.confirmation = true;
    }
  
  }

  export interface DialogInfo {
    confirmation:Boolean,
    title:string,
    message:string
  }
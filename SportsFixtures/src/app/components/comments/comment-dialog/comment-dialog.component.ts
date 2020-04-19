import { Component, OnInit, ViewChild, Inject, ElementRef } from '@angular/core';
import {MatPaginator, MatSort, MatTableDataSource, MatDialogRef, MAT_DIALOG_DATA, MatDialog, ErrorStateMatcher} from '@angular/material';
import { Injectable } from "@angular/core";
import { Http, Response, RequestOptions, Headers } from "@angular/http"; 
import { Observable, throwError } from "rxjs";  
import { map, tap, catchError } from 'rxjs/operators'; 
import { Globals } from "src/app/globals";
import { Comment } from "src/app/classes/comment";
import { ErrorResponse } from "src/app/classes/error";
import { Validators, FormControl, FormGroupDirective, NgForm, ValidationErrors, ValidatorFn, AbstractControl } from '@angular/forms';
import { CommentsService } from 'src/app/services/comments/comments.service';
import { CustomValidators } from 'src/app/classes/custom-validators';
import { GenericError } from 'src/app/classes/genericError';

@Component({
  selector: 'app-comment-dialog',
  templateUrl: './comment-dialog.component.html',
  styleUrls: ['./comment-dialog.component.css']
})
export class CommentDialogComponent {

  genericError = new GenericError();
  errorFlags = [];
  errorStatus = 200;
  commentControl:FormControl;


  constructor(
    public dialogRef: MatDialogRef<CommentDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data:CommentDialogData,
    private commentsService: CommentsService
  ) { 
    this.resetErrors();
    this.commentControl = new FormControl();
    this.setValidators();
  }

  matcher = new MyErrorStateMatcher();

  onNoClick():void {
    this.dialogRef.close();
  }

  onSaveClick():void{
    this.markControlsAsTouched();
    this.updateControls();
    if(this.allValid())
      var comment = new Comment(this.commentControl.value);
      comment.makerUsername = Globals.getUsername();
      this.addComment(comment, this.data.encounterId);
  }

  allValid(): boolean {
    return  this.commentControl.valid
  }

  addComment(newComment:Comment, matchId:number):void{
    this.commentsService.addComment(newComment, matchId).subscribe(
      ((result:Comment) => {
        newComment.id = result.id;
        this.dialogRef.close(newComment);
      }),
      ((error:ErrorResponse) => this.handleError(error))
    );
  }

  handleError(error: ErrorResponse): void {    
    this.genericError = <GenericError> error.errorObject;
    this.errorStatus = error.errorCode;
    this.checkErrors();
    this.setValidators();
    this.markControlsAsTouched();
    this.updateControls();
    this.resetErrors();
  }

  private updateControls() {
    this.commentControl.updateValueAndValidity();
  }

  private markControlsAsTouched() {
    this.commentControl.markAsTouched();
  }

  private checkErrors() {
    this.errorFlags['errorMessage'] = this.genericError.errorMessage != undefined;
  }

  private resetErrors(){
    this.errorFlags['errorMessage'] = false;

  }

  private setValidators() {
    this.commentControl.setValidators([
      this.existError("errorMessage"),
      Validators.required
    ]);
  }

  private existError(keyError:string): ValidatorFn {
    return (control:AbstractControl) : ValidationErrors | null=>{
      if(this.errorFlags[keyError]){
        let temp = [];
        temp[keyError] = true;
        return temp;
      }
      else
        return null;
    }
  }

  removeError(control:AbstractControl, keyError:string):void{
    control.setErrors(null);
    this.errorFlags[keyError] = false;
    control.updateValueAndValidity();
  }
}

export class MyErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null)   : boolean {
    const isSubmitted = form && form.submitted;
    return control && control.invalid && (control.dirty || control.touched);
  }
}

export interface CommentDialogData{
  encounterId: number;
  title: string;
}
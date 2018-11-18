import { Injectable } from "@angular/core";
import { Http, Response, RequestOptions, Headers } from "@angular/http"; 
import { Observable, throwError } from "rxjs";  
import { map, tap, catchError } from 'rxjs/operators'; 
import { Globals } from "src/app/globals";
import { ErrorResponse } from "src/app/classes/error";
import { HttpErrorResponse } from "@angular/common/http";
import { Comment } from "src/app/classes/comment";
import { Team } from "src/app/classes/team";

@Injectable()
export class CommentsService {
  private WEB_API_URL : string = 'https://localhost:5001/api/matches/'; 
 
  constructor(private _httpService: Http) {  } 


  getComment(id:number): Observable<Comment>{
      const myHeaders = new Headers(); 
      myHeaders.append('Accept', 'application/json');
      myHeaders.append('Authorization', 'Bearer '+ Globals.getToken());
      const requestOptions = new RequestOptions({headers: myHeaders}); 
      return this._httpService.get(this.WEB_API_URL+"comments/"+id, requestOptions) 
      .pipe( 
          map((response : Response) => response.json()),
          catchError(this.handleError)
      ); 
  }

  getEncounterComments(encounterId:number): Observable<Array<Comment>>{
    const myHeaders = new Headers(); 
    myHeaders.append('Accept', 'application/json');
    myHeaders.append('Authorization', 'Bearer '+ Globals.getToken());
    const requestOptions = new RequestOptions({headers: myHeaders}); 
    return this._httpService.get(this.WEB_API_URL+encounterId+"/comments", requestOptions) 
    .pipe( 
        map((response : Response) => response.json()),
        catchError(this.handleError)
    ); 
}

  getAllComments():Observable<Array<Comment>>{
      const myHeaders = new Headers(); 
      myHeaders.append('Accept', 'application/json');
      myHeaders.append('Authorization', 'Bearer '+ Globals.getToken());
      const requestOptions = new RequestOptions({headers: myHeaders}); 
      return this._httpService.get(this.WEB_API_URL+"/comments", requestOptions) 
      .pipe( 
          map((response : Response) => response.json()),
          catchError(this.handleError)
      ); 
  }

  addComment(aComment:Comment, matchId:number):Observable<Comment>{
      const myHeaders = new Headers(); 
      myHeaders.append('Accept', 'application/json');
      myHeaders.append('Authorization', 'Bearer '+ Globals.getToken());
      const requestOptions = new RequestOptions({headers: myHeaders}); 
      return this._httpService.post(this.WEB_API_URL+matchId+"/"+"comments", aComment, requestOptions) 
      .pipe( 
          map((response : Response) => response.json()),
          catchError(this.handleError)
      ); 
  }
  
  private handleError(errorResponse: Response) { 
    var error = new ErrorResponse();
    error.errorMessage = errorResponse.statusText;
    error.errorCode = errorResponse.status;
    try {
      error.errorObject = errorResponse.json();
    } catch (error) {
      error.errorObject = {};  
    }
    return throwError(error); 
} 
}

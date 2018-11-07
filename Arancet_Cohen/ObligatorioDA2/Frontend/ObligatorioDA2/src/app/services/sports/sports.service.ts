import { Injectable } from "@angular/core";
import { Http, Response, RequestOptions, Headers } from "@angular/http"; 
import { Observable, throwError } from "rxjs";  
import { map, tap, catchError } from 'rxjs/operators'; 
import { Globals } from "src/app/globals";
import { ErrorResponse } from "src/app/classes/error";
import { HttpErrorResponse } from "@angular/common/http";
import { Sport } from "src/app/classes/sport";

@Injectable()
export class SportsService {
  private WEB_API_URL : string = 'https://localhost:5001/api/sports/'; 
 
  constructor(private _httpService: Http) {  } 


  getSport(name:string): Observable<Sport>{
      const myHeaders = new Headers(); 
      myHeaders.append('Accept', 'application/json');
      myHeaders.append('Authorization', 'Bearer '+ Globals.getToken());
      const requestOptions = new RequestOptions({headers: myHeaders}); 
      return this._httpService.get(this.WEB_API_URL+name, requestOptions) 
      .pipe( 
          map((response : Response) => response.json()),
          catchError(this.handleError)
      ); 
  }

  getAllSports():Observable<Array<Sport>>{
      const myHeaders = new Headers(); 
      myHeaders.append('Accept', 'application/json');
      myHeaders.append('Authorization', 'Bearer '+ Globals.getToken());
      const requestOptions = new RequestOptions({headers: myHeaders}); 
      return this._httpService.get(this.WEB_API_URL, requestOptions) 
      .pipe( 
          map((response : Response) => response.json()),
          catchError(this.handleError)
      ); 
  }

  deleteSport(name:string):Observable<Sport>{
      const myHeaders = new Headers(); 
      myHeaders.append('Accept', 'application/json');
      myHeaders.append('Authorization', 'Bearer '+ Globals.getToken());
      const requestOptions = new RequestOptions({headers: myHeaders}); 
      return this._httpService.delete(this.WEB_API_URL+name, requestOptions) 
      .pipe( 
          map((response : Response) => response.json()),
          catchError(this.handleError)
      ); 
  }

  addSport(aSport:Sport):Observable<Sport>{
      const myHeaders = new Headers(); 
      myHeaders.append('Accept', 'application/json');
      myHeaders.append('Authorization', 'Bearer '+ Globals.getToken());
      const requestOptions = new RequestOptions({headers: myHeaders}); 
      return this._httpService.post(this.WEB_API_URL, aSport, requestOptions) 
      .pipe( 
          map((response : Response) => response.json()),
          catchError(this.handleError)
      ); 
  }
  
  private handleError(error: Response) { 
      const errorObj = new ErrorResponse();
      errorObj.errorMessage = error.statusText;
      errorObj.errorCode = error.status;
      errorObj.errorObject = error.json();
      return throwError(errorObj || 'Server error'); 
  } 
}

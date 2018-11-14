import { Injectable } from "@angular/core";
import { Http, Response, RequestOptions, Headers } from "@angular/http"; 
import { Observable, throwError } from "rxjs";  
import { map, tap, catchError } from 'rxjs/operators'; 
import { Globals } from "src/app/globals";
import { ErrorResponse } from "src/app/classes/error";
import { HttpErrorResponse } from "@angular/common/http";
import { Log } from "src/app/classes/log";
import { Team } from "src/app/classes/team";

@Injectable()
export class LogsService {
  private WEB_API_URL : string = 'https://localhost:5001/api/logs/'; 
 
  constructor(private _httpService: Http) {  } 


  getLog(name:string): Observable<Log>{
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

  getAllLogs():Observable<Array<Log>>{
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

  deleteLog(name:string):Observable<Log>{
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

  addLog(aLog:Log):Observable<Log>{
      const myHeaders = new Headers(); 
      myHeaders.append('Accept', 'application/json');
      myHeaders.append('Authorization', 'Bearer '+ Globals.getToken());
      const requestOptions = new RequestOptions({headers: myHeaders}); 
      return this._httpService.post(this.WEB_API_URL, aLog, requestOptions) 
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

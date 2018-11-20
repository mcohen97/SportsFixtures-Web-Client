import { Injectable } from "@angular/core";
import { Http, Response, RequestOptions, Headers } from "@angular/http"; 
import { Observable, throwError } from "rxjs";  
import { map, tap, catchError } from 'rxjs/operators'; 
import { Globals } from "src/app/globals";
import { ErrorResponse } from "src/app/classes/error";
import { HttpErrorResponse } from "@angular/common/http";
import { Sport } from "src/app/classes/sport";
import { Team } from "src/app/classes/team";
import { TablePosition } from "src/app/classes/table-position";

@Injectable()
export class SportsService {
  private WEB_API_URL : string = Globals.WEB_API_URL+'sports/'; 
 
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

  getTeams(sportName:string): Observable<Array<Team>>{
    const myHeaders = new Headers(); 
    myHeaders.append('Accept', 'application/json');
    myHeaders.append('Authorization', 'Bearer '+ Globals.getToken());
    const requestOptions = new RequestOptions({headers: myHeaders}); 
    return this._httpService.get(this.WEB_API_URL+sportName+"/teams", requestOptions) 
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

  getSportTable(sportName:string):Observable<Array<TablePosition>>{
    const myHeaders = new Headers(); 
    myHeaders.append('Accept', 'application/json');
    myHeaders.append('Authorization', 'Bearer '+ Globals.getToken());
    const requestOptions = new RequestOptions({headers: myHeaders}); 
    return this._httpService.get(this.WEB_API_URL+sportName+"/table", requestOptions) 
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

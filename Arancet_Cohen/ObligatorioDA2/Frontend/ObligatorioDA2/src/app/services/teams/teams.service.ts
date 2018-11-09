import { Injectable } from "@angular/core";
import { Http, Response, RequestOptions, Headers } from "@angular/http"; 
import { Observable, throwError } from "rxjs";  
import { map, tap, catchError } from 'rxjs/operators'; 
import { Globals } from "src/app/globals";
import { Team } from "src/app/classes/team";
import { ErrorResponse } from "src/app/classes/error";
import { HttpErrorResponse, HttpResponse } from "@angular/common/http";
import { TeamError } from "src/app/classes/teamError";

@Injectable()
export class TeamsService {

  private WEB_API_URL : string = 'https://localhost:5001/api/teams/'; 
 
  constructor(private _httpService: Http) {  } 


  getTeam(id:number): Observable<Team>{
      const myHeaders = new Headers(); 
      myHeaders.append('Accept', 'application/json');
      myHeaders.append('Authorization', 'Bearer '+ Globals.getToken());
      const requestOptions = new RequestOptions({headers: myHeaders}); 
      return this._httpService.get(this.WEB_API_URL+id, requestOptions) 
      .pipe( 
          map((response : Response) => response.json()),
          catchError(this.handleError)
      ); 
  }

  getAllTeams():Observable<Array<Team>>{
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

  modifyTeam(team:Team):Observable<Team>{
      const myHeaders = new Headers(); 
      myHeaders.append('Accept', 'application/json');
      myHeaders.append('Authorization', 'Bearer '+ Globals.getToken());
      const requestOptions = new RequestOptions({headers: myHeaders}); 
      return this._httpService.put(this.WEB_API_URL+team.id, team, requestOptions) 
      .pipe( 
          map((response : Response) => response.json()),
          catchError(this.handleError)
      ); 
  }

  deleteTeam(id:number):Observable<Response>{
      const myHeaders = new Headers(); 
      myHeaders.append('Accept', 'application/json');
      myHeaders.append('Authorization', 'Bearer '+ Globals.getToken());
      const requestOptions = new RequestOptions({headers: myHeaders}); 
      return this._httpService.delete(this.WEB_API_URL+id, requestOptions) 
      .pipe( 
          map((response : Response) => response.json()),
          catchError(this.handleError)
      ); 
  }

  addTeam(aTeam:Team):Observable<Team>{
      const myHeaders = new Headers(); 
      myHeaders.append('Accept', 'application/json');
      myHeaders.append('Authorization', 'Bearer '+ Globals.getToken());
      const requestOptions = new RequestOptions({headers: myHeaders}); 
      return this._httpService.post(this.WEB_API_URL, aTeam, requestOptions) 
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

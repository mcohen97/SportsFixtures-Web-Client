import { Injectable } from "@angular/core";
import { Http, Response, RequestOptions, Headers } from "@angular/http"; 
import { Observable, throwError } from "rxjs";  
import { map, tap, catchError } from 'rxjs/operators'; 
import { Globals } from "src/app/globals";
import { User } from "src/app/classes/user";
import { ErrorResponse } from "src/app/classes/error";
import { HttpErrorResponse } from "@angular/common/http";
import { OkMessage } from "src/app/classes/okMessage";
import { Team } from "src/app/classes/team";

@Injectable()
export class UsersService {
    private WEB_API_URL : string = Globals.WEB_API_URL+'users/'; 
 
    constructor(private _httpService: Http) {  } 


    getUser(username:string): Observable<User>{
        const myHeaders = new Headers(); 
        myHeaders.append('Accept', 'application/json');
        myHeaders.append('Authorization', 'Bearer '+ Globals.getToken());
        const requestOptions = new RequestOptions({headers: myHeaders}); 
        return this._httpService.get(this.WEB_API_URL+username, requestOptions) 
        .pipe( 
            map((response : Response) => response.json()),
            catchError(this.handleError)
        ); 
    }

    getAllUsers():Observable<Array<User>>{
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

    modifyUser(user:User):Observable<User>{
        const myHeaders = new Headers(); 
        myHeaders.append('Accept', 'application/json');
        myHeaders.append('Authorization', 'Bearer '+ Globals.getToken());
        const requestOptions = new RequestOptions({headers: myHeaders}); 
        return this._httpService.put(this.WEB_API_URL+user.username, user, requestOptions) 
        .pipe( 
            map((response : Response) => response.json()),
            catchError(this.handleError)
        ); 
    }

    deleteUser(username:string):Observable<Response>{
        const myHeaders = new Headers(); 
        myHeaders.append('Accept', 'application/json');
        myHeaders.append('Authorization', 'Bearer '+ Globals.getToken());
        const requestOptions = new RequestOptions({headers: myHeaders}); 
        return this._httpService.delete(this.WEB_API_URL+username, requestOptions) 
        .pipe( 
            map((response : Response) => response.json()),
            catchError(this.handleError)
        ); 
    }

    addUser(aUser:User):Observable<User>{
        const myHeaders = new Headers(); 
        myHeaders.append('Accept', 'application/json');
        myHeaders.append('Authorization', 'Bearer '+ Globals.getToken());
        const requestOptions = new RequestOptions({headers: myHeaders}); 
        return this._httpService.post(this.WEB_API_URL, aUser, requestOptions) 
        .pipe( 
            map((response : Response) => response.json()),
            catchError(this.handleError)
        ); 
    }

    followTeam(teamId:number):Observable<OkMessage>{
        const myHeaders = new Headers(); 
        myHeaders.append('Accept', 'application/json');
        myHeaders.append('Authorization', 'Bearer '+ Globals.getToken());
        const requestOptions = new RequestOptions({headers: myHeaders}); 
        return this._httpService.post(this.WEB_API_URL+"followed-teams/"+teamId, undefined, requestOptions) 
        .pipe( 
            map((response : Response) => response.json()),
            catchError(this.handleError)
        ); 
    }

    getFollowedTeams(username:string):Observable<Array<Team>>{
        const myHeaders = new Headers(); 
        myHeaders.append('Accept', 'application/json');
        myHeaders.append('Authorization', 'Bearer '+ Globals.getToken());
        const requestOptions = new RequestOptions({headers: myHeaders}); 
        return this._httpService.get(this.WEB_API_URL+username+"/followed-teams", requestOptions) 
        .pipe( 
            map((response : Response) => response.json()),
            catchError(this.handleError)
        );
    }
    
    unfollowTeam(teamId:number):Observable<OkMessage>{
        const myHeaders = new Headers(); 
        myHeaders.append('Accept', 'application/json');
        myHeaders.append('Authorization', 'Bearer '+ Globals.getToken());
        const requestOptions = new RequestOptions({headers: myHeaders}); 
        return this._httpService.delete(this.WEB_API_URL+"followed-teams/"+teamId, requestOptions) 
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
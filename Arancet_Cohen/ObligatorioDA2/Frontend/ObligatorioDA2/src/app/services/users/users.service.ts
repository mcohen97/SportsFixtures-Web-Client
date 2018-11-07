import { Injectable } from "@angular/core";
import { Http, Response, RequestOptions, Headers } from "@angular/http"; 
import { Observable, throwError } from "rxjs";  
import { map, tap, catchError } from 'rxjs/operators'; 
import { Globals } from "src/app/globals";
import { User } from "src/app/classes/user";
import { ErrorResponse } from "src/app/classes/error";
import { HttpErrorResponse } from "@angular/common/http";

@Injectable()
export class UsersService {
    private WEB_API_URL : string = 'https://localhost:5001/api/users/'; 
 
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
    
    private handleError(error: Response) { 
        const errorObj = new ErrorResponse();
        errorObj.errorMessage = error.statusText;
        errorObj.errorCode = error.status;
        errorObj.errorObject = error.json();
        return throwError(errorObj || 'Server error'); 
    } 

}
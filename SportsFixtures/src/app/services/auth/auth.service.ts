import { Injectable } from "@angular/core";
import { Token } from "../../classes/token";
import { Http, Response, RequestOptions, Headers } from "@angular/http"; 
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from "rxjs";  
import { map, tap, catchError } from 'rxjs/operators'; 
import { ErrorResponse } from "src/app/classes/error";
import { Globals } from "src/app/globals";

@Injectable()
export class AuthService {
    private WEB_API_URL : string = Globals.WEB_API_URL+"authentication"; 
 
    constructor(private _httpService: Http) {  } 


    authenticate(username:string, password:string): Observable<Token>{
        const myHeaders = new Headers(); 
        myHeaders.append('Accept', 'application/json');     
        const requestOptions = new RequestOptions({headers: myHeaders}); 
        return this._httpService.post(this.WEB_API_URL,{Username: username, Password: password}, requestOptions) 
        .pipe( 
            map((response : Response) => <Token> response.json()),
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
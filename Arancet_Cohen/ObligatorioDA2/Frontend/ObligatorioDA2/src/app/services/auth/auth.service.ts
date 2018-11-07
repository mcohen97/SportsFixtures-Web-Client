import { Injectable } from "@angular/core";
import { Token } from "../../classes/token";
import { Http, Response, RequestOptions, Headers } from "@angular/http"; 
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from "rxjs";  
import { map, tap, catchError } from 'rxjs/operators'; 
import { ErrorResponse } from "src/app/classes/error";

@Injectable()
export class AuthService {
    private WEB_API_URL : string = 'https://localhost:5001/api/authentication'; 
 
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
        error.errorObject = errorResponse.json();
        return throwError(error); 
    } 

}
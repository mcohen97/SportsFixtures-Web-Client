import { Injectable } from "@angular/core";
import { User } from "../../classes/user";
import { Http, Response, RequestOptions, Headers } from "@angular/http"; 
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from "rxjs";  
import { map, tap, catchError } from 'rxjs/operators'; 

@Injectable()
export class LoginService {
    private WEB_API_URL : string = 'https://localhost:5001/api/authentication'; 
 
    constructor(private _httpService: Http) {  } 
 
    /*getPets(): Observable<Array<Pet>> { 
        const myHeaders = new Headers(); 
        myHeaders.append('Accept', 'application/json');     
        const requestOptions = new RequestOptions({headers: myHeaders}); 
           
        return this._httpService.get(this.WEB_API_URL, requestOptions) 
        .pipe( 
            map((response : Response) => <Array<Pet>> response.json()),
            tap(data => console.log('Los datos que obtuvimos fueron: ' + JSON.stringify(data))),
            catchError(this.handleError) 
        ); 
    }
    
    getPetById(id:string): Observable<Pet> {
        const myHeaders = new Headers(); 
        myHeaders.append('Accept', 'application/json');     
        const requestOptions = new RequestOptions({headers: myHeaders}); 
           
        return this._httpService.get(this.WEB_API_URL, requestOptions) 
        .pipe( 
            map((response : Response) => <Pet> response.json().find((pet:Pet) => pet.id = id)),
            tap(data => console.log('Los datos que obtuvimos fueron: ' + JSON.stringify(data))),
            catchError(this.handleError) 
        ); 
    }*/
 
    /*authenticate(username:string, password:string): string{
        var result:string;
        console.log(username);
        console.log(password);
        this._httpService.post(this.WEB_API_URL, {Username:username, Password:password}).subscribe(
            (token:string)=> {
                result = token;
            },
            (error:any) => this.handleError(error)
        );
        return result;
    }*/

    authenticate(username:string, password:string): string{
        var result:string;
        const myHeaders = new Headers(); 
        myHeaders.append('Accept', 'application/json');     
        const requestOptions = new RequestOptions({headers: myHeaders}); 
           
        this._httpService.post(this.WEB_API_URL,{Username: username, Password: password}, requestOptions) 
        .pipe( 
            map((response : Response) => <string> response.json()),
            tap(data => console.log('Los datos que obtuvimos fueron: ' + JSON.stringify(data))),
        ); 

        return result;
    }
    
    private handleError(error: Response) { 
        console.error(error); 
        //return throwError(error.json().error|| 'Server error'); 
    } 

}
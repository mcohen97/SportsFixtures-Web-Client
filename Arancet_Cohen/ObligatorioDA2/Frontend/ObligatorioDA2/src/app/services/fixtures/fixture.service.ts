import { Injectable } from "@angular/core";
import { Http, Response, RequestOptions, Headers } from "@angular/http"; 
import { Observable, throwError } from "rxjs";  
import { map, tap, catchError } from 'rxjs/operators'; 
import { Globals } from "src/app/globals";
import { Encounter } from "src/app/classes/encounter";
import { ErrorResponse } from "src/app/classes/error";
import { HttpErrorResponse } from "@angular/common/http";
import { Standing } from "src/app/classes/standing";
import { Result } from "src/app/classes/result";
import { Fixture } from "src/app/classes/fixture";

@Injectable()
export class FixturesService {
    private WEB_API_URL : string = Globals.WEB_API_URL+'fixtures/'; 
 
    constructor(private _httpService: Http) {  } 


    getFixturesNames(): Observable<Array<string>>{
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

    /*getAllFixtures():Observable<Array<Fixture>>{
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

    modifyFixture(fixture:Fixture):Observable<Fixture>{
        const myHeaders = new Headers(); 
        myHeaders.append('Accept', 'application/json');
        myHeaders.append('Authorization', 'Bearer '+ Globals.getToken());
        const requestOptions = new RequestOptions({headers: myHeaders}); 
        return this._httpService.put(this.WEB_API_URL+fixture.id, fixture, requestOptions) 
        .pipe( 
            map((response : Response) => response.json()),
            catchError(this.handleError)
        ); 
    }

    deleteFixture(id:number):Observable<Response>{
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
    */
    addFixture(aFixture:Fixture):Observable<Fixture>{
        const myHeaders = new Headers(); 
        myHeaders.append('Accept', 'application/json');
        myHeaders.append('Authorization', 'Bearer '+ Globals.getToken());
        const requestOptions = new RequestOptions({headers: myHeaders}); 
        return this._httpService.post(this.WEB_API_URL + aFixture.sportName, aFixture, requestOptions) 
        .pipe( 
            map((response : Response) => response.json()),
            catchError(this.handleError)
        ); 
    }
    /*
    addResult(fixtureId:number, aResult:Result):Observable<Fixture>{
        const myHeaders = new Headers(); 
        myHeaders.append('Accept', 'application/json');
        myHeaders.append('Authorization', 'Bearer '+ Globals.getToken());
        const requestOptions = new RequestOptions({headers: myHeaders}); 
        return this._httpService.post(this.WEB_API_URL+fixtureId+"/result", aResult, requestOptions) 
        .pipe( 
            map((response : Response) => response.json()),
            catchError(this.handleError)
        ); 
    }*/
    
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
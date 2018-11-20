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

@Injectable()
export class EncountersService {
    private WEB_API_URL : string =  Globals.WEB_API_URL+'matches/'; 
 
    constructor(private _httpService: Http) {  } 


    getEncounter(id:number): Observable<Encounter>{
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

    getAllEncountersOfSport(sportName:string):Observable<Array<Encounter>>{
        const myHeaders = new Headers(); 
        myHeaders.append('Accept', 'application/json');
        myHeaders.append('Authorization', 'Bearer '+ Globals.getToken());
        const requestOptions = new RequestOptions({headers: myHeaders}); 
        return this._httpService.get(this.WEB_API_URL+"sport/"+sportName, requestOptions) 
        .pipe( 
            map((response : Response) => response.json()),
            catchError(this.handleError)
        ); 
    }


    getAllEncounters():Observable<Array<Encounter>>{
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

    modifyEncounter(encounter:Encounter):Observable<Encounter>{
        const myHeaders = new Headers(); 
        myHeaders.append('Accept', 'application/json');
        myHeaders.append('Authorization', 'Bearer '+ Globals.getToken());
        const requestOptions = new RequestOptions({headers: myHeaders}); 
        return this._httpService.put(this.WEB_API_URL+encounter.id, encounter, requestOptions) 
        .pipe( 
            map((response : Response) => response.json()),
            catchError(this.handleError)
        ); 
    }

    deleteEncounter(id:number):Observable<Response>{
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

    addEncounter(aEncounter:Encounter):Observable<Encounter>{
        const myHeaders = new Headers(); 
        myHeaders.append('Accept', 'application/json');
        myHeaders.append('Authorization', 'Bearer '+ Globals.getToken());
        const requestOptions = new RequestOptions({headers: myHeaders}); 
        return this._httpService.post(this.WEB_API_URL, aEncounter, requestOptions) 
        .pipe( 
            map((response : Response) => response.json()),
            catchError(this.handleError)
        ); 
    }

    addResult(encounterId:number, aResult:Result):Observable<Encounter>{
        const myHeaders = new Headers(); 
        myHeaders.append('Accept', 'application/json');
        myHeaders.append('Authorization', 'Bearer '+ Globals.getToken());
        const requestOptions = new RequestOptions({headers: myHeaders}); 
        return this._httpService.post(this.WEB_API_URL+encounterId+"/result", aResult, requestOptions) 
        .pipe( 
            map((response : Response) => response.json()),
            catchError(this.handleError)
        ); 
    }

    getEncountersOfTeam(teamId:number):Observable<Array<Encounter>>{
        const myHeaders = new Headers(); 
        myHeaders.append('Accept', 'application/json');
        myHeaders.append('Authorization', 'Bearer '+ Globals.getToken());
        const requestOptions = new RequestOptions({headers: myHeaders}); 
        return this._httpService.get(this.WEB_API_URL+"team/"+teamId, requestOptions) 
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
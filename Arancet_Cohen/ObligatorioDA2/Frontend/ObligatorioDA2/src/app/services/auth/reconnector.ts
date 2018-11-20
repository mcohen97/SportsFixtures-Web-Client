import { AuthService } from "./auth.service";
import { Token } from "../../classes/token";
import { Observable } from "rxjs";
import { Globals } from "../../globals";
import { ErrorResponse } from "../../classes/error";
import { Injectable } from "@angular/core";

@Injectable()
export class ReConnector{

    token:Observable<Token>;
    tryCount:number;
    

    constructor(private auth: AuthService){
        this.tryCount = 0;
    }

    reconnect()
    {
        var b = false;
        
    }

    tryReconnect(resultRef:{result:boolean}){
        this.tryCount++;
        Globals.logOut();
        const username = Globals.getUsername();
        const password = Globals.getPassword();
        this.token = this.auth.authenticate(username, password); 
        this.token.subscribe(
            ((data:Token) => {
                resultRef.result = true;
                this.successfulLogin(data, username, password)
            }),
            ((error:ErrorResponse) => this.handleError(error)),
            (()=>{
                if(this.tryCount <= 15 && !Globals.isUserLogged())
                    this.tryReconnect(resultRef);
                else
                    resultRef.result = true;
            })
        )
    }

    private successfulLogin(tokenResponse:Token, username:string, password:string) {
        Globals.setToken(tokenResponse.token);
        Globals.setLoggedUser(username, password);
        console.log("logueado con exito: " + Globals.getToken());
    }

    private handleError(error:ErrorResponse) {
        if (error.errorCode != 401){
            Globals.logOut();
        }
        
    }

    reset(){
        this.tryCount = 0;
    }

}
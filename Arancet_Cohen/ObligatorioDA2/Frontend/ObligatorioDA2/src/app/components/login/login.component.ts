import { Component } from '@angular/core';
import { AuthService } from 'src/app/services/auth/auth.service';
import { Observable } from 'rxjs';
import { errorHandler } from '@angular/platform-browser/src/browser';
import { Token } from 'src/app/classes/token';
import { Globals } from 'src/app/globals';
import { UsersService } from 'src/app/services/users/users.service';
import { User } from 'src/app/classes/user';
import { HttpErrorResponse } from '@angular/common/http';
import { ErrorResponse } from 'src/app/classes/error';

@Component({
  selector: 'login-component',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

    token:Observable<Token>;
    errorMessage:string;
    errorLogin:boolean;
    isLoading = false;

    constructor(private auth: AuthService){
        this.errorMessage = "";
        this.errorLogin = false;
    }

    login(event){
        this.errorLogin = false;
        event.preventDefault();
        const target = event.target;
        const username = target.querySelector('#username').value;
        const password = target.querySelector('#password').value;
        this.isLoading = true;
        this.token = this.auth.authenticate(username, password); 
        this.token.subscribe(
            ((data:Token) => this.successfulLogin(data, username, password)),
            ((error:ErrorResponse) => this.handleError(error))
        )

    }

    private successfulLogin(tokenResponse:Token, username:string, password:string) {
        Globals.setToken(tokenResponse.token);
        Globals.setLoggedUser(username, password);
        console.log("logueado con exito: " + Globals.getToken());
        this.isLoading = false;
    }

    private handleError(error:ErrorResponse) {
        if(error.errorCode>=500)
            this.errorMessage = "Server error";
        else if(error.errorCode == 0)
            this.errorMessage = "No connection to server";
        else
            this.errorMessage = "Wrong username or password";
        console.log(this.errorMessage);
        this.errorLogin = true;
        this.isLoading = false;
    }

    /*private getUserLoggedInfo(username:string){
        var userResponse:Observable<User>;
        userResponse = this.userServices.getUser(username);
        userResponse.subscribe(
            ((data:User) => this.successfulUserLoggedInfoGetted(data))
        );
    }
     private successfulUserLoggedInfoGetted(user:User){
        Globals.setLoggedUser(user);
        console.log(Globals.getUserLogged());
     }*/
   
}

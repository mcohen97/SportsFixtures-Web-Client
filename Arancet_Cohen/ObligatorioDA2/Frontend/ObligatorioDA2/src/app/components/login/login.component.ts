import { Component } from '@angular/core';
import { AuthService } from 'src/app/services/auth/auth.service';
import { Observable } from 'rxjs';
import { errorHandler } from '@angular/platform-browser/src/browser';
import { Token } from 'src/app/classes/token';
import { Globals } from 'src/app/globals';
import { UsersService } from 'src/app/services/users/users.service';
import { User } from 'src/app/classes/user';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'login-component',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

    token:Observable<Token>;
    errorMessage:string;
    errorLogin:boolean;

    constructor(private globals:Globals, private auth: AuthService, private userServices: UsersService){
        this.errorMessage = "";
        this.errorLogin = false;
    }

    login(event){
        this.errorLogin = false;
        event.preventDefault();
        const target = event.target;
        const username = target.querySelector('#username').value;
        const password = target.querySelector('#password').value;
        this.token = this.auth.authenticate(username, password);
        this.token.subscribe(
            ((data:Token) => this.successfulLogin(data, username)),
            ((error:any) => this.handleError(error))
        )
    }

    private successfulLogin(tokenResponse:Token, username:string) {
        this.globals.token = tokenResponse.token;
        console.log("logueado con exito: " + this.globals.token);
        this.getUserLoggedInfo(username);
    }

    private handleError(error:{errorMessage:string}) {
        this.errorMessage = "Wrong username or password";
        console.log(this.errorMessage);
        this.errorLogin = true;
    }

    private getUserLoggedInfo(username:string){
        var userResponse:Observable<User>;
        userResponse = this.userServices.getUser(username);
        userResponse.subscribe(
            ((data:User) => this.successfulUserLoggedInfoGetted(data))
        );
    }
     private successfulUserLoggedInfoGetted(user:User){
        this.globals.loggedUser = user;
        console.log(this.globals.loggedUser);
     }
   
}

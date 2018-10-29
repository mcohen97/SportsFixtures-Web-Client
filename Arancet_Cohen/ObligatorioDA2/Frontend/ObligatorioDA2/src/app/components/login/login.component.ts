import { Component } from '@angular/core';
import { LoginService } from 'src/app/services/login/login.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'login-component',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

    stringToken:string;

    constructor(private Auth: LoginService){
        this.stringToken = "";
    }

    login(event){
        event.preventDefault();
        const target = event.target;
        const username = target.querySelector('#username').value;
        const password = target.querySelector('#password').value;
        this.stringToken = this.Auth.authenticate(username, password);
        console.log(this.stringToken);
    }
   
}

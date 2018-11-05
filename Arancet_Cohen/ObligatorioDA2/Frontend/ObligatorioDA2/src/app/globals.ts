import { Injectable, OnInit } from '@angular/core';
import { User } from './classes/user';

@Injectable()
export class Globals {

  WEB_API_URL : string = 'https://localhost:5001/api'; 

  static setToken(token:string){
    localStorage.setItem("token", token);
  }

  static getToken(): string{
    return localStorage.getItem("token");
  }

  static setLoggedUser(username:string){
    localStorage.setItem("username", username)
  }

  static getUserLogged():string{
    return localStorage.getItem("username");
  }

  static isUserLogged():boolean{
    return localStorage.getItem("token") != "";
  }

  static logOut():void{
    localStorage.setItem("token", "");
  }
}
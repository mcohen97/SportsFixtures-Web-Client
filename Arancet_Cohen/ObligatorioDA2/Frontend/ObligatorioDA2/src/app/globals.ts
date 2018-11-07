import { Injectable, OnInit } from '@angular/core';
import { User } from './classes/user';
import { StringifyOptions } from 'querystring';

@Injectable()
export class Globals {

  WEB_API_URL : string = 'https://localhost:5001/api'; 

  static setToken(token:string){
    localStorage.setItem("token", token);
  }

  static getToken(): string{
    return localStorage.getItem("token");
  }

  static setLoggedUser(username:string, password:string){
    localStorage.setItem("username", username);
    localStorage.setItem("password", password);
  }

  static getPassword():string{
    return localStorage.getItem("password");
  }

  static getUsername():string{
    return localStorage.getItem("username");
  }

  static isUserLogged():boolean{
    return localStorage.getItem("token") != "";
  }

  static logOut():void{
    localStorage.setItem("token", "");
  }
}
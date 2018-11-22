import { Injectable, OnInit } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';


@Injectable()
export class Globals {

  public static WEB_API_URL : string = 'https://localhost:5001/api/';
  
  private static refs = [];

  static addInterval(ref:any){
    this.refs.push(ref);
  }

  static endAllIntervals(){
    this.refs.forEach(ref => {
      clearInterval(ref);
    });
  }

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
    localStorage.setItem("username", "");
    localStorage.setItem("password", "");
  }

  static setRole(role:string){
    localStorage.setItem("role", role);
  }

  static getRole():string{
    var role = "";
    if(this.isUserLogged()){
      var decoded =  Globals.getDecodedAccessToken();
      if(decoded)
        role = Globals.getDecodedAccessToken()["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];  
    }
    return role;
  }

  static getDecodedAccessToken(): any {
    const jwtService = new JwtHelperService();
    var token = Globals.getToken();
    try{
        return jwtService.decodeToken(token);
    }
    catch(Error){
        return null;
    }
  }
}
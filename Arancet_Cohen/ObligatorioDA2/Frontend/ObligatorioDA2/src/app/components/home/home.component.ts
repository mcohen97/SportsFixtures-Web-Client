import { Component, ViewChild } from '@angular/core';
import {AuthService} from './../../services/auth/auth.service';
import { LoginComponent } from './../../components/login/login.component';
import {MatSidenav} from '@angular/material/sidenav';
import { Globals } from 'src/app/globals';


@Component({
  selector: 'home-view',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  viewProviders: [LoginComponent],
  providers: [AuthService]
})
export class HomeComponent {
  @ViewChild('sidenav') sidenav:MatSidenav;
  title = 'Sporture';

  constructor(){
  }

  getLogged():string{
    if(Globals.isUserLogged())
      return Globals.getUsername();
    else
      return "Log in";
  }

  close(){
    this.sidenav.close();
  }

  logOut(){
    Globals.logOut();
    this.getLogged();
  }

  isUserLogged():boolean{
    return Globals.isUserLogged();
  }

  checkRole(role:string):boolean{
    return Globals.getRole() == role;
  }
}
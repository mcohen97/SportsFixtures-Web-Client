import { Component, OnInit } from '@angular/core';
import { Globals } from 'src/app/globals';

@Component({
  selector: 'app-welcome',
  templateUrl: './welcome.component.html',
  styleUrls: ['./welcome.component.css']
})
export class WelcomeComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

  getUsername():string{
    return Globals.getUsername();
  }

  checkRole(role:string):boolean{
    return Globals.getRole() == role;
  }
}

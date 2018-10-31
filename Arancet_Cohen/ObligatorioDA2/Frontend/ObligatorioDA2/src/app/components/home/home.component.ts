import { Component, ViewChild } from '@angular/core';
import {AuthService} from './../../services/auth/auth.service';
import { LoginComponent } from './../../components/login/login.component';
import {MatSidenav} from '@angular/material/sidenav';


@Component({
  selector: 'home-view',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  viewProviders: [LoginComponent],
  providers: [AuthService]
})
export class HomeComponent {
  @ViewChild('sidenav') sidenav:MatSidenav;
  title = 'Sportlendar';

  close(){
    this.sidenav.close();
  }

}

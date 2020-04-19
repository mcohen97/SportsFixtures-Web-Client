import { Component } from '@angular/core';
import {AuthService} from './services/auth/auth.service';
import { LoginComponent } from './components/login/login.component';
import{HomeComponent} from './components/home/home.component'

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  viewProviders: [
    LoginComponent,
    HomeComponent
  ],
  providers: [AuthService]
})
export class AppComponent {
  title = 'Sportlendar';
}

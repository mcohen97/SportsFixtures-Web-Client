import { Component } from '@angular/core';
import {LoginService} from './services/login/login.service';
import { LoginComponent } from './components/login/login.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  viewProviders: [LoginComponent],
  providers: [LoginService]
})
export class AppComponent {
  title = 'Sportlendar';
}

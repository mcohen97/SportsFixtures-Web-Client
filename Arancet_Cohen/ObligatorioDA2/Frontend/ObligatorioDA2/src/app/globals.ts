import { Injectable } from '@angular/core';
import { User } from './classes/user';

@Injectable()
export class Globals {
  token: string;
  loggedUser: User;  
  WEB_API_URL : string = 'https://localhost:5001/api'; 

}
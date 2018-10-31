import { Injectable } from '@angular/core';
import { User } from './classes/user';

@Injectable()
export class Globals {
  token: string;
  loggedUser: User;  
}
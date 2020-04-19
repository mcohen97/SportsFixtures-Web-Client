import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { AuthService } from './auth.service';
import { Globals } from 'src/app/globals';

@Injectable()
export class CanActivateViaAuthGuard implements CanActivate {

  constructor() {}

  canActivate() {
    return Globals.getRole() == "Admin";
  }
}
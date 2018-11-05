import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import { AppComponent } from './app.component';
import {LoginComponent} from './components/login/login.component';
import {RouterModule} from '@angular/router';
import { ConfirmationDialogComponent } from './components/confirmation-dialog/confirmation-dialog'


import {
  MatAutocompleteModule,
  MatBadgeModule,
  MatBottomSheetModule,
  MatButtonModule,
  MatButtonToggleModule,
  MatCardModule,
  MatCheckboxModule,
  MatChipsModule,
  MatDatepickerModule,
  MatDialogModule,
  MatDividerModule,
  MatExpansionModule,
  MatGridListModule,
  MatIconModule,
  MatInputModule,
  MatListModule,
  MatMenuModule,
  MatNativeDateModule,
  MatPaginatorModule,
  MatProgressBarModule,
  MatProgressSpinnerModule,
  MatRadioModule,
  MatRippleModule,
  MatSelectModule,
  MatSidenavModule,
  MatSliderModule,
  MatSlideToggleModule,
  MatSnackBarModule,
  MatSortModule,
  MatStepperModule,
  MatTableModule,
  MatTabsModule,
  MatToolbarModule,
  MatTooltipModule,
  MatTreeModule,
  ErrorStateMatcher,
  ShowOnDirtyErrorStateMatcher,
} from '@angular/material';
import { AuthService } from './services/auth/auth.service';
import {HomeComponent}from'./components/home/home.component';
import { Http, HttpModule } from '@angular/http';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { Globals } from './globals';
import { UserInfoComponent } from './components/user-info/user-info.component';
import { UsersService } from './services/users/users.service';
import { UsersComponent } from './components/users/users.component';
import { UserEditDialogComponent } from './components/users/user-edit-dialog/user-edit-dialog';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { User } from './classes/user';
import { UserDialogComponent } from './components/users/user-dialog/user-dialog.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent,
    NotFoundComponent,
    UserInfoComponent,
    UsersComponent,
    UserEditDialogComponent,
    ConfirmationDialogComponent,
    UserDialogComponent
  ],
  imports: [
    RouterModule.forRoot([
      {
        path: 'login',
        component: LoginComponent
      },
      {
        path:'users',
        component:UsersComponent
      },
      {
        path: '**',
        component: NotFoundComponent
      }
    ]),
    BrowserModule,
    BrowserAnimationsModule,
    MatAutocompleteModule,
    HttpModule,
    MatBadgeModule,
    MatBottomSheetModule,
    MatButtonModule,
    MatButtonToggleModule,
    MatCardModule,
    MatCheckboxModule,
    MatChipsModule,
    MatDatepickerModule,
    MatDialogModule,
    MatDividerModule,
    MatExpansionModule,
    MatGridListModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatNativeDateModule,
    MatPaginatorModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MatRadioModule,
    MatRippleModule,
    MatSelectModule,
    MatSidenavModule,
    MatPaginatorModule,
    MatSliderModule,
    MatSlideToggleModule,
    MatSnackBarModule,
    MatSortModule,
    MatStepperModule,
    MatTableModule,
    MatTabsModule,
    MatToolbarModule,
    MatTooltipModule,
    MatTreeModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [
    Globals,
    AuthService,
    UsersService,
    {provide: ErrorStateMatcher, useClass: ShowOnDirtyErrorStateMatcher}

  ],
  entryComponents: [
    UserEditDialogComponent, 
    ConfirmationDialogComponent,
    UserDialogComponent,
  ],
  bootstrap: [AppComponent],
  
})
export class AppModule { }

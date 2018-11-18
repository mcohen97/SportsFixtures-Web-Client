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
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { User } from './classes/user';
import { UserDialogComponent } from './components/users/user-dialog/user-dialog.component';
import { ReConnector } from './services/auth/reconnector';
import { SportsComponent } from './components/sports/sports.component';
import { SportDialogComponent } from './components/sports/sport-dialog/sport-dialog.component';
import { SportsService } from './services/sports/sports.service';
import { TeamsComponent } from './components/teams/teams.component';
import { TeamDialogComponent } from './components/teams/team-dialog/team-dialog.component';
import { TeamsService } from './services/teams/teams.service';
import { EncountersComponent } from './components/encounters/encounters.component';
import { EncounterDialogComponent } from './components/encounters/encounter-dialog/encounter-dialog.component';
import { EncounterCardComponent } from './components/encounters/encounter-card/encounter-card.component';
import { EncounterCardDeckComponent } from './components/encounters/encounter-card-deck/encounter-card-deck.component';
import { EncountersService } from './services/encounters/encounters.service';
import { EncounterResultDialogComponent } from './components/encounters/encounter-result-dialog/encounter-result-dialog.component';
import { FixturesService } from './services/fixtures/fixture.service';
import { LogsComponent } from './components/logs/logs.component';
import { LogsService } from './services/logs/logs.service';
import { FollowTeamDialogComponent } from './components/teams-follower/follow-team-dialog/follow-team-dialog.component';
import { TeamsFollowerComponent } from './components/teams-follower/teams-follower.component';
import { CalendarComponent } from './components/calendar/calendar.component';
import { EventCardComponent } from './components/calendar/event-card/event-card.component';
import { EventDeckComponent } from './components/calendar/event-deck/event-deck.component';
import { CommentDialogComponent } from './components/comments/comment-dialog/comment-dialog.component';
import { CommentsComponent } from './components/comments/comments.component';
import { CommentsService } from './services/comments/comments.service';
import { CommentsTableComponent } from './components/comments/comments-table/comments-table.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent,
    NotFoundComponent,
    UserInfoComponent,
    UsersComponent,
    ConfirmationDialogComponent,
    UserDialogComponent,
    SportsComponent,
    SportDialogComponent,
    TeamsComponent,
    TeamDialogComponent,
    EncountersComponent,
    EncounterDialogComponent,
    EncounterCardComponent,
    EncounterCardDeckComponent,
    EncounterResultDialogComponent,
    LogsComponent,
    FollowTeamDialogComponent,
    TeamsFollowerComponent,
    CalendarComponent,
    EventCardComponent,
    EventDeckComponent,
    CommentDialogComponent,
    CommentsComponent,
    CommentsTableComponent
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
        path:'sports',
        component:SportsComponent
      },
      {
        path:'teams',
        component:TeamsComponent
      },
      {
        path:'encounters',
        component:EncountersComponent
      },
      {
        path:'logs',
        component:LogsComponent
      },
      {
        path:'follower-teams',
        component:TeamsFollowerComponent
      },
      {
        path:'calendar',
        component:CalendarComponent
      },
      {
        path:'comments',
        component:CommentsComponent
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
    {provide: Globals, useClass:Globals},
    {provide: AuthService, useClass:AuthService},
    {provide: UsersService, useClass:UsersService},
    {provide: SportsService, useClass:SportsService},
    {provide: TeamsService, useClass:TeamsService},
    {provide: EncountersService, useClass:EncountersService},
    {provide: FixturesService, useClass:FixturesService},
    {provide: LogsService, useClass:LogsService},
    {provide: CommentsService, useClass:CommentsService},
    {provide: ReConnector, useClass:ReConnector},
    {provide: ErrorStateMatcher, useClass: ShowOnDirtyErrorStateMatcher}
  ],
  entryComponents: [
    ConfirmationDialogComponent,
    UserDialogComponent,
    SportDialogComponent,
    TeamDialogComponent,
    EncounterDialogComponent,
    EncounterResultDialogComponent,
    FollowTeamDialogComponent,
    CommentDialogComponent
  ],
  bootstrap: [AppComponent],
  
})
export class AppModule { }

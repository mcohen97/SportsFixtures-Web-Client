import { Component, OnInit, Input } from '@angular/core';
import { Encounter } from 'src/app/classes/encounter';
import { EventEncounters } from 'src/app/classes/event-encounter';


@Component({
  selector: 'event-deck',
  templateUrl: './event-deck.component.html',
  styleUrls: ['./event-deck.component.css']
})
export class EventDeckComponent implements OnInit {

  @Input() encounters:Array<Encounter>;
  @Input() date:Date;
  @Input() sport:string;
  
  encountersCalendar:EventEncounters[][];

  constructor() {
  }

  ngOnInit() {
    this.encountersCalendar = this.generateCalendarOfEncounters(this.encounters);
  }

  generateCalendarOfEncounters(encounters:Array<Encounter>):EventEncounters[][]{
    var generatedCalendarbyDay = [[]];
    if(encounters){
      encounters.forEach(encounter => {
        var date = new Date(encounter.date);
        var day = date.getDate();
        var month = date.getMonth();
        if(!generatedCalendarbyDay[month])
          generatedCalendarbyDay[month] = new Array<EventEncounters>();
        if(!generatedCalendarbyDay[month][day]){
          generatedCalendarbyDay[month][day] = new EventEncounters();
          generatedCalendarbyDay[month][day].encounters = new Array<Encounter>();
        }
        generatedCalendarbyDay[month][day].date = date;
        generatedCalendarbyDay[month][day].encounters.push(encounter);
      });    
    }
    return generatedCalendarbyDay;
  }

  existEncounter():boolean{
    return this.encountersCalendar.some(encounterList => encounterList.some(e => e.date.getMonth() == this.date.getMonth() && e.date.getFullYear() == this.date.getFullYear()));
  }
}

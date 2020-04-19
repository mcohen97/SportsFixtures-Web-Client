import { Component, OnInit, Input } from '@angular/core';
import { Team } from 'src/app/classes/team';
import { Encounter } from 'src/app/classes/encounter';
import { EventEncounters } from 'src/app/classes/event-encounter';
import { MatDialog } from '@angular/material';

@Component({
  selector: 'event-card',
  templateUrl: './event-card.component.html',
  styleUrls: ['./event-card.component.css']
})
export class EventCardComponent implements OnInit {

  @Input() event: EventEncounters;
  @Input() date:Date;
  @Input() encounterCount:number;
  toolTipInfo:string

  constructor(private dialog:MatDialog) { 
  }

  ngOnInit() {
   this.toolTipInfo = this.generateTooltipInfo();
  }

  generateTooltipInfo():string{
    var result = "";
    result += this.date.getDate() + "/" + (this.date.getMonth()+1)+ "/"+this.date.getFullYear();
    if(this.event && this.event.enocunters){
      this.event.enocunters.forEach(encounter => {
        result += "Encounter: " + encounter.teams.toString();
        result += "Has result: " + encounter.hasResult;
      });
    }
    return result;
  }

}

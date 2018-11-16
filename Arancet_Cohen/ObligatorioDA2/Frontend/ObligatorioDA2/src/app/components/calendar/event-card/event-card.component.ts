import { Component, OnInit, Input } from '@angular/core';
import { Team } from 'src/app/classes/team';
import { Encounter } from 'src/app/classes/encounter';

@Component({
  selector: 'app-event-card',
  templateUrl: './event-card.component.html',
  styleUrls: ['./event-card.component.css']
})
export class EventCardComponent implements OnInit {

  @Input() encounters: Array<Encounter>;
  @Input() date: Date;

  constructor() { }

  ngOnInit() {
  }

}

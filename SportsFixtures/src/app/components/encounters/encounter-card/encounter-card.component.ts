import { Component, OnInit, Input } from '@angular/core';
import { Team } from 'src/app/classes/team';
import { Encounter } from 'src/app/classes/encounter';

@Component({
  selector: 'encounter-card',
  templateUrl: './encounter-card.component.html',
  styleUrls: ['./encounter-card.component.css']
})
export class EncounterCardComponent implements OnInit {

  @Input() encounter:Encounter;
  
  constructor() { }

  ngOnInit() {
  }

}

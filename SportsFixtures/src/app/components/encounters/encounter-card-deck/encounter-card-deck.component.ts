import { Component, OnInit, Input } from '@angular/core';
import { Encounter } from 'src/app/classes/encounter';

@Component({
  selector: 'encounter-card-deck',
  templateUrl: './encounter-card-deck.component.html',
  styleUrls: ['./encounter-card-deck.component.css']
})
export class EncounterCardDeckComponent implements OnInit {

  @Input() encounters: Array<Encounter>;

  constructor() { }

  ngOnInit() {
  }

}

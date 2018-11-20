import { Component, OnInit, Input } from '@angular/core';
import { Encounter } from 'src/app/classes/encounter';

@Component({
  selector: 'app-encounter-card-deck',
  templateUrl: './encounter-card-deck.component.html',
  styleUrls: ['./encounter-card-deck.component.css']
})
export class EncounterCardDeckComponent implements OnInit {

  @Input() encounters: Array<Encounter>;
  @Input() sport: string;
  @Input() month: number;
  @Input() year: number;

  constructor() { }

  ngOnInit() {
  }

}

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EncounterCardDeckComponent } from './encounter-card-deck.component';

describe('EncounterCardDeckComponent', () => {
  let component: EncounterCardDeckComponent;
  let fixture: ComponentFixture<EncounterCardDeckComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EncounterCardDeckComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EncounterCardDeckComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

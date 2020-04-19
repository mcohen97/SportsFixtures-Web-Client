import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EventDeckComponent } from './event-deck.component';

describe('EventDeckComponent', () => {
  let component: EventDeckComponent;
  let fixture: ComponentFixture<EventDeckComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EventDeckComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EventDeckComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

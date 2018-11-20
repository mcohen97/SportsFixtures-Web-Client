import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EncounterDialogComponent } from './encounter-dialog.component';

describe('EncounterDialogComponent', () => {
  let component: EncounterDialogComponent;
  let fixture: ComponentFixture<EncounterDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EncounterDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EncounterDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

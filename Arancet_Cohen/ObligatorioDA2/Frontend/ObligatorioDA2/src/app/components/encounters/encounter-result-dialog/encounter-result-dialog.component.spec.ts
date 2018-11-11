import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EncounterResultDialogComponent } from './encounter-result-dialog.component';

describe('EncounterResultDialogComponent', () => {
  let component: EncounterResultDialogComponent;
  let fixture: ComponentFixture<EncounterResultDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EncounterResultDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EncounterResultDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

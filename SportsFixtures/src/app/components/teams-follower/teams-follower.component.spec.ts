import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TeamsFollowerComponent } from './teams-follower.component';

describe('TeamsFollowerComponent', () => {
  let component: TeamsFollowerComponent;
  let fixture: ComponentFixture<TeamsFollowerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TeamsFollowerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TeamsFollowerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

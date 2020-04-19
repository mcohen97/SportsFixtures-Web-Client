import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FollowTeamDialogComponent } from './follow-team-dialog.component';

describe('FollowTeamDialogComponent', () => {
  let component: FollowTeamDialogComponent;
  let fixture: ComponentFixture<FollowTeamDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FollowTeamDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FollowTeamDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

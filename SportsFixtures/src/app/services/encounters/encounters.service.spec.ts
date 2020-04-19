import { TestBed } from '@angular/core/testing';

import { EncountersService } from './encounters.service';

describe('EncountersService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: EncountersService = TestBed.get(EncountersService);
    expect(service).toBeTruthy();
  });
});

import { TestBed } from '@angular/core/testing';

import { FriendsService as FriendsService } from './friends.service';

describe('FriendsServiceService', () => {
  let service: FriendsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FriendsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

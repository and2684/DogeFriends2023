import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DogsPopupComponent } from './dogs-popup.component';

describe('DogsPopupComponent', () => {
  let component: DogsPopupComponent;
  let fixture: ComponentFixture<DogsPopupComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DogsPopupComponent]
    });
    fixture = TestBed.createComponent(DogsPopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

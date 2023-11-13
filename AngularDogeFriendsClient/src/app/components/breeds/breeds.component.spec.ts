/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { BreedsComponent } from './breeds.component';

describe('BreedsComponent', () => {
  let component: BreedsComponent;
  let fixture: ComponentFixture<BreedsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BreedsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BreedsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

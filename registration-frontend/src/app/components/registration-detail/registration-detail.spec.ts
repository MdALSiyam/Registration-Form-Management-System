import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrationDetail } from './registration-detail';

describe('RegistrationDetail', () => {
  let component: RegistrationDetail;
  let fixture: ComponentFixture<RegistrationDetail>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RegistrationDetail]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RegistrationDetail);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

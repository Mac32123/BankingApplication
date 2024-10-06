import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LiignComponent } from './liign.component';

describe('LiignComponent', () => {
  let component: LiignComponent;
  let fixture: ComponentFixture<LiignComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [LiignComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(LiignComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

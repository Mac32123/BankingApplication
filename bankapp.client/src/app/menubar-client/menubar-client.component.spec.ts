import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MenubarClientComponent } from './menubar-client.component';

describe('MenubarClientComponent', () => {
  let component: MenubarClientComponent;
  let fixture: ComponentFixture<MenubarClientComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [MenubarClientComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(MenubarClientComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

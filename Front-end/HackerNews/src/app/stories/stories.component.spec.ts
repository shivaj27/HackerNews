import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormsModule } from '@angular/forms';
import { StoriesComponent } from './stories.component';

describe('StoriesComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        StoriesComponent,
        HttpClientTestingModule,
        FormsModule
      ]
    }).compileComponents();
  });

  it('should create', () => {
    const fixture = TestBed.createComponent(StoriesComponent);
    const component = fixture.componentInstance;
    expect(component).toBeTruthy();
  });

  it('should search stories', () => {
    const fixture = TestBed.createComponent(StoriesComponent);
    const component = fixture.componentInstance;

    component.query = 'Angular';
    component.onSearch();

    expect(component.page).toBe(1);
  });

  it('should load stories on init', () => {
    const fixture = TestBed.createComponent(StoriesComponent);
    const component = fixture.componentInstance;
    fixture.detectChanges();

    expect(component.stories).toBeDefined();
  });
});



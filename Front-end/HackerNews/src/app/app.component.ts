import { Component } from '@angular/core';
import { StoriesComponent } from './stories/stories.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  standalone: true,
  imports: [StoriesComponent]
})
export class AppComponent {
  title = 'Hacker News';
}

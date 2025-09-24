import { Component, OnInit } from '@angular/core';
import { ApiService } from '../services/api.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-stories',
  templateUrl: './stories.component.html',
  styleUrls: ['./stories.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule]
})
export class StoriesComponent implements OnInit {
  stories: any[] = [];
  page = 1;
  pageSize = 10;
  query = '';
  total = 0;
  loading = false;

  constructor(private api: ApiService) {}

  ngOnInit() {
    this.loadStories();
  }

  loadStories() {
  this.loading = true;

  const obs = this.query
    ? this.api.search(this.query, this.page, this.pageSize)
    : this.api.getNewest(this.page, this.pageSize);

  obs.subscribe({
    next: (result) => {
      this.stories = result.items;
      this.total = result.total;
      this.loading = false;
    },
    error: () => {
      this.loading = false;
    }
  });
}


  onSearch() {
    this.page = 1;
    this.loadStories();
  }

  nextPage() {
    if (this.page * this.pageSize < this.total) {
      this.page++;
      this.loadStories();
    }
  }

  prevPage() {
    if (this.page > 1) {
      this.page--;
      this.loadStories();
    }
  }
}

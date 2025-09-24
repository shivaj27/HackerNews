import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private baseUrl = 'http://localhost:5003';

  constructor(private http: HttpClient) {}

  getNewest(page: number, pageSize: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/api/hackernews/newest?page=${page}&pageSize=${pageSize}`);
  }

  search(query: string, page: number, pageSize: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/api/hackernews/search?query=${query}&page=${page}&pageSize=${pageSize}`);
  }
}

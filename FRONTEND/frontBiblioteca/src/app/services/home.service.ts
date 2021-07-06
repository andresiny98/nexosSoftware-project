import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class HomeService {
  url: string = 'https://localhost:44397/api/';

  constructor(private http: HttpClient) {}

  getAllLibro() {
    return this.http.get(this.url + 'libros');
  }

  getFilterLibro(termino: string) {
    return this.http.get(this.url + 'libros/find/' + termino);
  }
}

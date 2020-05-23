import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OfficesService {

  private offices;

  constructor(private http: HttpClient) { }

  // Gets JSON object with offices
  public getOffices(): Observable<JSON> {
    const url = 'https://4bdc2cf2-5cd5-4837-8dbc-eb1a53e55115.mock.pstmn.io/offices';
    this.offices = this.http.get(url);
    return this.offices;
  }
}

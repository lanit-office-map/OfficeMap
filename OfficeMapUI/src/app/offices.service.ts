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
    const url = 'https://dce9e984-09b1-4435-b433-638826db5d31.mock.pstmn.io/offices';
    this.offices = this.http.get(url);
    return this.offices;
  }
}

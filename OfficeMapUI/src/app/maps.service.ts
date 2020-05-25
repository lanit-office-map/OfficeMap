import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MapsService {

  private maps;
  private mapsUrl: string;

  constructor(private http: HttpClient) { }
  public getMaps(id: number): Observable<JSON> {
    this.mapsUrl = 'https://dce9e984-09b1-4435-b433-638826db5d31.mock.pstmn.io/map/' + id;
    console.log(this.mapsUrl);
    this.maps = this.http.get(this.mapsUrl);
    return this.maps;
  }
}

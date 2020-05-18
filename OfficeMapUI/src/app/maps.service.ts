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
    this.mapsUrl = 'https://aeb8e870-6e43-45e6-a165-00dfa6ce609c.mock.pstmn.io/map/' + id;
    console.log(this.mapsUrl);
    this.maps = this.http.get(this.mapsUrl);
    return this.maps;
  }
}

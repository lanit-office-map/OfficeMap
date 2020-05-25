import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MapNameService {

  private mapNameSource = new BehaviorSubject('Карта не выбрана');
  public mapName = this.mapNameSource.asObservable();

  constructor() { }

  // Passes map's name to subscribers
  public passMapName(mapName: string): void {
    this.mapNameSource.next(mapName);
  }
}

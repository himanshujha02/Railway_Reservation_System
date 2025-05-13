import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Station } from '../Station.model';

@Injectable({ providedIn: 'root' })
export class StationService {
  private apiUrl = 'http://localhost:5206/api/Station';

  constructor(private http: HttpClient) {}

  getAllStations(): Observable<Station[]> {
    return this.http.get<Station[]>(this.apiUrl);
  }

  addStation(station: Station): Observable<any> {
    return this.http.post(this.apiUrl, station);
  }
}

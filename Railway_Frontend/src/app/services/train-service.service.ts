import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TrainSearchDto, TrainSearchResultDto } from '../train-search.model';
import { TrainDto, TrainListDto, UpdateTrainAdminMessage } from '../Train.Model';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class TrainService {
  private baseUrl = 'http://localhost:5206/api/TrainSearch'; // Update accordingly

  constructor(private http: HttpClient, private authService:AuthService) {}

  private getAuthHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  searchTrains(searchData: TrainSearchDto): Observable<TrainSearchResultDto[]> {
    return this.http.post<TrainSearchResultDto[]>(`${this.baseUrl}/search`, searchData);
  }

  addTrain(trainDto: TrainDto): Observable<any> {
    return this.http.post(`${this.baseUrl}/add`, trainDto, { headers :this.getAuthHeaders()});
  }

  getAllTrains(): Observable<TrainListDto[]> {
    return this.http.get<TrainListDto[]>(`${this.baseUrl}`,{ headers :this.getAuthHeaders()});
  }

  getTrainById(trainId: number): Observable<TrainDto> {
    return this.http.get<TrainDto>(`${this.baseUrl}/${trainId}`,{ headers :this.getAuthHeaders()});
  }

  updateTrain(trainId: number, trainDto: UpdateTrainAdminMessage): Observable<any> {
    return this.http.put(`${this.baseUrl}/update/${trainId}`, trainDto, { headers :this.getAuthHeaders()});
  }
}

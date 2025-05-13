import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';


@Injectable({ providedIn: 'root' })
export class AuthService {
  private api = 'http://localhost:5206/api';

  constructor(private http: HttpClient) {}

  register(data: any) {
    return this.http.post(`${this.api}/Auth/register`, data);
  }

  login(data: any) {
    return this.http.post(`${this.api}/Auth/login`, data);
  }

  decodeToken(token: string): any {
    try {
      
      return jwtDecode(token);
    } catch (error) {
      console.error('Token decoding failed:', error);
      return null;
    }
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  getUserRole(): string | null {
    const token = this.getToken();
    if (!token) return null;
  
    const decoded = this.decodeToken(token);
    let role = decoded['role'];
          if (!role) {
            // sometimes role is saved inside this key
            role = decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
          }
          if(role==='Admin'){
            return 'Admin';
          }
          else if(role=='User'){
            return 'User';
          }
          else{
            return null;
          }
  }
  

  isLoggedIn(): boolean {
    const token = this.getToken();
    console.log(token);
    if (!token) return false;
    return true}
}

import { Component } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginData = { username: '', password: '' };

  constructor(private authService: AuthService, private router: Router) {}

  login() {
    this.authService.login(this.loginData).subscribe({
      next: (res: any) => {
        if (res.token) {
          localStorage.setItem('token', res.token);

          const decoded = this.authService.decodeToken(res.token);
          console.log('Decoded token:', decoded);

          let role = decoded['role'];
          if (!role) {
            // sometimes role is saved inside this key
            role = decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
          }
          if (role === 'Admin') {
            this.router.navigate(['/admin']);
          } else if (role === 'User') {
            this.router.navigate(['/user']);
          } else {
            alert('Unknown role!');
          }
        } else {
          alert('No token received.');
        }
      },
      error: (error) => {
        console.error('Login error:', error);
        alert('Invalid username or password. Please try again.');
      }
    });
  }
  

  goToRegister() {
    this.router.navigate(['/register']);
  }
}

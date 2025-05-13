import { Component } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  registerData = {
    username: '',
    email: '',
    password: '',
    role: 'User',
    aadharNumber: '',
    admincode: ''
  };

  constructor(private authService: AuthService, private router: Router) {}

  register() {
    this.authService.register(this.registerData).subscribe({
      next: () => {
        alert('Registration successful! Please login.');
        this.router.navigate(['/login']);
      },
      error: (error) => {
        console.error("Registration error: ", error);
        if (error.error && error.error.errors) {
          alert('Registration failed: ' + error.error.errors.join(', '));
        } else {
          alert('Registration failed. Please try again.');
        }
      }
    });
  }

  goToLogin() {
    this.router.navigate(['/login']);
  }
} 
import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { UserComponent } from './user/user.component';
import { AdminComponent } from './admin/admin.component';
import { TrainSearchComponent } from './train-search/train-search.component';
import { BookTicketComponent } from './book-ticket/book-ticket.component';
import { ShowTicketComponent } from './show-ticket/show-ticket.component';
import { MyBookingsComponent } from './my-bookings/my-bookings.component';
import { AddTrainComponent } from './add-train/add-train.component';
import { UpdateTrainComponent } from './update-train/update-train.component';
import { StationComponent } from './station/station.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'user', component: UserComponent },
  { path: 'admin', component: AdminComponent },
  { path: 'train-search', component: TrainSearchComponent },
  {path:'book-ticket',component:BookTicketComponent},
  {path: 'show-ticket',component: ShowTicketComponent},
  {path:'my-bookings',component:MyBookingsComponent},
  {path:'add-train',component:AddTrainComponent},
  {path:'update-train',component:UpdateTrainComponent},
  {path:'station',component:StationComponent},
  { path: '**', redirectTo: 'login',pathMatch: 'full' }
];

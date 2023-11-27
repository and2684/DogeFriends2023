import { NgModule, Component } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BaseComponent } from './components/base/base.component';
import { BreedsComponent } from './components/breeds/breeds.component';
import { BreedDetailsComponent } from './components/breed-details/breed-details.component';
import { DogsComponent } from './components/dogs/dogs.component';
import { FriendsComponent } from './components/friends/friends.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { UserComponent } from './components/user/user.component';


const routes: Routes = [
  { path: '', component: BaseComponent },
  { path: 'breeds', component: BreedsComponent },
  { path: 'breed-details/:id', component: BreedDetailsComponent },
  { path: 'dogs', component: DogsComponent },
  { path: 'friends', component: FriendsComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'user/:username', component: UserComponent },
  { path: '**', redirectTo: "", component: BaseComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

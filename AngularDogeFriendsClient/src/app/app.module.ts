import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { MatSidenavModule} from '@angular/material/sidenav';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatDividerModule } from '@angular/material/divider';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { HttpClientModule } from '@angular/common/http';
import { MatDialogModule } from '@angular/material/dialog';
import { NgImageSliderModule } from 'ng-image-slider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { RegisterComponent } from './components/register/register.component';
import { LoginComponent } from './components/login/login.component';
import { UserComponent } from './components/user/user.component';
import { LeftPanelComponent } from './components/UI/left-panel/left-panel.component';
import { BaseComponent } from './components/base/base.component';
import { BreedsComponent } from './components/breeds/breeds.component';
import { BreedDetailsComponent } from './components/breed-details/breed-details.component';
import { FriendsComponent } from './components/friends/friends.component';

@NgModule({
  declarations: [
    AppComponent,
    UserComponent,
    LeftPanelComponent,
    BaseComponent,
    BreedsComponent,
    BreedDetailsComponent,
    FriendsComponent,
    RegisterComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatSidenavModule,
    MatToolbarModule,
    MatIconModule,
    MatListModule,
    MatDividerModule,
    MatButtonModule,
    MatCardModule,
    HttpClientModule,
    MatDialogModule,
    NgImageSliderModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

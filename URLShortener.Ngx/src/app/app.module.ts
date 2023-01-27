import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDialogModule } from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatCardModule } from '@angular/material/card';

import { environment } from 'src/environments/environment';
import { API_BASE_URL } from './api.clients';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AddUrlComponent } from './add-url/add-url.component';
import { ListUrlComponent } from './list-url/list-url.component';
import { TruncatePipe } from './truncate.pipe';
import { MomentPipe } from './moment.pipe';

@NgModule({
  declarations: [
    MomentPipe,
    TruncatePipe,
    AppComponent,
    AddUrlComponent,
    ListUrlComponent,
  ],
  imports: [
    HttpClientModule,
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSnackBarModule,
    MatDialogModule,
    MatTableModule,
    MatTooltipModule,
    MatPaginatorModule,
    MatIconModule,
    MatToolbarModule,
    MatCardModule,
  ],
  providers: [
    {
      provide: API_BASE_URL,
      useFactory: getBaseUrl,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}

export function getBaseUrl(): string {
  return environment.api;
}

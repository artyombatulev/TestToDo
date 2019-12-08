import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { TodoComponent } from './todo/todo.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { AboutComponent } from './about/about.component';
import { PointsModule } from './point/points.module';
import { TodoService } from './shared/todo.service';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { TodoPointService } from './shared/todoPoint.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatIconModule } from '@angular/material/icon';
import { DatePipe } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { ToastrModule } from 'ngx-toastr';
import { MatDialogModule } from '@angular/material/dialog';
import { DialogBodyComponent } from './dialog-body/dialog-body.component';
import { MatButtonModule } from '@angular/material/button';
import { MatRadioModule, MatTableModule, MatCheckboxModule, MatSortModule, MatFormFieldModule, MatInputModule, MatPaginatorModule } from '@angular/material';

@NgModule({
    declarations: [
        AppComponent,
        TodoComponent,
        PageNotFoundComponent,
        AboutComponent,
        DialogBodyComponent
    ],
    imports: [
        BrowserModule, PointsModule, AppRoutingModule, NgbModule, FormsModule,
        HttpClientModule, BrowserAnimationsModule, MatIconModule, ReactiveFormsModule,
        ToastrModule.forRoot(), MatDialogModule, MatButtonModule, MatRadioModule, MatTableModule, MatCheckboxModule,
        MatSortModule, MatFormFieldModule, MatInputModule, MatPaginatorModule
    ],
    providers: [TodoService, TodoPointService, DatePipe],
    bootstrap: [AppComponent],
    entryComponents: [DialogBodyComponent]
})
export class AppModule { }

import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { PointsRoutingModule } from './points-routing.module';
import { PointComponent } from './point/point.component';
import { PointDetailsComponent } from './point-details/point-details.component';
import { MatIconModule } from '@angular/material/icon';
import { ToastrModule } from 'ngx-toastr';


@NgModule({
  declarations: [PointComponent, PointDetailsComponent],
  imports: [
    CommonModule,
    PointsRoutingModule,
      FormsModule,
      MatIconModule, ReactiveFormsModule, ToastrModule.forRoot(),
  ],
  providers: [DatePipe]
})
export class PointsModule { }

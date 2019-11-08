import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PointComponent } from 'src/app/point/point/point.component';
import { PointDetailsComponent} from 'src/app/point/point-details/point-details.component'


const routes: Routes = [
    { path: 'point', component: PointComponent },
    { path: 'point/:id', component: PointDetailsComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PointsRoutingModule { }

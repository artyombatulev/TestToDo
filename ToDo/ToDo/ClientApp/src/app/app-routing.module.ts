import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TodoComponent } from 'src/app/todo/todo.component';
import { AboutComponent } from 'src/app/about/about.component';
import { PageNotFoundComponent } from 'src/app/page-not-found/page-not-found.component';

//import { PointComponent } from 'src/app/point/point/point.component';
//import { PointDetailsComponent } from 'src/app/point/point-details/point-details.component'


const routes: Routes = [
    { path: 'todo', component: TodoComponent },
    { path: 'about', component: AboutComponent },
    { path: '', redirectTo: '/todo', pathMatch: 'full' },
    { path: '**', component: PageNotFoundComponent },
   // { path: 'point', component: PointComponent },
    //{ path: 'point/:id', component: PointDetailsComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TodoComponent } from 'src/app/todo/todo.component';
import { AboutComponent } from 'src/app/about/about.component';
import { PageNotFoundComponent } from 'src/app/page-not-found/page-not-found.component';


const routes: Routes = [
    { path: 'todo', component: TodoComponent },
    //{ path: 'about', component: AboutComponent },
    { path: '', redirectTo: '/todo', pathMatch: 'full' },
    { path: '**', component: PageNotFoundComponent }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }

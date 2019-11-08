import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Todo } from 'src/app/shared/todo.model';
import { TodoService } from 'src/app/shared/todo.service';
import { TodoPoint } from 'src/app/shared/todoPoint.model';
import { TodoPointService } from 'src/app/shared/todoPoint.service';
import { Router } from '@angular/router';

import { MatIconModule } from '@angular/material/icon';
import { DatePipe } from '@angular/common';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-point-details',
  templateUrl: './point-details.component.html',
  styleUrls: ['./point-details.component.css'],
  providers: [TodoService, TodoPointService]
})
export class PointDetailsComponent implements OnInit {
    id: number;
    pointsId: number[];
    todo: Todo = new Todo();
    point: TodoPoint = new TodoPoint();
    points: TodoPoint[];

    tableMode: boolean = true;        
    filtre: string = "All"

    constructor(private activatedRoute: ActivatedRoute, private pointService: TodoPointService,
        private todoService: TodoService,
        public datepipe: DatePipe,
        private toastr: ToastrService) {
        this.id = this.activatedRoute.snapshot.params.id;
    }

    ngOnInit() {
        this.loadPoints(this.filtre);
        //this.checkCompletedPoints();
        this.loadTodo(this.id);
    }
    loadPoints(f) {
        //this.todoService.getTodos()
        //.subscribe((data: Todo[]) => this.todos = data);
        this.pointService.getPoints(this.id).toPromise().then(res => {
            this.points = res as TodoPoint[]
            this.filterPoints(this.filtre)
        });


    }
    loadTodo(id: number) {
        this.todoService.getTodo(id).toPromise().then(res => { this.todo = res as Todo });
    }
    
    // сохранение данных
    save() {
        if (this.point.description == '' || this.point.description == undefined) {
            this.toastr.error('Description field must be filled', 'Verify Your Form');
        }
        else {

            if (this.point.pointId == null) {
                this.pointService.createPoint(this.point, this.id)
                    .subscribe((data: TodoPoint) => this.points.push(data));
                this.toastr.success('Point Was Added Successfully', 'Point Adding');
            } else {
                this.pointService.updatePoint(this.point)
                    .subscribe(data => this.loadPoints(this.filtre));
                this.toastr.info('Point Was Updated successfully', 'Point Updating');
            }
            this.cancel();
        }
    }

    create() {
        this.pointService.createPoint(this.point, this.id);
    }

    editPoint(p: TodoPoint) {
        this.point = p;
    }

    cancel() {
        this.point = new TodoPoint();
        this.tableMode = true;
    }

    delete(p: TodoPoint) {
        this.pointService.deletePoint(p.pointId)
            .subscribe(data => this.loadPoints(this.filtre));
        this.toastr.info('Point Was Deleted successfully', 'Point Deleting');
    }


    clearAllPoints() {
        this.pointService.clearAllPoints(this.id);
    }

    add() {
        this.cancel();
        this.tableMode = false;
    }
    
    filterlist(fil: string) {
        this.filtre = fil;
        this.loadPoints(this.filtre)
    }


    filterPoints(f) {
        if (f == 'Active') {
            this.points = this.points.filter(x => x.isCompleted == false)
        }
        if (f == 'Completed') {
            this.points = this.points.filter(x => x.isCompleted)
        }
        if (f == 'All') {
            this.points = this.points;
        }
    }
    
    changeCompletedPoints(p: boolean) {
        if (p === true) {
            this.point.isCompleted = false;
        }
        if (p === false) {
            this.point.isCompleted = true;
        }
        
    }

    changeDateFormat(p: any) {
        //var moment = require('moment');
        //var date = moment(p.dateOfComplition).format("hh:mm a D MMM. YYYY");
       // return date;
        
            
        return  this.datepipe.transform(p, 'hh:mm a  dd/MM/yyy');
        
    }
    
}

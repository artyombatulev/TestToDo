import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Todo } from 'src/app/shared/todo.model';
import { TodoService } from 'src/app/shared/todo.service';
import { TodoPoint } from 'src/app/shared/todoPoint.model';
import { TodoPointService } from 'src/app/shared/todoPoint.service';

import { MatIconModule } from '@angular/material/icon';
import { DatePipe } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { MatDialogConfig, MatDialog, MatTableDataSource, MatSort, MatPaginator, MatRadioChange } from '@angular/material';
import { DialogBodyComponent } from '../../dialog-body/dialog-body.component';
import { take } from 'rxjs/operators';

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
    tableMode: boolean = true;
    createMode: boolean = true;

    checked: boolean = false;
    filtre: string = "All"

    public displayedColumns = ['description', 'dateOfComplition', 'isCompleted', 'update', 'delete'];
    public dataSource = new MatTableDataSource<TodoPoint>();

    @ViewChild(MatSort, { static: false }) sort: MatSort;
    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;

    constructor(private activatedRoute: ActivatedRoute, private pointService: TodoPointService,
        private todoService: TodoService,
        public datepipe: DatePipe,
        private toastr: ToastrService,
        public dialog: MatDialog) {
        this.id = this.activatedRoute.snapshot.params.id;
    }

    ngOnInit() {
        this.loadPoints();
    }

    ngAfterViewInit(): void {
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
    }

    loadPoints() {
        this.todoService.getTodo(this.id)
            .pipe(take(1))
            .subscribe(res => {
                this.todo = res as Todo;
                this.dataSource.data = this.todo.points;
                this.filterPoints(this.filtre);
                this.checkTodo();
            });
    }

    save() {
        if (this.point.description == '' || this.point.description == undefined) {
            this.toastr.error('Description field must be filled', 'Verify Your Form');
        }
        else if (this.point.description.length < 5 || this.point.description.length > 100) {
            this.toastr.error('Description must be 5-100 symbols', 'Verify Your Form');
        }
        else {
            if (this.point.pointId == null) {
                this.pointService.createPoint(this.point, this.id)
                    .pipe(take(1))
                    .subscribe((data: TodoPoint) => {
                        this.todo.points.push(data);
                        this.toastr.success('Point Was Added Successfully', 'Point Adding');
                        this.loadPoints();
                        this.cancel();
                    }, () => {
                    })
            } else {
                this.pointService.updatePoint(this.point)
                    .pipe(take(1))
                    .subscribe(() => {
                        this.toastr.info('Point Was Updated successfully', 'Point Updating');
                        this.loadPoints();
                        this.cancel();
                    }, err => {
                        console.log(err)
                    }
                    )
            }

        }
    }

    editPoint(p: TodoPoint) {
        this.point = p;
        this.tableMode = false;
        this.createMode = false;
    }

    cancel() {
        this.point = new TodoPoint();
        this.tableMode = true;
        this.checked = true;
        this.loadPoints();
        this.doFilter('');
    }

    delete(point: TodoPoint) {
        this.pointService.deletePoint(point)
            .pipe(take(1))
            .subscribe(() => {
                this.loadPoints();
                this.toastr.info('Point Was Deleted successfully', 'Point Deleting');
            },
                err => {
                    console.log(err)
                }
            );
    }

    deleteAllPoints() {
        this.pointService.deleteAllPoints(this.id)
            .pipe(take(1))
            .subscribe(() => {
                this.loadPoints();
                this.toastr.info('All Points Was Deleted successfully', 'Points Deleting');
            },
                err => {
                    console.log(err);
                }
            );
    }

    add() {
        this.cancel();
        this.tableMode = false;
        this.createMode = true;
    }

    filterlist($event: MatRadioChange) {
        console.log($event.source.name, $event.value);

        this.filtre = $event.value;
        this.loadPoints();
    }

    filterPoints(fltr: string) {
        if (fltr == 'Active') {
            this.dataSource.data = this.dataSource.data.filter(x => x.isCompleted == false);
        }
        if (fltr == 'Completed') {
            this.dataSource.data = this.dataSource.data.filter(x => x.isCompleted);
        }
        if (fltr == 'All') {
            this.dataSource.data = this.dataSource.data;
        }
    }

    checkTodo() {
        let count = 0;
        for (var p in this.todo.points) {
            count++
        }
        if (count == 0 && this.checked == false) {
            this.tableMode = false;
        }
    }

    openDialog() {
        const dialogConfig = new MatDialogConfig();

        dialogConfig.data = {
            title: 'All Points for Todo with title "' + this.todo.title + '"'
        };

        const dialogRef = this.dialog.open(DialogBodyComponent, dialogConfig);

        dialogRef.afterClosed()
            .pipe(take(1))
            .subscribe(result => {
                if (result == true) {
                    this.deleteAllPoints();
                }
            });
    }

    openDialogById(point: TodoPoint) {
        const dialogConfig = new MatDialogConfig();

        dialogConfig.data = {
            title: 'this Point for Todo with title "' + this.todo.title + '"'
        };

        const dialogRef = this.dialog.open(DialogBodyComponent, dialogConfig);

        dialogRef.afterClosed()
            .pipe(take(1))
            .subscribe(result => {
                if (result == true) {
                    this.delete(point);
                }
            });
    }

    public doFilter = (value: string) => {
        this.dataSource.filter = value.trim().toLocaleLowerCase();
    }

}

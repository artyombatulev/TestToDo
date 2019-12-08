import { Component, OnInit, ViewChild, AfterViewInit, OnDestroy } from '@angular/core';
import { Todo } from 'src/app/shared/todo.model';
import { TodoService } from 'src/app/shared/todo.service';
import { Router } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { DatePipe } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { DialogBodyComponent } from '../dialog-body/dialog-body.component';
import { MatRadioModule, MatRadioChange } from '@angular/material/radio';
import { MatTableDataSource, MatSort, MatPaginator } from '@angular/material';
import { Subscription } from 'rxjs';
import { take } from 'rxjs/operators';


@Component({
    selector: 'app-todo',
    templateUrl: './todo.component.html',
    styleUrls: ['./todo.component.css'],
    providers: [TodoService]
})
export class TodoComponent implements OnInit, AfterViewInit {

    todo: Todo = new Todo();
    todos: Todo[];
    tableMode: boolean = true;
    createMode: boolean = true;

    filtre: string = "All"

    public displayedColumns = ['title', 'creationDate', 'completed', 'details', 'update', 'delete'];
    public dataSource = new MatTableDataSource<Todo>();

    @ViewChild(MatSort, { static: false }) sort: MatSort;
    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;

    constructor(private todoService: TodoService,
        private router: Router,
        public datepipe: DatePipe,
        private toastr: ToastrService,
        public dialog: MatDialog) {
    }

    ngOnInit() {
        this.loadTodos();
    }

    ngAfterViewInit(): void {
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
    }

    loadTodos() {
        this.todoService.getTodos()
            .pipe(take(1))
            .subscribe(res => {
                //this.todos = res as Todo[];
                this.dataSource.data = res as Todo[];
                this.filterTodos(this.filtre);
                this.checkTodos();
                this.dataSource.sort = this.sort;
                this.dataSource.paginator = this.paginator;
            });
    }

    save() {
        if (this.todo.title == '' || this.todo.title == undefined) {
            this.toastr.error('Title field must be filled', 'Verify Your Form');
        }
        else if (this.todo.title.length < 5 || this.todo.title.length > 30) {
            this.toastr.error('Title must be 5-30 symbols', 'Verify Your Form');
        }
        else {
            if (this.todo.todoId == null) {
                this.todoService.createTodo(this.todo)
                    .pipe(take(1))
                    .subscribe((data: Todo) => {
                        this.dataSource.data.push(data);
                        this.toastr.success('Todo Was Added Successfully', 'Todo Adding');
                        this.loadTodos();
                        this.cancel();
                    }, () => {
                    })
            }
            else {
                this.todoService.updateTodo(this.todo)
                    .pipe(take(1))
                    .subscribe(() => {
                        this.toastr.info('ToDo Was Updated successfully', 'ToDo Updating');
                        this.loadTodos();
                        this.cancel();
                    }, err => {
                        console.log(err);
                    }
                    )
            }
        }
    }

    editTodo(todo: Todo) {
        this.todo = todo;
        this.tableMode = false
        this.createMode = false;
    }

    cancel() {
        this.todo = new Todo();
        this.tableMode = true;
        this.loadTodos();
        this.doFilter('');
    }

    delete(todo: Todo) {
        this.todoService.deleteTodo(todo.todoId)
            .pipe(take(1))
            .subscribe(
                () => {
                    this.loadTodos();
                    this.toastr.info('Todo Was Deleted successfully', 'Todo Deleting');
                }, err => {
                    console.log(err);
                }
            );
    }

    deleteAllTodos() {
        this.todoService.deleteAllTodos()
            .pipe(take(1))
            .subscribe(
                () => {
                    this.loadTodos();
                    this.toastr.info('All Todos Was Deleted successfully', 'Todos Deleting');
                }, err => {
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
        this.loadTodos();
    }

    filterTodos(fltr: string) {
        if (fltr == 'Active') {
            this.dataSource.data = this.dataSource.data.filter(x => x.completed == false);
        }
        if (fltr == 'Completed') {
            this.dataSource.data = this.dataSource.data.filter(x => x.completed);
        }
        if (fltr == 'All') {
            this.dataSource.data = this.dataSource.data;
        }
    }

    redirectToPoints(todoId: any) {
        this.router.navigate(['/point/', todoId]);
    }

    checkTodos() {
        if (this.dataSource.data.length == 0) {
            this.tableMode = false;
        }
    }

    openDialog() {
        const dialogConfig = new MatDialogConfig();

        dialogConfig.data = {
            title: 'All Todos'
        };

        const dialogRef = this.dialog.open(DialogBodyComponent, dialogConfig);

        dialogRef.afterClosed().pipe(take(1)).subscribe(result => {
            if (result == true) {
                this.deleteAllTodos();
            }
        });
    }

    openDialogById(todo: Todo) {
        const dialogConfig = new MatDialogConfig();

        dialogConfig.data = {
            title: 'Todo with title "' + todo.title + '"'
        };

        const dialogRef = this.dialog.open(DialogBodyComponent, dialogConfig);

        dialogRef.afterClosed().pipe(take(1)).subscribe(result => {
            if (result == true) {
                this.delete(todo);
            }
        });
    }

    public doFilter = (value: string) => {
        this.dataSource.filter = value.trim().toLocaleLowerCase();
    }
}

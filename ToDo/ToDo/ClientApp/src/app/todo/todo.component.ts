import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { Todo } from 'src/app/shared/todo.model';
import { TodoService } from 'src/app/shared/todo.service';
import { Router } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { DatePipe } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { DialogBodyComponent } from '../dialog-body/dialog-body.component';
import { MatRadioModule, MatRadioChange } from '@angular/material/radio';
import { MatTableDataSource, MatSort, MatPaginator  } from '@angular/material';


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

    public displayedColumns = ['title', 'creationDate', 'completed', 'details','update', 'delete'];
    public dataSource = new MatTableDataSource<Todo>();

    @ViewChild(MatSort) sort: MatSort;
    @ViewChild(MatPaginator) paginator: MatPaginator;

    constructor(private todoService: TodoService,
        private router: Router,
        public datepipe: DatePipe,
        private toastr: ToastrService,
        public dialog: MatDialog) {
    }


    ngOnInit() {
        this.loadTodos(this.filtre);
    }

    ngAfterViewInit(): void {
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
    }

    loadTodos(f) {
        //this.todoService.getTodos()
        //.subscribe((data: Todo[]) => this.todos = data);

        this.todoService.getTodos().toPromise().then(res => {
            this.todos = res as Todo[]
            this.dataSource.data = res as Todo[];
            this.filterTodos(this.filtre)
            this.checkTodos()
        });


    }

    save() {
        if (this.todo.title == '' || this.todo.title == undefined) {
            this.toastr.error('Title field must be filled', 'Verify Your Form');
        }
        else {

            if (this.todo.todoId == null) {
                this.todoService.createTodo(this.todo)
                    .subscribe((data: Todo) => {
                        this.dataSource.data.push(data)
                        this.toastr.success('Todo Was Added Successfully', 'Todo Adding')
                        this.loadTodos(this.filtre)
                        this.cancel()
                    },() => {
                        })
            }
            else {
                this.todoService.updateTodo(this.todo)
                    .subscribe(data => {
                        this.toastr.info('ToDo Was Updated successfully', 'ToDo Updating')
                        this.loadTodos(this.filtre)
                        this.cancel()
                    }, err => {
                        console.log(err)
                    }
                    )
            }
        }
        
    }

    create() {
        this.todoService.createTodo(this.todo);
        
    }

    editTodo(t: Todo) {
        this.todo = t;
        this.tableMode = false
        this.createMode = false;
    }

    cancel() {
        this.todo = new Todo();
        this.tableMode = true;
        this.loadTodos(this.filtre);
        this.doFilter('');
    }

    delete(t: Todo) {
        this.todoService.deleteTodo(t.todoId)
            .subscribe(
                data => {
                    this.loadTodos(this.filtre)
                    this.toastr.info('Todo Was Deleted successfully', 'Todo Deleting')
                }, err => {
                    console.log(err)
                }
            );

    }

    deleteAllTodos() {
        this.todoService.deleteAllTodos()
            .subscribe(
            data => {
                this.loadTodos(this.filtre)
                this.toastr.info('All Todos Was Deleted successfully', 'Todos Deleting')
            }, err => {
                console.log(err)
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
        this.loadTodos(this.filtre)
    }


    filterTodos(f) {
        if (f == 'Active') {
            this.dataSource.data = this.dataSource.data.filter(x => x.completed == false)
        }
        if (f == 'Completed') {
            this.dataSource.data = this.dataSource.data.filter(x => x.completed)
        }
        if (f == 'All') {
            this.dataSource.data = this.dataSource.data;
        }
    }

    redirectToPoints(todoId:any) {
        this.router.navigate(['/point/', todoId]);
    }
    

    checkTodos() {
        if (this.todos.length == 0) {
            this.tableMode = false;
        }
    }


    openDialog() {
        const dialogConfig = new MatDialogConfig();

        dialogConfig.data = {
            title: 'All Todos'
        };

        const dialogRef = this.dialog.open(DialogBodyComponent, dialogConfig);

        dialogRef.afterClosed().subscribe(result => {
            if (result == true) {
                this.deleteAllTodos();
            }
        });
    }

    openDialogById(t: Todo) {
        const dialogConfig = new MatDialogConfig();

        dialogConfig.data = {
            title: 'Todo with title "' + t.title + '"'
        };

        const dialogRef = this.dialog.open(DialogBodyComponent, dialogConfig);

        dialogRef.afterClosed().subscribe(result => {
            if (result == true) {
                this.delete(t);
            }
        });
    }

    public doFilter = (value: string) => {
        this.dataSource.filter = value.trim().toLocaleLowerCase();
    }
}

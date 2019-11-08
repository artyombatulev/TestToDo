import { Component, OnInit } from '@angular/core';
import { Todo } from 'src/app/shared/todo.model';
import { TodoService } from 'src/app/shared/todo.service';
import { Router } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { DatePipe } from '@angular/common';
import { ToastrService } from 'ngx-toastr';



@Component({
  selector: 'app-todo',
  templateUrl: './todo.component.html',
    styleUrls: ['./todo.component.css'],
    providers: [TodoService]
})
export class TodoComponent implements OnInit {


    todo: Todo = new Todo();   // изменяемый товар
    todos: Todo[];                // массив товаров
    tableMode: boolean = true;          // табличный режим
    editedTodo: Todo = new Todo();
    filtre: string = "All"
    editMode: boolean = false;

    constructor(private todoService: TodoService,
        private router: Router,
        public datepipe: DatePipe,
        private toastr: ToastrService) {
    }

    ngOnInit() {
        this.loadTodos(this.filtre);    // загрузка данных при старте компонента  
    }
    // получаем данные через сервис
    loadTodos(f) {
        //this.todoService.getTodos()
        //.subscribe((data: Todo[]) => this.todos = data);
        this.todoService.getTodos().toPromise().then(res => {
            this.todos = res as Todo[]
            this.filterTodos(this.filtre)
        });


    }
    // сохранение данных
    save() {
        if (this.todo.title == '' || this.todo.title == undefined) {
            this.toastr.error('Title field must be filled', 'Verify Your Form');
        }
        else {
        if (this.todo.todoId == null) {
            this.todoService.createTodo(this.todo)
                .subscribe((data: Todo) => this.todos.push(data));
            this.toastr.success('Todo Was Added Successfully', 'Todo Adding');
        } else {
            this.todoService.updateTodo(this.todo)
                .subscribe(data => this.loadTodos(this.filtre));
            this.toastr.info('ToDo Was Updated successfully', 'ToDo Updating');
        }
            this.cancel();
        }
    }

    create() {
        this.todoService.createTodo(this.todo);
        
    }

    editTodo(t: Todo) {
        this.todo = t;
        this.editedTodo = t;
        this.editMode = true;
    }

    cancel() {
        this.todo = new Todo();
        this.tableMode = true;
        this.editMode = false;
    }

    delete(t: Todo) {
        const ans = confirm('Do you want to delete this ToDo?');
        if (ans) {
        this.todoService.deleteTodo(t.todoId)
            .subscribe(data => this.loadTodos(this.filtre));
            this.toastr.info('Todo Was Deleted successfully', 'Todo Deleting');
        }
    }

    DeleteALLTodos() {
        this.todoService.deleteAllTodos();
    }

    add() {
        this.cancel();
        this.tableMode = false;
    }

    filterlist(fil: string) {
        this.filtre = fil;
        this.loadTodos(this.filtre)
    }


    filterTodos(f) {
        if (f == 'Active') {
            this.todos = this.todos.filter(x => x.completed == false)
        }
        if (f == 'Completed') {
            this.todos = this.todos.filter(x => x.completed)
        }
        if (f == 'All') {
            this.todos = this.todos;
        }
    }

    gotoPoints(todoId:any) {
        this.router.navigate(['/point/', todoId]);
    }
    
    changeDateFormat(t: any) {
        return this.datepipe.transform(t, 'hh:mm a  dd/MM/yyy');
    }

}

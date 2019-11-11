import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Todo } from './todo.model';

@Injectable()
export class TodoService {

    private url = "/api/todo";

    constructor(private http: HttpClient) {
    }

    getTodos() {
        return this.http.get(this.url);
    }

    getTodo(id: number) {
        return this.http.get(this.url + `/${id}`);
    }
    createTodo(todo: Todo) {
        //let dateTime = new Date();
        //todo.creationDate = dateTime;
        //todo.completed = false;
        return this.http.post(this.url, todo);
    }
    updateTodo(todo: Todo) {
        return this.http.put(this.url + '/' + todo.todoId, todo);
    }

    deleteAllTodos() {
        return this.http.delete(this.url);
    }

    deleteTodo(id: number) {
        return this.http.delete(this.url + `/${id}`);
    }

    
}

"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var TodoService = /** @class */ (function () {
    function TodoService(http) {
        this.http = http;
        this.url = "/api/todo";
    }
    TodoService.prototype.getTodos = function () {
        return this.http.get(this.url);
    };
    TodoService.prototype.getTodo = function (id) {
        return this.http.get(this.url + ("/" + id));
    };
    TodoService.prototype.createTodo = function (todo) {
        //let dateTime = new Date();
        //todo.creationDate = dateTime;
        //todo.completed = false;
        return this.http.post(this.url, todo);
    };
    TodoService.prototype.updateTodo = function (todo) {
        return this.http.put(this.url + '/' + todo.todoId, todo);
    };
    TodoService.prototype.deleteAllTodos = function () {
        return this.http.delete(this.url);
    };
    TodoService.prototype.deleteTodo = function (id) {
        return this.http.delete(this.url + ("/" + id));
    };
    TodoService = __decorate([
        core_1.Injectable()
    ], TodoService);
    return TodoService;
}());
exports.TodoService = TodoService;
//# sourceMappingURL=todo.service.js.map
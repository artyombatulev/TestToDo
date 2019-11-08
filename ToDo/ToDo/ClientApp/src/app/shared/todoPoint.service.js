"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var TodoPointService = /** @class */ (function () {
    function TodoPointService(http) {
        this.http = http;
        this.url = "/api/point";
    }
    TodoPointService.prototype.getPoints = function (todoId) {
        return this.http.get(this.url + '/' + todoId);
    };
    /* getPoint(id: number) {
         return this.http.get(this.url + '/' + id);
     }*/
    TodoPointService.prototype.createPoint = function (point, id) {
        //let dateTime = new Date();
        //point.dateOfComplition = dateTime;
        point.isCompleted = false;
        point.todoId = id;
        return this.http.post(this.url, point);
    };
    TodoPointService.prototype.updatePoint = function (point) {
        return this.http.put(this.url + '/' + point.pointId, point);
    };
    TodoPointService.prototype.deletePoint = function (id) {
        return this.http.delete(this.url + '/' + id);
    };
    TodoPointService.prototype.clearAllPoints = function (id) {
        return this.http.delete(this.url + '/' + id);
    };
    TodoPointService = __decorate([
        core_1.Injectable()
    ], TodoPointService);
    return TodoPointService;
}());
exports.TodoPointService = TodoPointService;
//# sourceMappingURL=todoPoint.service.js.map
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TodoPoint } from './todoPoint.model';

@Injectable()
export class TodoPointService {

    private url = "/api/point";

    constructor(private http: HttpClient) {
    }

    createPoint(point: TodoPoint, id: number) {
        point.isCompleted = false;
        point.todoId = id;
        return this.http.post(this.url, point);
    }

    updatePoint(point: TodoPoint) {
        return this.http.put(this.url + '/' + point.pointId, point);
    }

    deletePoint(point: TodoPoint) {
        return this.http.post(this.url + '/' + point.pointId, point);
    }

    deleteAllPoints(id: number) {
        return this.http.delete(this.url + `/${id}`);
    }
}

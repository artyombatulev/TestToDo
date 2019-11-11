import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TodoPoint } from './todoPoint.model';

@Injectable()
export class TodoPointService {

    private url = "/api/point";

    constructor(private http: HttpClient) {
    }

    getPoints(todoId: number) {
        return this.http.get(this.url + `/${todoId}`);
    }

   /* getPoint(id: number) {
        return this.http.get(this.url + '/' + id);
    }*/
    createPoint(point: TodoPoint,id:number) {
        //let dateTime = new Date();
        //point.dateOfComplition = dateTime;
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

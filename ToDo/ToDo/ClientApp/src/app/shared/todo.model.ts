import { TodoPoint } from './todoPoint.model';

export class Todo {
    constructor(
        public todoId?: number,
        public title?: string,
        public creationDate?: Date,
        public completed?: boolean,
        public points?: TodoPoint[]) { }
}

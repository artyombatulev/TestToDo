export class Todo {
    constructor(
        public todoId?: number,
        public title?: string,
        public creationDate?: Date,
        public completed?: boolean) { }
}

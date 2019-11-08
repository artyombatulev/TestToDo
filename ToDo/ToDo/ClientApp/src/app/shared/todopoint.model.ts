export class TodoPoint {
    constructor(
        public pointId?: number,
        public description?: string,
        public isCompleted?: boolean,
        public dateOfComplition?: Date,
        public todoId?: number,
    ) { }
}

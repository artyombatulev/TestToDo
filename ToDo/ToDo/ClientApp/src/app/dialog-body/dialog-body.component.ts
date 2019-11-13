import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material";
import { MatButtonModule } from '@angular/material/button';

@Component({
    selector: 'app-dialog-body',
    templateUrl: './dialog-body.component.html',
    styleUrls: ['./dialog-body.component.css']
})
export class DialogBodyComponent implements OnInit {

    constructor(public dialogRef: MatDialogRef<DialogBodyComponent>,
        @Inject(MAT_DIALOG_DATA) private data) { }

    ngOnInit() {
    }

    close() {
        this.dialogRef.close();
    }

}

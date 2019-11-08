import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-point',
  templateUrl: './point.component.html',
  styleUrls: ['./point.component.css']
})
export class PointComponent implements OnInit {

  //constructor() { }
    constructor(private router: Router) { }

  ngOnInit() {
  }
    point = {
        id: 100,
        title: 'How to make router & navigation in Angular 8',
        description: 'A complete tutorial about creating router and navigation in the Angular 8 Web Application'
    };


    gotoDetails(pointId: any) {
        this.router.navigate(['/point/', pointId]);
    }
}



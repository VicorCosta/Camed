import { Component, Input } from '@angular/core';


@Component({
  selector: 'app-placeholder-grid',
  styleUrls: ['placeholder-grid.css'],
  templateUrl: 'placeholder-grid.component.html',
})
export class PlaceholderGridComponent {
  @Input() show: any;

}

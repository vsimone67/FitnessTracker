import { Component, OnInit } from '@angular/core';
import { EventService } from '@shared/services/event.service';
import { events } from '@shared/models/events.model';

@Component({
  selector: 'spinner',
  templateUrl: 'spinner.component.html'
})
export class SpinnerComponent implements OnInit {
  isActive = '';

  constructor(private _eventService: EventService) {
    _eventService.getEvent(events.spinnerEvent).subscribe(
      spinnerState => {
        this.isActive = spinnerState;
      });
  }
  ngOnInit() { }
}

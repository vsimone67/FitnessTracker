import { Component, OnInit, Input, ElementRef } from '@angular/core';
import { GridOptions } from 'ag-grid-community';
import { EventService } from '../../../shared/services';
import { WorkoutService } from '../../services/workout.service';
import { BaseComponent } from '../../../shared/components';
import { RepsFieldComponent, DeleteExerciseComponent, EditExerciseComponent, RepsDropDownComponent } from '../../components';
import { Workout, set, ExerciseName, RepsName, Exercise, Reps, SetName } from '../../models';
import { DropDownModel, events } from '../../../shared/models';
import { DeleteImageComponent } from '../../../shared/components/delete-image/delete-image.component'
import { fail } from 'assert';
@Component({
  selector: 'addSet',
  templateUrl: 'add-set.component.html'
})
export class AddSetComponent extends BaseComponent implements OnInit {
  @Input() workout: Workout;
  public _set: set;
  dialog: any;
  exercises: Array<ExerciseName>;
  currentExercise: ExerciseName;
  currentMeasure: DropDownModel;
  currentSet: SetName;
  measure: Array<DropDownModel>;
  public selectedMeasure: DropDownModel;
  public selectedExercise: Exercise;
  public selectedSet: set;
  public reps: Array<Reps>;
  public sets: Array<SetName>;
  private repNames: Array<RepsName>;
  private isExerciseAdded: boolean;
  selectedRep: RepsName;
  gridOptions: GridOptions;
  repsGridOptions: GridOptions;

  constructor(private _el: ElementRef, private _workoutService: WorkoutService, public _eventService: EventService) {
    super(_eventService);
    this.sets = new Array<SetName>();
    this.repNames = new Array<RepsName>();

    this.measure = [
      { displayName: 'W', value: 'W' },
      { displayName: 'R', value: 'R' },
      { displayName: 'RT: W', value: 'RT: W' },
      { displayName: 'LT: W', value: 'LT: W' },
      { displayName: 'RT: R', value: 'RT: R' },
      { displayName: 'LT: R', value: 'LT: R' },
      { displayName: 'Sec', value: 'Sec' },
      { displayName: 'C', value: 'C' }
    ];

    this.isExerciseAdded = false;
  }

  ngOnInit() {
    this._set = new set();
    this.InitExercise();
    this.setGridOptions();

    // TODO: Add to NGSX store
    this._workoutService.getExercises((exercises: Array<ExerciseName>) => { this.exercises = exercises; });
    this._workoutService.getSets((sets: Array<SetName>) => { this.sets = sets; });
    this._workoutService.getReps((reps: Array<RepsName>) => { this.repNames = reps; });
  }
  setGridOptions() {
    this.gridOptions = <GridOptions>{};
    this.gridOptions.columnDefs = this.createColumnDefs();
    this.gridOptions.rowData = this._set.Exercise;
    this.gridOptions.rowHeight = 30;

    this.repsGridOptions = <GridOptions>{};
    this.repsGridOptions.columnDefs = [

      {
        headerName: 'Reps', field: 'Name',
        cellRendererFramework: RepsDropDownComponent, width: 120
      },
      { headerName: 'Time To Next Exercise', field: 'TimeToNextExercise', width: 180, editable: true },
      {
        headerName: 'Remove', field: 'Name',
        cellRendererFramework: DeleteImageComponent, width: 120
      }
    ];
    this.repsGridOptions.rowData = this.reps;
    this.repsGridOptions.rowHeight = 30;
  }
  createColumnDefs() {
    return [
      { headerName: 'Exercise', field: 'Name', width: 215 },
      { headerName: "Order", field: "ExerciseOrder", width: 100, editable: true },
      { headerName: 'Measure', field: 'Measure', width: 100 },
      {
        headerName: 'Reps', field: 'Reps',
        cellRendererFramework: RepsFieldComponent, width: 390
      },
      {
        headerName: 'Edit', field: 'Reps',
        cellRendererFramework: EditExerciseComponent, width: 90
      },
      {
        headerName: 'Remove', field: 'Reps',
        cellRendererFramework: DeleteExerciseComponent, width: 90
      }

    ];
  }

  InitExercise() {
    this.selectedExercise = new Exercise();
    this.reps = new Array<Reps>();

    this.selectedRep = new RepsName();
    this.selectedMeasure = new DropDownModel('', '');
  }
  showDialog() {
    this._set = new set();
    this.selectedSet = new set();
    this.InitExercise();
    this.gridOptions.api.setRowData(this._set.Exercise);

    this.dialog = this._el.nativeElement.querySelector('dialog');

    if (!this.dialog.showModal) {
      this.dialog.dialogPolyfill.registerDialog(this.dialog);
    }
    this.isExerciseAdded = false;
    this.dialog.showModal();
  }

  showDialogForEdit(set: set) {
    this._set = set;
    this.selectedSet = set;
    this.currentSet = this.sets.find(exp => exp.Name === this._set.Name);
    this.gridOptions.api.setRowData(this._set.Exercise);
    this.dialog = this._el.nativeElement.querySelector('dialog');
    this.isExerciseAdded = false;
    if (!this.dialog.showModal) {
      this.dialog.dialogPolyfill.registerDialog(this.dialog);
    }

    this.dialog.showModal();
  }
  onAddExercise() {
    this.isExerciseAdded = true;
    if (this.reps.length > 0) {
      this.selectedExercise.Measure = this.selectedMeasure.value;
      this.selectedExercise.Reps = this.reps;
      this.selectedExercise.ExerciseOrder = this._set.Exercise.length + 1
      this._set.Exercise.push(this.selectedExercise);
    }

    this.gridOptions.api.setRowData(this._set.Exercise);
    this.clearControls();
  }
  onCellClicked($event: any) {
    if ($event.colDef.headerName === 'Remove') {
      this._set.Exercise.splice($event.rowIndex, 1);
      this.gridOptions.api.setRowData(this._set.Exercise);
    }
    else if ($event.colDef.headerName === 'Edit') {
      this.selectedExercise = $event.data;
      this.currentExercise = this.exercises.find(exp => exp.Name === this.selectedExercise.Name);
      this.currentMeasure = this.measure.find(exp => exp.value === this.selectedExercise.Measure);

      this.reps = this.selectedExercise.Reps;

      this.repsGridOptions.api.setRowData(this.reps);
    }
  }
  onRepsCellClicked($event: any) {
    if ($event.colDef.headerName === 'Remove') {
      this.reps.splice($event.rowIndex, 1);
      this.repsGridOptions.api.setRowData(this.reps);
    }
  }
  onAddReps() {
    this.reps.push(new Reps());
    this.repsGridOptions.api.setRowData(this.reps);
  }

  onSaveSet() {
    if (this.isExerciseAdded) {
      this.onAddExercise();
      this._set.SetOrder = this.workout.Set.length + 1;
      this.workout.Set.push(this._set);
    }
    this.onClose();
    this._eventService.sendEvent(events.addSetEvent, this._set);
  }
  clearControls() {
    this.reps = new Array<Reps>();
    this.selectedExercise = new Exercise();
    this.selectedRep = new RepsName();
    this.selectedMeasure = new DropDownModel('', '');
    this.repsGridOptions.api.setRowData(this.reps);
    this.currentExercise = new ExerciseName();
    this.currentMeasure = new DropDownModel('', '');
    this.currentSet = new SetName();
  }
  onClose() {
    this.clearControls();
    this.dialog.close();
  }
  exerciseSelected(item: any) {
    this.selectedExercise.ExerciseOrder = 1;
    this.selectedExercise.Name = item.Name;
    this.selectedExercise.ExerciseNameId = item.ExerciseNameId;
  }
  setSelected(item: any) {
    this._set.Name = item.Name;
    this._set.SetNameId = item.SetNameId;
    this.selectedSet = item;
  }
  measureSelected(item: any) {
    this.selectedExercise.Measure = item.value;
    this.selectedMeasure = item;
  }
  repSelected(item: any) {
    this.selectedRep = item;
  }
}

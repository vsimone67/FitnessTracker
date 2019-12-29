import { Component, OnInit, ElementRef, Inject } from "@angular/core";
import { EventService } from "../../../shared/services";
import { BaseComponent } from "../../../shared/components";
import { MeatabolicInfoConstants } from "../../models";
import { MeatabolicInfo } from "../../models/metabolic-info.model";
import { Store, Select } from "@ngxs/store";
import { Observable } from "rxjs/Observable";
import {
  GetMetabolicInfo,
  SaveMetabolicInfo
} from "../../actions/metabolic-info.actions";
import { MetabolicInfoState } from "../../state/metabolic-info.state";

@Component({
  selector: "macrocalc",
  templateUrl: "macro-calculator.component.html"
})
export class MacroCalculatorComponent extends BaseComponent implements OnInit {
  dialog: any;
  public metabolicInfo: Array<MeatabolicInfo>;
  public weight: number;
  public activityLevel: number;
  public bmr: number;
  public dcr: number;

  public calorieDeficitPerCut: number;
  public calorieDeficitPerMaintain: number;
  public calorieDeficitPerGain: number;

  public calorieDeficitCut: number;
  public calorieDeficitMaintain: number;
  public calorieDeficitGain: number;

  public newCalorieCut: number;
  public newCalorieMaintain: number;
  public newCalorieGain: number;

  public fatPerCut: number;
  public fatPerMaintain: number;
  public fatPerGain: number;

  public fatCaloriesCut: number;
  public fatCaloriesMaintain: number;
  public fatCaloriesGain: number;

  public fatGramsCut: number;
  public fatGramsMaintain: number;
  public fatGramsGain: number;

  public proteinPerCut: number;
  public proteinPerMaintain: number;
  public proteinPerGain: number;

  public proteinCaloriesCut: number;
  public proteinCaloriesMaintain: number;
  public proteinCaloriesGain: number;

  public proteinGramsCut: number;
  public proteinGramsMaintain: number;
  public proteinGramsGain: number;

  public carbsPerCut: number;
  public carbsPerMaintain: number;
  public carbsPerGain: number;

  public carbsCaloriesCut: number;
  public carbsCaloriesMaintain: number;
  public carbsCaloriesGain: number;

  public carbsGramsCut: number;
  public carbsGramsMaintain: number;
  public carbsGramsGain: number;

  @Select(MetabolicInfoState.metabolicInfoList)
  metabolicInfoList$: Observable<Array<MeatabolicInfo>>;

  constructor(
    private _el: ElementRef,
    private _store: Store,
    public _eventService: EventService
  ) {
    super(_eventService);
    this.metabolicInfo = new Array<MeatabolicInfo>();
  }

  ngOnInit() {
    this._store.dispatch(new GetMetabolicInfo());
    // .subscribe(() =>
    //   this.metabolicInfoList$.subscribe(
    //     metabolicInfoList => (this.metabolicInfo = metabolicInfoList)
    //   )
    // );

    this.metabolicInfoList$.subscribe(
      metabolicInfoList => (this.metabolicInfo = metabolicInfoList)
    );
  }
  showDialog() {
    this.getMacroInputs();
    this.calcMacros();
    this.dialog = this._el.nativeElement.querySelector("dialog");

    if (!this.dialog.showModal) {
      this.dialog.dialogPolyfill.registerDialog(this.dialog);
    }

    this.dialog.showModal();
  }
  reCalcMacro(event: any) {
    this.calcMacros();
  }
  onSave() {
    let weight: MeatabolicInfo = this.metabolicInfo.find(
      exp => exp.macro === MeatabolicInfoConstants.weight
    );
    weight.cut = this.weight;
    weight.maintain = this.weight;
    weight.gain = this.weight;

    this._store
      .dispatch(new SaveMetabolicInfo(weight))
      .subscribe(() => this.showMessage("Weight Saved"));

    let dcr = this.metabolicInfo.find(
      exp => exp.macro === MeatabolicInfoConstants.dcr
    );
    dcr.cut = this.dcr;
    dcr.maintain = this.dcr;
    dcr.gain = this.dcr;

    this._store
      .dispatch(new SaveMetabolicInfo(dcr))
      .subscribe(() => this.showMessage("DCR Saved"));

    let caloriePer = this.metabolicInfo.find(
      exp => exp.macro === MeatabolicInfoConstants.caldefper
    );
    caloriePer.cut = this.calorieDeficitPerCut / 100;
    caloriePer.maintain = this.calorieDeficitPerMaintain / 100;
    caloriePer.gain = this.calorieDeficitPerGain / 100;

    this._store
      .dispatch(new SaveMetabolicInfo(caloriePer))
      .subscribe(() => this.showMessage("Calorie % Saved"));

    let calorieMax = this.metabolicInfo.find(
      exp => exp.macro === MeatabolicInfoConstants.calories
    );
    calorieMax.cut = this.newCalorieCut;
    calorieMax.maintain = this.newCalorieMaintain;
    calorieMax.gain = this.newCalorieGain;

    this._store
      .dispatch(new SaveMetabolicInfo(calorieMax))
      .subscribe(() => this.showMessage("Calorie Max Saved"));

    let fatPer = this.metabolicInfo.find(
      exp => exp.macro === MeatabolicInfoConstants.fatper
    );
    fatPer.cut = this.fatPerCut / 100;
    fatPer.maintain = this.fatPerMaintain / 100;
    fatPer.gain = this.fatPerGain / 100;

    this._store
      .dispatch(new SaveMetabolicInfo(fatPer))
      .subscribe(() => this.showMessage("Fat Saved"));

    let proteinPer = this.metabolicInfo.find(
      exp => exp.macro === MeatabolicInfoConstants.proteinper
    );
    proteinPer.cut = this.proteinPerCut / 100;
    proteinPer.maintain = this.proteinPerMaintain / 100;
    proteinPer.gain = this.proteinPerGain / 100;

    this._store
      .dispatch(new SaveMetabolicInfo(proteinPer))
      .subscribe(() => this.showMessage("Protein Saved"));

    let carbsPer = this.metabolicInfo.find(
      exp => exp.macro === MeatabolicInfoConstants.carbsper
    );
    carbsPer.cut = this.carbsPerCut / 100;
    carbsPer.maintain = this.carbsPerMaintain / 100;
    carbsPer.gain = this.carbsPerGain / 100;

    this._store
      .dispatch(new SaveMetabolicInfo(carbsPer))
      .subscribe(() => this.showMessage("Carbs Saved"));

    this.onClose();
  }
  onClose() {
    this.dialog.close();
  }
  getMacroInputs() {
    // these are the variables that can be changed on the screen
    this.weight = this.metabolicInfo.find(
      exp => exp.macro === MeatabolicInfoConstants.weight
    ).cut;
    this.dcr = this.metabolicInfo.find(
      exp => exp.macro === MeatabolicInfoConstants.dcr
    ).cut;
    this.calorieDeficitPerCut =
      this.metabolicInfo.find(
        exp => exp.macro === MeatabolicInfoConstants.caldefper
      ).cut * 100;
    this.calorieDeficitPerMaintain =
      this.metabolicInfo.find(
        exp => exp.macro === MeatabolicInfoConstants.caldefper
      ).maintain * 100;
    this.calorieDeficitPerGain =
      this.metabolicInfo.find(
        exp => exp.macro === MeatabolicInfoConstants.caldefper
      ).gain * 100;

    this.fatPerCut =
      this.metabolicInfo.find(
        exp => exp.macro === MeatabolicInfoConstants.fatper
      ).cut * 100;
    this.fatPerMaintain =
      this.metabolicInfo.find(
        exp => exp.macro === MeatabolicInfoConstants.fatper
      ).maintain * 100;
    this.fatPerGain =
      this.metabolicInfo.find(
        exp => exp.macro === MeatabolicInfoConstants.fatper
      ).gain * 100;

    this.proteinPerCut =
      this.metabolicInfo.find(
        exp => exp.macro === MeatabolicInfoConstants.proteinper
      ).cut * 100;
    this.proteinPerMaintain =
      this.metabolicInfo.find(
        exp => exp.macro === MeatabolicInfoConstants.proteinper
      ).maintain * 100;
    this.proteinPerGain =
      this.metabolicInfo.find(
        exp => exp.macro === MeatabolicInfoConstants.proteinper
      ).gain * 100;

    this.carbsPerCut =
      this.metabolicInfo.find(
        exp => exp.macro === MeatabolicInfoConstants.carbsper
      ).cut * 100;
    this.carbsPerMaintain =
      this.metabolicInfo.find(
        exp => exp.macro === MeatabolicInfoConstants.carbsper
      ).maintain * 100;
    this.carbsPerGain =
      this.metabolicInfo.find(
        exp => exp.macro === MeatabolicInfoConstants.carbsper
      ).gain * 100;
  }
  calcMacros() {
    this.activityLevel = this.metabolicInfo.find(
      exp => exp.macro === MeatabolicInfoConstants.actlvl
    ).cut;
    this.bmr = this.metabolicInfo.find(
      exp => exp.macro === MeatabolicInfoConstants.bmr
    ).cut;

    this.calorieDeficitCut = Math.round(
      (this.calorieDeficitPerCut / 100) * this.dcr
    );
    this.calorieDeficitMaintain = Math.round(
      (this.calorieDeficitPerMaintain / 100) * this.dcr
    );
    this.calorieDeficitGain = Math.round(
      (this.calorieDeficitPerGain / 100) * this.dcr
    );
    this.newCalorieCut = Number(this.dcr) - this.calorieDeficitCut;
    this.newCalorieMaintain = Number(this.dcr) - this.calorieDeficitMaintain;
    this.newCalorieGain = Number(this.dcr) + this.calorieDeficitGain;

    this.fatCaloriesCut = Math.round(
      (this.fatPerCut / 100) * this.newCalorieCut
    );
    this.fatGramsCut = Math.round(this.fatCaloriesCut / 9);
    this.fatCaloriesMaintain = Math.round(
      (this.fatPerMaintain / 100) * this.newCalorieMaintain
    );
    this.fatGramsMaintain = Math.round(this.fatCaloriesMaintain / 9);
    this.fatCaloriesGain = Math.round(
      (this.fatPerGain / 100) * this.newCalorieGain
    );
    this.fatGramsGain = Math.round(this.fatCaloriesGain / 9);

    this.proteinCaloriesCut = Math.round(
      (this.proteinPerCut / 100) * this.newCalorieCut
    );
    this.proteinGramsCut = Math.round(this.proteinCaloriesCut / 4);
    this.proteinCaloriesMaintain = Math.round(
      (this.proteinPerMaintain / 100) * this.newCalorieMaintain
    );
    this.proteinGramsMaintain = Math.round(this.proteinCaloriesMaintain / 4);
    this.proteinCaloriesGain = Math.round(
      (this.proteinPerGain / 100) * this.newCalorieGain
    );
    this.proteinGramsGain = Math.round(this.proteinCaloriesGain / 4);

    this.carbsCaloriesCut = Math.round(
      (this.carbsPerCut / 100) * this.newCalorieCut
    );
    this.carbsGramsCut = Math.round(this.carbsCaloriesCut / 4);
    this.carbsCaloriesMaintain = Math.round(
      (this.carbsPerMaintain / 100) * this.newCalorieMaintain
    );
    this.carbsGramsMaintain = Math.round(this.carbsCaloriesMaintain / 4);
    this.carbsCaloriesGain = Math.round(
      (this.carbsPerGain / 100) * this.newCalorieGain
    );
    this.carbsGramsGain = Math.round(this.carbsCaloriesGain / 4);
  }
}

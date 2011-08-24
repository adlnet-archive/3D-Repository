/**  
 * Copyright 2011 U.S. Department of Defense

 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at

 *     http://www.apache.org/licenses/LICENSE-2.0

 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */


function SliderWidget(sliderElement, valueElement, unitType, initialValue) {
    this.CurrentValue = initialValue;
    this.SliderElement = sliderElement;
    this.ValueElement = valueElement;
    this.UnitType = unitType;
    this.TypeScaleFactor = 1;
    this.Active = false;

    $(this.UnitType).change(function (eventObject) {
        with (ScaleSlider) {
            TypeScaleFactor = parseFloat($(this).val());
            CurrentValue = $(SliderElement).slider("option", "value") * TypeScaleFactor;
            SetUnitScale(CurrentValue);
        }
    });

    this.UpdateScale = function (event, ui) {
        with (ScaleSlider) {
            CurrentValue = ui.value * TypeScaleFactor;
            $(ValueElement).html((CurrentValue / TypeScaleFactor).toString());
            SetUnitScale(CurrentValue);
        }
    }


    this.Activate = function () {
        with (this) {
            $(SliderElement).slider({
                value: CurrentValue,
                min: 1,
                step: 0.5,
                slide: UpdateScale
            });
            $(ValueElement).html(this.CurrentValue.toString());
        }
    }

 return true;
}



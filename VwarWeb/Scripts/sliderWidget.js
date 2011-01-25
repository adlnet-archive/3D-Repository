
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



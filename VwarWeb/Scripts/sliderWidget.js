
function SliderWidget(sliderElement, valueElement, unitType, initialValue) {
    this.CurrentValue = initialValue;
    this.SliderElement = sliderElement;
    this.ValueElement = valueElement;
    this.UnitType = unitType;
    this.TypeScaleFactor = 1;

    $(this.UnitType).change(function (eventObject) {
        with (ScaleSlider) {
            TypeScaleFactor = parseFloat($(this).val());
            CurrentValue = $(SliderElement).slider("option", "value") * TypeScaleFactor;
            SetScale(CurrentValue);
        }
    });

    this.UpdateScale = function (event, ui) {
        with (ScaleSlider) {
            CurrentValue = ui.value * TypeScaleFactor;
            $(ValueElement).html((CurrentValue / TypeScaleFactor).toString());
            SetScale(CurrentValue);
        }
    }


    this.Activate = function () {
        with (this) {
            $(SliderElement).slider({
                value: initialValue,
                min: 1,
                step: 0.5,
                slide: UpdateScale
            });
            $(ValueElement).html(this.CurrentValue.toString());
        }
    }

 return true;
}



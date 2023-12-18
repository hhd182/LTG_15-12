using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryBar : MonoBehaviour
{
    public Slider enegySlider;

    public void SetMaxBattery(float energy) {
        enegySlider.maxValue = energy;
        enegySlider.value = energy;
    }

    public void SetEnergy(float energy) {
        enegySlider.value = energy;
    }
}

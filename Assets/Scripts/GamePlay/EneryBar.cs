using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EneryBar : MonoBehaviour
{
    public Slider enegySlider;

    public void SetMaxStamina(int enery) {
        enegySlider.maxValue = enery;
        enegySlider.value = enery;
    }

    public void SetStamina(float stamina) {
        enegySlider.value = stamina;
    }
}

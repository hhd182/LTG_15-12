using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider staminaSlider;
    
    public void SetMaxStamina(float stamina) {
        staminaSlider.maxValue = stamina;
        staminaSlider.value = stamina;
    } 

    public void SetStamina(float stamina) {
        staminaSlider.value = stamina;
    }
}

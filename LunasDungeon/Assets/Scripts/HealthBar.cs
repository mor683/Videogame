using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;          // para que se inicie siempre con la vida maxima

        fill.color = gradient.Evaluate(1f);             // para que se inicie con la vida en verde
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);             // para que se actualice el color con la vida (gradient va desde 0 hasta 1)
    }
}

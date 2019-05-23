using Core;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    [SerializeField] private Slider slider;
    [SerializeField] private Image health;
    
    private  Gradient _gradient = new Gradient();
    private GradientColorKey[] _gradientColorKeys;
    private GradientAlphaKey[] _gradientAlphaKeys;

    private void Awake()
    {
        EventContainer<int>.Subscribe(Topics.HealthBarValuesChange, SetHealthBarValues);
        EventContainer.Subscribe(Topics.GameReload, ResetHealthBar);
    }
	
    private void SetHealthBarValues(int currentHealth)
    {
        slider.value = currentHealth/100f;
        health.color = SetGradient( Color.red, Color.yellow, Color.green, slider.value);        
    }
    
    private Color SetGradient(Color a, Color b, Color c, float t)
    {
        _gradientColorKeys = new GradientColorKey[3];
        _gradientColorKeys[0].color = a;
        _gradientColorKeys[0].time = 0.0f;
        _gradientColorKeys[1].color = b;
        _gradientColorKeys[1].time = 0.5f;
        _gradientColorKeys[2].color = c;
        _gradientColorKeys[2].time = 1f;
            
        _gradientAlphaKeys = new GradientAlphaKey[3];
        _gradientAlphaKeys[0].alpha = 1f;
        _gradientAlphaKeys[0].time = 0.0f;
        _gradientAlphaKeys[1].alpha = 1f;
        _gradientAlphaKeys[1].time = 0.5f;
        _gradientAlphaKeys[2].alpha = 1f;
        _gradientAlphaKeys[2].time = 1f;
            
        _gradient.SetKeys(_gradientColorKeys, _gradientAlphaKeys);
        
        return _gradient.Evaluate(t);
    }

    private void ResetHealthBar()
    {
        SetHealthBarValues(Constants.MaxPlayerHealth);
    }
    
}

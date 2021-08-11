using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class GenericStatBarUI : MonoBehaviour
{
    public Entity listenToEntityHealth;

    public RectTransform linearStatbarTransform;
    public TextMeshProUGUI barText;
    public bool verticalBar = false;

    [Space(10)]

    public bool isRadial;
    public Image barImage;
    public float zeroFill = 0;
    public float maxFill = 1;

    public Image lagBehindImage;
    public float lagBehindTime;

    [Space(10)]

    [SerializeField] private float _maxValue;
    public float maxValue
    {
        get { return _maxValue; }
        set { _maxValue = value; UpdateStatBar(); }
    }

    [SerializeField] private float _currentValue;
    public float currentValue
    {
        get => _currentValue;
        set { SetValue(value); }
    }

    public void SetValue(float value)
    {
        _currentValue = Mathf.Clamp(value,0,_maxValue);
        UpdateStatBar();
    }

    private void Awake()
    {
        if(listenToEntityHealth != null)
        {
            listenToEntityHealth.HealthChanged += SetValue;
        }
    }

    private void OnValidate()
    {
        UpdateStatBar();
    }

    private void UpdateStatBar()
    {
        float normalizedValue = currentValue / _maxValue;
        if (barText)
            barText.text = currentValue.ToString() + "/" + _maxValue;
        if (float.IsNaN(normalizedValue))
            normalizedValue = 0;

        if(!isRadial)
        {
            Vector3 initialScale = linearStatbarTransform.localScale;
            Vector3 updatedScale = verticalBar ? new Vector3(initialScale.x, normalizedValue, initialScale.z) :
                new Vector3(normalizedValue, initialScale.y, initialScale.z);

            linearStatbarTransform.localScale = updatedScale;
        }
        else
        {
            float targetFillAmount = normalizedValue * (maxFill - zeroFill) + zeroFill;
            barImage.fillAmount = targetFillAmount;
            //Lagbehind animation
            if(lagBehindImage && lagbackTargetFillAmount != targetFillAmount)
            {
                if(radialLagbehindCorout != null)
                {
                    StopCoroutine(radialLagbehindCorout);
                }
                lagbackTargetFillAmount = targetFillAmount;
                radialLagbehindCorout = StartCoroutine(Corout_RadialLagbehindAnim(targetFillAmount));
            }
                    
        }
    }

    private Coroutine radialLagbehindCorout;
    private float lagbackTargetFillAmount;

    private IEnumerator Corout_RadialLagbehindAnim(float targetFillAmount)
    {
        float frames = 30;
        float currentFillAmount = lagBehindImage.fillAmount;
        float interval = lagBehindTime / frames*0.8f;
        yield return new WaitForSeconds(lagBehindTime);
        for(int x = 0;x <= frames;x++)
        {
            yield return new WaitForSeconds(interval);
            lagBehindImage.fillAmount = Mathf.SmoothStep(currentFillAmount, targetFillAmount, x / frames);
        }
        lagBehindImage.fillAmount = targetFillAmount;
        radialLagbehindCorout = null;
    }

}


﻿using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Light))]
[RequireComponent(typeof(AudioSource))]
public class flashLight : MonoBehaviour
{
    #region Parameters 


    [SerializeField] KeyCode reloadKey = KeyCode.R;
    [SerializeField] KeyCode toggleKey = KeyCode.F;

    [SerializeField] int maxBattires = 4;
    [SerializeField] int batteries = 2;

    [SerializeField] bool autoReduce = true;
    [SerializeField] float reduceSpeed = 1.0f;

    [SerializeField] bool autoIncrease = false;
    [SerializeField] float increaseSpeed = 0.5f;

    [Range(0, 1)]
    [SerializeField] float toggleOnWaitPrecentage = 0.05f;

    public const float minBatteryLife = 0.0f;
    [SerializeField] float maxBatteryLife = 10.0f;

    [SerializeField] float followSpeed = 5.0f;

    #endregion

    #region References

    [SerializeField] AudioClip onSound = null;
    [SerializeField] AudioClip offSound = null;
    [SerializeField] AudioClip reloadSound = null;

    [SerializeField] Image stateIcon = null;
    [SerializeField] Slider lifeSlider = null;
    [SerializeField] Image lifeSliderFill = null;
    [SerializeField] TextMeshProUGUI reloadText = null;
    [SerializeField] TextMeshProUGUI countText = null;
    [SerializeField] CanvasGroup holder = null;
    [SerializeField] Color fillLifeColor = Color.green;
    [SerializeField] Color outLifeColor = Color.red;

    [SerializeField]new Camera camera= null;
    [SerializeField]GameObject flashlight = null;
#endregion
    #region Statisics

    private float batteryLife = 0.0f;
    [SerializeField]
    private bool usingFlashlight = false;
    [SerializeField]
    private bool outOfBattery = false;
    #endregion
    
    #region Private and Properties

    private IEnumerator IE_UpdateBatteryLife = null;
    Light _light = null;
    Light Light
   
    {
        get
        {
            if(_light = null)
            {
                _light = GetComponent<Light>();
                if (_light == null) {_light = gameObject.AddComponent<Light>();}
                _light.type = LightType.Spot;

            }
            return _light;


        }

    }
    
    float defailtIntesnity = 0.0f;
    AudioSource _source = null;
    AudioSource Source 
 
 

    {
        get
        {
            if(_source == null)
            {
                _source = GetComponent<AudioSource>();
                if (_source == null ){_source = gameObject.AddComponent<AudioSource>();}
                _source.playOnAwake = false;
            }
            return _source;

        }
    



    }
    
  
    float GetlifePercentage
    
    {
        get
        {
            return batteryLife / maxBatteryLife;
        }
    }
  
    
    float GetlifeIntensity
    {
        get
        {
            return defailtIntesnity * GetlifePercentage;
        }
    }
    bool CanReload
    {
        get
        {
            return usingFlashlight && (batteries > 0 && batteryLife < maxBatteryLife);
        }
    }
    bool MoreThanNeededPercetage
    {
        get
        {
            return GetlifePercentage >= toggleOnWaitPrecentage;

        }
    }




    #endregion
    

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey)) 
        {
            ToggleFlashLight(!usingFlashlight, true);
        }
        if (Input.GetKeyDown(reloadKey) && CanReload)
        {
            Reload();

        }
      
    }

    private void Reload()
    {
        batteryLife = maxBatteryLife;
        Light.intensity = GetlifeIntensity;
        batteries--;

        UpdateCountText();
        UpdateSlider();

        UpdateBatteryState(false);
        UpdateBatteryLifeProcess();
    }

    void ToggleFlashLight(bool state, bool playSound)
    {
        usingFlashlight = state;
        flashLight.SetActive(state);

        state = (outOfBattery && usingFlashlight) ? false : usingFlashlight;
        ToggleObject(state);
        if (holder)
        {
            switch (usingFlashlight)
            {
                case true:
                    holder.alpha = 1.0f;
                    holder.blocksRaycasts = true;
                    break;
                case false:
                    holder.alpha = 0.0f;
                    holder.blocksRaycasts = false;

                    break;

            }
        
        }
        if (playSound)
        {
            PlaySFX(usingFlashlight ? onSound : offSound);

        }
        UpdateBatteryLifeProcess();


    }
    private void UpdateBatteryLifeProcess()
    {
        if (IE_UpdateBatteryLife != null) { StopCoroutine(IE_UpdateBatteryLife); }
        if (usingFlashlight && !outOfBattery) 
        {
            if (autoReduce)
            {
                IE_UpdateBatteryLife = ReduceBattery();
                StartCoroutine(IE_UpdateBatteryLife);
            
            }
            return;
        
        }
        if (autoIncrease)
        {
            IE_UpdateBatteryLife = IncreaseBattery();
            StartCoroutine(IE_UpdateBatteryLife);
        
        }
    }

    private IEnumerator IncreaseBattery()
    {
        while (batteryLife < maxBatteryLife)
        {
            var newValue = batteryLife + increaseSpeed * Time.deltaTime;
            batteryLife = Mathf.Clamp(newValue, minBatteryLife, maxBatteryLife);
            Light.intensity = GetlifeIntensity;

            BatteryLifeCheck();

            UpdateSlider();
            yield return null;

           
        
        }
    }

    private void BatteryLifeCheck()
    {
        if (MoreThanNeededPercetage && outOfBattery)
        {
            UpdateBatteryState(false);
            UpdateBatteryLifeProcess();
        
        }
    }

    private IEnumerator ReduceBattery()
    {
        while (batteryLife > 0.0f)
        {
            var newValue = batteryLife - reduceSpeed * Time.deltaTime;
            batteryLife = Mathf.Clamp(newValue, minBatteryLife, maxBatteryLife);

            Light.intensity = GetlifeIntensity;

            UpdateSlider();
            yield return null;
        
        }
        UpdateBatteryState(true);
        UpdateBatteryLifeProcess();
    }

    private void UpdateBatteryState(bool isDead) 
    {
        outOfBattery = isDead;
        if (reloadText) 
        {
            reloadText.enabled = isDead;
        
        }
        if(stateIcon){ stateIcon.color = isDead ? new Color(1, 1, 1, .5f) : Color.white; }
        var state = outOfBattery ? false : usingFlashlight;
        ToggleObject(state);

    }

    private void PlaySFX(AudioClip clip)
    {
        if (clip == null) { return;}
        Source.clip = clip;
        Source.Play();
    }
   
    private static void SetActive(bool state)
    {
        throw new NotImplementedException();
    }

    void ToggleObject(bool state) 
    {
        Light.enabled = state;
    }
    void UpdateSlider() 
    {
        if (lifeSlider) { lifeSlider.value = batteryLife;}
        if(lifeSliderFill)
        {
            lifeSliderFill.color = Color.Lerp(outLifeColor, fillLifeColor, GetlifePercentage);
        }
    
    }
    void UpdateCountText() 
    {
        if (countText) 
        {
            StringBuilder countString = new StringBuilder("Batteries");
            countString.Append(batteries);
            countString.Append("/");
            countString.Append(maxBattires);

            countText.text = countString.ToString();
        
        }
    }
    private void Start()
    {
        Init();
    }

    private void Init()
    {

        batteryLife = maxBatteryLife;

        UpdateBatteryState(false);
        ToggleFlashLight(false, false);

        UpdateCountText();


        lifeSlider.minValue = minBatteryLife;
        lifeSlider.maxValue = maxBatteryLife;
        lifeSlider.value = batteryLife;
        
        if (!camera)
        {
            camera = Camera.main;
        
        }
        reloadText.text = "RELOAD(" + reloadKey + ")";
    }
    
}

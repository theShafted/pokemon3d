using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Pokemon
{
    public class TimeController : MonoBehaviour
    {
        [SerializeField] TMP_Text _timeText;
        [SerializeField] TimeSettings _settings;
        
        [SerializeField] Light _sun;
        [SerializeField] Light _moon;
        
        [SerializeField] AnimationCurve _lightIntensityCurve;
        [SerializeField] float _maxSunIntensity = 1f;
        [SerializeField] float _maxMoonIntensity = 0.5f;
        
        [SerializeField] Material _skyboxMat;

        [SerializeField] Color _dayAmbientLight;
        [SerializeField] Color _nightLightAmbientLight;
        [SerializeField] Volume _volume;

        ColorAdjustments _colorAdjustments; 

        TimeService _service;

        void Start()
        {
            _service = new TimeService(_settings);
            _volume.profile.TryGet(out _colorAdjustments);
        }
        void Update()
        {
            UpdateTimeOfDay();
            RotateSun();
            UpdateLightSettings();
            UpdateSkyBox();

            if (Input.GetKeyDown("z")) _settings._timeMultiplier *=2;
            if (Input.GetKeyDown("x")) _settings._timeMultiplier /=2;
        }

        void UpdateSkyBox()
        {
            float dot = Vector3.Dot(_sun.transform.forward, Vector3.up);
            float blend = Mathf.Lerp(0, 1, _lightIntensityCurve.Evaluate(dot));
            
            float rotation = Mathf.Atan2(_sun.transform.forward.y, _sun.transform.forward.z) * Mathf.Rad2Deg;
            
            _skyboxMat.SetFloat("_Blend", blend);
            _skyboxMat.SetFloat("_Rotation1", rotation);
            _skyboxMat.SetFloat("_Rotation2", rotation);
        }
        void UpdateLightSettings()
        {
            float dot = Vector3.Dot(_sun.transform.forward, Vector3.down);
            _sun.intensity = Mathf.Lerp(0, _maxSunIntensity, _lightIntensityCurve.Evaluate(dot));
            _moon.intensity = Mathf.Lerp(0, _maxMoonIntensity, _lightIntensityCurve.Evaluate(dot));

            if (_colorAdjustments == null) return;

            _colorAdjustments.colorFilter.value = Color.Lerp(_nightLightAmbientLight, _dayAmbientLight, _lightIntensityCurve.Evaluate(dot));
        }
        void RotateSun()
        {
            float rotation = _service.SunAngle();
            _sun.transform.rotation = Quaternion.AngleAxis(rotation, Vector3.right);
        }
        void UpdateTimeOfDay()
        {
            _service.UpdateTime(Time.deltaTime);

            if (_timeText != null) _timeText.text = _service.CurrentTime.ToString("hh:mm");
        }
    }
}
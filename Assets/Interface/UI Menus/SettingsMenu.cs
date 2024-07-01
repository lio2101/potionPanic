using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace LJ.UI
{
    public class SettingsMenu : MonoBehaviour
    {
        // --- Enums ------------------------------------------------------------------------------------------------------

        // --- Fields -----------------------------------------------------------------------------------------------------
        [SerializeField] private CarouselControl _resolutionControl;
        [SerializeField] private CarouselControl _screenSettingControl;

        [SerializeField] private VolumeController _masterVolume;
        [SerializeField] private VolumeController _musicVolume;
        [SerializeField] private VolumeController _effectVolume;

        [SerializeField] private Button _applyVideoSettingsButton;

        // --- Properties -------------------------------------------------------------------------------------------------

        // --- Unity Functions --------------------------------------------------------------------------------------------
        private void Awake()
        {
            //fill screen setting options
            _screenSettingControl.SetOptions(SettingManager.WINDOW_MODE_NAMES);

            //fill resolution options

            List<string> resolutionList = new List<string>();
            foreach(Resolution resolution in SettingManager.Instance.AvailableResolutions)
            {
                resolutionList.Add($"{resolution.width}x{resolution.height}");
            }
            _resolutionControl.SetOptions(resolutionList);

        }

        private void OnEnable()
        {
            _applyVideoSettingsButton.onClick.AddListener(SettingManager.Instance.ApplyVideoSettings);
            EventSystem.current.SetSelectedGameObject(_resolutionControl.gameObject);
            _resolutionControl.EnableHighlight();

            // Load current settings into the UI
            if(SettingManager.Settings != null)
            {
                int resolutionIndex = SettingManager.Instance.AvailableResolutions.IndexOf(SettingManager.Settings.Resolution);
                _resolutionControl.SetValue(resolutionIndex);

                int screenModeIndex = SettingManager.SUPPORTED_WINDOW_MODES.IndexOf(SettingManager.Settings.fullScreenMode);
                _screenSettingControl.SetValue(screenModeIndex);

                _masterVolume.SetValue(SettingManager.Settings.masterVolume);
                _musicVolume.SetValue(SettingManager.Settings.musicVolume);
                _effectVolume.SetValue(SettingManager.Settings.sfxVolume);
            }
            //continue here
            _resolutionControl.ValueChanged += SettingManager.Instance.SetNewResolution;
            _screenSettingControl.ValueChanged += SettingManager.Instance.SetScreenSetting;

            _masterVolume.ValueChanged += SettingManager.Instance.SetMasterVolume;
            _musicVolume.ValueChanged += SettingManager.Instance.SetMusicVolume;
            _effectVolume.ValueChanged += SettingManager.Instance.SetEffectVolume;

        }
        private void OnDisable()
        {
            _applyVideoSettingsButton.onClick.RemoveListener(SettingManager.Instance.ApplyVideoSettings);

            _resolutionControl.ValueChanged -= SettingManager.Instance.SetNewResolution;
            _screenSettingControl.ValueChanged -= SettingManager.Instance.SetScreenSetting;
            _masterVolume.ValueChanged -= SettingManager.Instance.SetMasterVolume;
            _musicVolume.ValueChanged -= SettingManager.Instance.SetMusicVolume;
            _effectVolume.ValueChanged -= SettingManager.Instance.SetEffectVolume;

            SettingManager.Instance.SaveSettings();
        }

        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------

        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

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

        

        // --- Properties -------------------------------------------------------------------------------------------------

        // --- Unity Functions --------------------------------------------------------------------------------------------
        private void Start()
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
            EventSystem.current.SetSelectedGameObject(_resolutionControl.gameObject);

            _resolutionControl.ValueChanged += SettingManager.Instance.SetNewResolution;
            _screenSettingControl.ValueChanged += SettingManager.Instance.SetScreenSetting;
            _masterVolume.ValueChanged += SettingManager.Instance.SetMasterVolume;
            _musicVolume.ValueChanged += SettingManager.Instance.SetMusicVolume;
            _effectVolume.ValueChanged += SettingManager.Instance.SetEffectVolume;

        }
        private void OnDisable()
        {
            _resolutionControl.ValueChanged -= SettingManager.Instance.SetNewResolution;
            _screenSettingControl.ValueChanged -= SettingManager.Instance.SetScreenSetting;
            _masterVolume.ValueChanged -= SettingManager.Instance.SetMasterVolume;
            _musicVolume.ValueChanged -= SettingManager.Instance.SetMusicVolume;
            _effectVolume.ValueChanged -= SettingManager.Instance.SetEffectVolume;
        }



        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------

        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}
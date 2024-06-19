using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using static LJ.UI.SettingsMenu;

namespace LJ
{
	public class SettingManager : MonoBehaviour
	{
        // --- Enums ------------------------------------------------------------------------------------------------------

        // --- Fields -----------------------------------------------------------------------------------------------------
        [SerializeField] private AudioMixer _mixer;

        private Resolution[] _availableResolutions;

        private Resolution _currentResolution;
        private FullScreenMode _currentScreenSetting;
        private RefreshRate _currentRefreshRate;

        private float _masterVolume;
        private float _musicVolume;
        private float _effectVolume;

        public static readonly FullScreenMode[] SUPPORTED_WINDOW_MODES = new FullScreenMode[]
        {
            FullScreenMode.ExclusiveFullScreen, 
            FullScreenMode.FullScreenWindow, 
            FullScreenMode.Windowed
        };

        public static readonly string[] WINDOW_MODE_NAMES = new string[]
        {
            "Full screen",
            "Borderless window",
            "Window",
        };

        // --- Properties -------------------------------------------------------------------------------------------------
        public static SettingManager Instance { get; private set; }
        public Resolution[] AvailableResolutions {  get { return _availableResolutions; } }


        // --- Unity Functions --------------------------------------------------------------------------------------------
        private void Awake()
        {
            if(Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            _availableResolutions = Screen.resolutions;
            _currentResolution = Screen.currentResolution;
            _currentRefreshRate = Screen.currentResolution.refreshRateRatio;
            _currentScreenSetting = Screen.fullScreenMode;

            //subscribe to Value Changed events
        }

        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------
        public void SetNewResolution(int newValue)
        {
            Debug.Log($"Set New Resolution to {_availableResolutions[newValue].width} x {_availableResolutions[newValue].height}");
            Screen.SetResolution(_availableResolutions[newValue].width, _availableResolutions[newValue].height, _currentScreenSetting, _currentRefreshRate);
        }

        internal void SetScreenSetting(int newValue)
        {
            FullScreenMode mode = SUPPORTED_WINDOW_MODES[newValue];
            Debug.Log($"Set Screen Setting to {mode}");
            Screen.SetResolution(_currentResolution.width, _currentResolution.height, mode, _currentRefreshRate);
        }

        internal void SetMasterVolume(float newValue, float percentage)
        {
            float newVolume = Mathf.Lerp(-80f, 20f, percentage);

            Debug.Log($"Set master to {newVolume}");
            _mixer.SetFloat("MasterVolume", newVolume);
        }

        internal void SetMusicVolume(float newValue, float percentage)
        {
            float newVolume = Mathf.Lerp(-80f, 20f, percentage);
            _mixer.SetFloat("MusicVolume", newVolume);
        }

        internal void SetEffectVolume(float newValue, float percentage)
        {
            float newVolume = Mathf.Lerp(-80f, 20f, percentage);
            _mixer.SetFloat("SfxVolume", newVolume);
        }

        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}
using System.IO;
using UnityEngine;
using UnityEngine.Audio;

namespace LJ
{
    public class SettingManager : MonoBehaviour
    {
        // --- Enums ------------------------------------------------------------------------------------------------------

        // --- Fields -----------------------------------------------------------------------------------------------------
        [SerializeField] private AudioMixer _mixer;

        private Resolution[] _availableResolutions;

        private const float MIN_VOLUME = -80f;
        private const float MAX_VOLUME = 0f;

        private const float MIN_VOLUME_SLIDER_VALUE = 0f;
        private const float MAX_VOLUME_SLIDER_VALUE = 10f;

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

        private const string FILE_NAME = "settings.json";

        private Settings _settings;

        // --- Properties -------------------------------------------------------------------------------------------------
        public static SettingManager Instance { get; private set; }
        public static Settings Settings => Instance != null ? Instance._settings : null;
        public Resolution[] AvailableResolutions { get { return _availableResolutions; } }
        private string SettingsPath => Path.Combine(Application.persistentDataPath, FILE_NAME);

        private Resolution _preSaveRes;
        private FullScreenMode _preSaveMode;


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
            LoadSettings();
            ApplySettings();

            //subscribe to Value Changed events            
        }

        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------
        public void LoadSettings()
        {
            if(File.Exists(SettingsPath))
            {
                string json = File.ReadAllText(SettingsPath);
                _settings = JsonUtility.FromJson<Settings>(json);
            }
            else
            {
                _settings = Settings.CreateDefault();
                SaveSettings();
            }
        }

        public void SaveSettings()
        {
            if(_settings == null)
            {
                Debug.LogError($"Cant save NULL");
                return;
            }
            //try catch error
            string json = JsonUtility.ToJson(_settings, true);
            File.WriteAllText(SettingsPath, json);
        }

        public void ApplySettings()
        {
            ApplyVideoSettings();
            ApplyAudioSettings();
        }

        // --------------------------------------------------------------------------------------------
        public void SetNewResolution(int newValue)
        {
            Debug.Log($"Set New Resolution to {_availableResolutions[newValue].width} x {_availableResolutions[newValue].height}");
            _preSaveRes.width = _availableResolutions[newValue].width;
            _preSaveRes.height = _availableResolutions[newValue].height;
            //ApplyVideoSettings();
        }

        internal void SetScreenSetting(int newValue)
        {
            _preSaveMode = SUPPORTED_WINDOW_MODES[newValue];
            //ApplyVideoSettings();
        }

        internal void SetMasterVolume(float newValue, float percentage)
        {
            _settings.masterVolume = (int)newValue;
            ApplyAudioSettings();
        }

        internal void SetMusicVolume(float newValue, float percentage)
        {
            _settings.musicVolume = (int)newValue;
            ApplyAudioSettings();
        }

        internal void SetEffectVolume(float newValue, float percentage)
        {
            _settings.sfxVolume = (int)newValue;
            ApplyAudioSettings();
        }

        public void ApplyVideoSettings()
        {
            if(_preSaveRes.width > 0 && _preSaveRes.height > 0f)
            {
                _settings.resolutionX = _preSaveRes.width;
                _settings.resolutionY = _preSaveRes.height;
            }

            _settings.fullScreenMode = _preSaveMode;
            Debug.Log($"Set Screen Setting to {_preSaveMode}");

            Screen.SetResolution(_settings.resolutionX, _settings.resolutionY, _settings.fullScreenMode, _settings.RefreshRate);
        }
        // --- Protected/Private Methods ----------------------------------------------------------------------------------        

        private void ApplyAudioSettings()
        {
            SetVolume("MasterVolume", _settings.masterVolume);
            SetVolume("MusicVolume", _settings.musicVolume);
            SetVolume("SfxVolume", _settings.sfxVolume);
        }

        private void SetVolume(string parameter, float value)
        {
            float t = Mathf.InverseLerp(MIN_VOLUME_SLIDER_VALUE, MAX_VOLUME_SLIDER_VALUE, value);
            float newVolume = Mathf.Lerp(MIN_VOLUME, MAX_VOLUME, t);
            _mixer.SetFloat(parameter, newVolume);
        }

        // --------------------------------------------------------------------------------------------
    }
}
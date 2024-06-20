using UnityEngine;

namespace LJ
{
    [System.Serializable]
    public class Settings
    {
        // --- Enums ------------------------------------------------------------------------------------------------------		

        // --- Fields -----------------------------------------------------------------------------------------------------
        // Video settings
        public FullScreenMode fullScreenMode;
        public int resolutionX;
        public int resolutionY;
        public uint refreshRateNumerator;
        public uint refreshRateDenominator;

        // Audio settings
        public int masterVolume;
        public int musicVolume;
        public int sfxVolume;

        // --- Properties -------------------------------------------------------------------------------------------------
        public Resolution Resolution => new()
        {
            width = resolutionX,
            height = resolutionY,
            refreshRateRatio = RefreshRate
        };

        public RefreshRate RefreshRate => new()
        {
            numerator = refreshRateNumerator,
            denominator = refreshRateDenominator,
        };

        // --- Constructors -----------------------------------------------------------------------------------------------
        public Settings()
        {
        }

        public static Settings CreateDefault()
        {
            Settings s = new();

            s.fullScreenMode = Screen.fullScreenMode;
            s.SetResolution(Screen.currentResolution);

            s.masterVolume = 7;
            s.musicVolume = 7;
            s.sfxVolume = 7;

            return s;
        }

        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------
        public void SetResolution(Resolution r)
        {
            resolutionX = r.width;
            resolutionY = r.height;
            refreshRateNumerator = r.refreshRateRatio.numerator;
            refreshRateDenominator = r.refreshRateRatio.denominator;
        }

        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}
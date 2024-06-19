using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LJ
{
	public class VolumeController : Selectable, IMoveHandler, ISelectHandler, IDeselectHandler
    {
        // --- Enums ------------------------------------------------------------------------------------------------------

        // --- Fields -----------------------------------------------------------------------------------------------------
        [SerializeField] private Image _highlight;
        [SerializeField] private Slider _volumeSlider;

        [SerializeField] private float _maxVolume = 10;
        [SerializeField] private float _startVolume = 10;
        [SerializeField] private float _volumeStepSize = 1;

        public delegate void ValueChangedEvent(float newValue, float percentage);
        public event ValueChangedEvent ValueChanged;

        // --- Properties -------------------------------------------------------------------------------------------------

        private float Percentage => Mathf.InverseLerp(0f, _maxVolume, _volumeSlider.value);

        // --- Unity Functions --------------------------------------------------------------------------------------------
        protected override void Awake()
        {
            base.Awake();
            _volumeSlider = GetComponentInChildren<Slider>();

            _volumeSlider.maxValue = _maxVolume;
            _volumeSlider.value = _startVolume;

            ValueChanged?.Invoke(_volumeSlider.value, Percentage);
        }

        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------
        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            _highlight.enabled = true;
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            _highlight.enabled = false;
        }

        public override void OnMove(AxisEventData eventData)
        {
            Vector2 movement = eventData.moveVector;
            if(movement.x != 0f && movement.y == 0f)
            {
                float change = movement.x > 0 ? _volumeStepSize : -_volumeStepSize;
                _volumeSlider.value += change;

                ValueChanged?.Invoke(_volumeSlider.value, Percentage);
            }
            else
            {
                base.OnMove(eventData);
            }
        }
        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}
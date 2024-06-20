using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LJ.UI
{
    public class CarouselControl : Selectable, IMoveHandler, ISelectHandler, IDeselectHandler
    {
        // --- Enums ------------------------------------------------------------------------------------------------------

        // --- Fields -----------------------------------------------------------------------------------------------------
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private Image _highlight;
        [SerializeField] private List<string> _options = new();
        private int _selectedIndex = 0;

        public delegate void ValueChangedEvent(int optionIndex);
        public event ValueChangedEvent ValueChanged;

        // --- Properties -------------------------------------------------------------------------------------------------

        // --- Unity Functions --------------------------------------------------------------------------------------------
        protected override void Awake()
        {

        }

        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------
        public override void OnMove(AxisEventData eventData)
        {
            Vector2 movement = eventData.moveVector;
            if(movement.x != 0f && movement.y == 0f)
            {
                // TODO: Go to previous/next option
                _selectedIndex = movement.x > 0 ? _selectedIndex+=1 : _selectedIndex-=1 ;

                if(_selectedIndex >= _options.Count)
                {
                    _selectedIndex = 0;
                }
                if(_selectedIndex < 0)
                {
                    _selectedIndex = _options.Count - 1;
                }
                _label.text = _options[_selectedIndex].ToString();

                ValueChanged?.Invoke(_selectedIndex);
            }
            else
            {
                base.OnMove(eventData);
            }
        }

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

        public void SetOptions(IEnumerable<string> newOptions)
        {
            _options.Clear();
            _selectedIndex = 0;
            _options.AddRange(newOptions);
            
        }

        public void SetValue(int index)
        {
            if(_options.Count == 0)
                return;

            _selectedIndex = Mathf.Clamp(index, 0, _options.Count - 1);
            _label.text = _options[_selectedIndex].ToString();
        }

        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}
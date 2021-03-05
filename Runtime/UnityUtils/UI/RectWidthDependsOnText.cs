﻿using System;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityUtils.UI
{
    public class RectWidthDependsOnText : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] private float margin;
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private TextMeshProUGUI text;
#pragma warning restore 0649

        private Action _onUpdate;
        
        private void Awake()
        {
            OnTextChange(text);
            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChange);
        }

        private void OnDestroy()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChange);
        }

        private void Update() => _onUpdate?.Invoke();

        private void OnTextChange(Object obj)
        {
            if(obj != text) return;
            _onUpdate = UpdateRectWidth;
        }

        private void UpdateRectWidth()
        {
            var sizeDelta = rectTransform.sizeDelta;
            sizeDelta.x = margin + text.GetRenderedValues(true).x;
            rectTransform.sizeDelta = sizeDelta;
            _onUpdate = null;
        }
    }
}
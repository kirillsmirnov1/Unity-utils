﻿using UnityEngine;
using UnityEngine.UI;

namespace UnityUtils.Variables.Input
{
    public class IntArrayVariableInputEntry : MonoBehaviour
    {
        [SerializeField] private InputField input;
        
        private int _index;
        private IntArrayVariableInput _parent;
        
        public void Fill(IntArrayVariableInput parent, int i, int val)
        {
            _parent = parent;
            _index = i;
            input.text = val.ToString();
        }

        public void OnEdit(string str) 
            => _parent.OnEntryEdit(_index, int.Parse(str));

        public void OnPlusPressed()
        {
            // TODO
        }
        
        public void OnMinusPressed()
        {
            // TODO
        }
        
        public void OnXPressed()
        {
            // TODO
        }
    }
}
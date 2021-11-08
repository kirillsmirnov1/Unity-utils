﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityUtils.Variables;

namespace UnityUtils.Saves
{
    [CreateAssetMenu(menuName = "Variables/SaveFile", fileName = "new SaveFile", order = 0)]
    public class SaveFile : InitiatedScriptableObject
    {
        [SerializeField] private bool logSave;
        [SerializeField] private AVariable[] vars;

        private readonly object _lockable = new object();
        private string SaveFileName => name;

        private readonly Dictionary<string, int> _guidToVar = new Dictionary<string, int>();

        public override void Init()
        {
            InitDictionary();
            ReadSave();
            SubscribeToChanges();
        }

        private void SubscribeToChanges() // IMPR other subscription modes 
        {
            foreach (var variable in vars)
            {
                variable.OnChangeBase += WriteSave;
            }
        }

        private void InitDictionary()
        {
            for (int i = 0; i < vars.Length; i++)
            {
                _guidToVar.Add(vars[i].SaveFileName, i);
            }
        }

        private void ReadSave()
        {
            var serializationPairs = SaveIO
                .ReadObjectAsJsonString<SaveFileSerialized>(SaveFileName, _lockable, logSave)
                .pairs;

            foreach (var serializationPair in serializationPairs)
            {
                ReadSaveToVariable(serializationPair);
            }
        }

        private void ReadSaveToVariable(SerializationPair serializationPair)
        {
            var variableIndex = _guidToVar[serializationPair.name];
            var variableRaw = vars[variableIndex];

            var serializedData = serializationPair.data;
            var data = variableRaw.IsPrimitive
                ? Convert.ChangeType(serializedData, variableRaw.Type) // IMPR no need to get it every time
                : JsonUtility.FromJson(serializedData, variableRaw.Type);
            
            variableRaw.Set(data);
        }

        private void WriteSave()
        {
            // TODO combine variables
            // TODO write JSON
        }
        
        [Serializable]
        public struct SaveFileSerialized
        {
            public SerializationPair[] pairs;
        }
        
        [Serializable]
        public struct SerializationPair
        {
            public string name;
            public string data;
        }
    }
}
using System;
using System.Collections.Generic;
using Meta.Configs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Meta.ConfigOdin
{
    [CreateAssetMenu(fileName = "NewUnit", menuName = "ScriptableObject/MetaConfig/Units")]
    public class UnitConfigOdin : ConfigElem
    {
        public TypeUnitCalculation TypeCalculation;
        
        [ShowIf("TypeCalculation", TypeUnitCalculation.Formula)]
        [SerializeField, HideLabel] public UnitPowerByFormula PowerByFormula = new UnitPowerByFormula();
        
        [ShowIf("TypeCalculation", TypeUnitCalculation.Table)]
        [SerializeField, HideLabel] public UnitPowerByTable PowerByTable = new UnitPowerByTable();

    }

    
    [System.Serializable]
    public class UnitPowerByFormula
    {
        public string Melee;
        public string Range;
        public string Health;
    }
    
    
    [System.Serializable]
    public class UnitPowerByTable
    {
        [Button("Add Level")]
        public void AddLevel()
        {
            var level = Table.Count > 0 ? Table[^1].Level + 1 : 1;
            Table.Add(new Info {Level = level});
        }
        
        [TableList(AlwaysExpanded = true), HideLabel]
        public List<Info> Table = new List<Info>();
        
        [Serializable]
        public class Info
        {
            public int Level;
            public int Melee;
            public int Range;
            public int Health;
        }
    }



}
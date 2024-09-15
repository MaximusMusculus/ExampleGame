using System;
using System.Collections.Generic;
using Meta.Models;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Meta.ConfigOdin
{
    /// <summary>
    /// Теперь есть вопрос. Как их этого - получить то, что нужно мете? (поллекция по которой бегаю или рефлексия?)
    /// ,0
    /// </summary>
    public interface IEntitiesCollection
    {
    }

    [Serializable]
    public class EntitiesCollection : IEntitiesCollection
    {
        [SerializeReference] [HideReferenceObjectPicker] [LabelText("Changes")] [ListDrawerSettings(ShowFoldout = false)]
        public List<IEntitiesCollection> Changes = new List<IEntitiesCollection>();


        [HorizontalGroup("Create")]
        [Button("AddResource")]
        public void Resource()
        {
            Changes.Add(new EntityResource());
        }

        [HorizontalGroup("Create")]
        [Button("AddUnit")]
        public void Unit()
        {
            Changes.Add(new EntityUnit());
        }
    }


    public class EntityResource : IEntitiesCollection
    {
        [HorizontalGroup("Split"), HideLabel] public ResourceConfigOdin Resource;

        [VerticalGroup("Split/Right"), HideLabel] [HorizontalGroup("Split/Right/Count")]
        public int Count;

        [HorizontalGroup("Split/Right/Count"), Button("10")]
        public void Count10() => Count = 10;

        [HorizontalGroup("Split/Right/Count"), Button("50")]
        public void Count50() => Count = 50;

        [HorizontalGroup("Split/Right/Count"), Button("100")]
        public void Count100() => Count = 100;
    }

    public class EntityUnit : IEntitiesCollection
    {
        [HorizontalGroup("Split"), HideLabel] public UnitConfigOdin Unit;
        
        [Range(0, 100)]
        [VerticalGroup("Split/Right")] public int Count;

        [BoxGroup("Split/Right/Stats"), LabelWidth(200), HideLabel]
        public UnitProgressionDto Progression;
    }
}
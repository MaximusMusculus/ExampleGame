using System;
using System.Collections.Generic;
using Meta.Models;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Demos.RPGEditor;
using UnityEngine;


namespace Meta.ConfigOdin
{
    public interface IConfigComponent 
    {
    }
    public class TechInfo : IConfigComponent
    {
        public string TechName;
        public Sprite Icon;
    }
    
    public class ConfigElem
    {
        public ulong Id;
        
        [SerializeReference][HideReferenceObjectPicker]
        public List<IConfigComponent> Components = new List<IConfigComponent>();
    }
    
    


    /// <summary>
    /// Позволяет работать с разными конфигами
    /// Конфиг для мета-данных. Помимо мета конфига игры может хранить вспомогательные данные для редактора
    /// например следующий id для индексера. 
    /// </summary>
    [CreateAssetMenu(fileName = "MetaConfigOdin", menuName = "ScriptableObject/MetaConfigOdin")]
    public class MetaConfigOdin : SerializedScriptableObject
    {
        [ReadOnly] public readonly IdProviderDto IdProviderData = new IdProviderDto();
        
        //Hash?
        [HideInInspector] public List<ResourceConfigOdin> Resources = new List<ResourceConfigOdin>();
        [HideInInspector] public List<UnitConfigOdin> Units = new List<UnitConfigOdin>();
        public List<QuestConfigOdin> Quests = new List<QuestConfigOdin>();
        //public List<> QuestStory - цепочки квестов

        //Conver->MetaConfig
        public override int GetHashCode()
        {
	        return HashHelper.GetHashCode(Resources, Units, Quests);
	        //если id provider data не менялась, то элементы не добавлялись.
        }

        [Button("Open Editor")]
        public void OpenConfigEditor()
        {
	        MetaOdinConfigInspector.Open(this);
        }
    }
    

    public class ResourceConfigOdin : ConfigElem
    {
        //[ReadOnly] //indexer? + type?
        [ReadOnly]
        public ulong ResourceType=>Id;
        
        [HorizontalGroup]
        public string TechName;
        //поля локализации и описания можно не добавлять сюда, а добавлять в соответствующие массивы, а тут отображать как связанные данные. 

        public override int GetHashCode() => ResourceType.GetHashCode();

        [HorizontalGroup]
        [HideLabel, PreviewField(55)]
        public Sprite icon;
        
        
    }

    

    public class QuestConfigOdin //
    {
        [ReadOnly]
        public ulong QuestType; //уникальный id
        public string TechName; //поле, которое будет отображать техническое имя для удобства настройки
        
        [SerializeReference]
        public IReward Reward; //выпадашка с типом награды

        public override int GetHashCode() => HashHelper.GetHashCode(QuestType, Reward);
    }
    
    


    public interface IReward
    {
    }

    public class RewardEquip : IReward
    {
        public CharacterEquipment StartingEquipment;
        public Armor Armor;
        public override int GetHashCode() => HashHelper.GetHashCode(StartingEquipment.Body, StartingEquipment.Head, StartingEquipment.MainHand);
    }

    public class ListRewards : IReward
    {
        [SerializeReference]
        [HideReferenceObjectPicker]
        public List<IReward> Rewards = new List<IReward>();
        public override int GetHashCode() => HashHelper.GetHashCode(Rewards);
    }
    
    public class AddResource : IReward
    {
        //[ResourceSelectedAttribute
        //Вывод как ссылка (указываю на ресурс)
        
        //[GetDropDownValues("GetResourceTypes")]  .... по идее окно - является единственным, поэтому можно в атрибуте заюзать статику...?
        //Тут, как то надо 
        
        [ValueDropdown("TreeViewOfInts", ExpandAllMenuItems = true)]
        public ulong ResourceType; 
        public int Count;
        
       
        private ValueDropdownList<ulong> TreeViewOfInts()
        {
            return (ValueDropdownList<ulong>) MetaOdinConfigInspector.DropDownElemProvider.GetDropDownElems();
        }
        
        
        //RootLink or InterfaceDropDownElems (- self)
        public override int GetHashCode()
        {
	        return HashHelper.GetHashCode(ResourceType, Count);
        }
    }

    public class AddUnit : IReward
    {
        //[SelectedAttribute(typeof(UnitConfigOdin))] <-- ссылка на архитип юнита
        public UnitPreset UnitPreset;
        public int Count;

        public override int GetHashCode() => HashHelper.GetHashCode(UnitPreset, Count);
    }

    [Serializable]
    public class UnitPreset
    {
        public ulong UnitType;
        [HideReferenceObjectPicker]
        public UnitProgressionDto Progression;
    }

    public class Item : IReward
    {
        [HideLabel]
        public ConsumableItem ConsumableItem;
        
        [HideLabel]
        public int count;
    }

  
}
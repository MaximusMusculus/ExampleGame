using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


namespace Meta.ConfigOdin
{
    //использовать маппер
    [CreateAssetMenu(fileName = "NewMetaConfig", menuName = "ScriptableObject/MetaConfig/Config")]
    public class MetaConfigOdin : SerializedScriptableObject
    {
        public List<ResourceConfigOdin> Resources = new List<ResourceConfigOdin>();
        public List<BuildingsConfigOdin> Buildings = new List<BuildingsConfigOdin>();
        public List<UnitConfigOdin> Units = new List<UnitConfigOdin>();
        public List<QuestConfigOdin> Quests = new List<QuestConfigOdin>();
        public List<MissionsConfigOdin> Missions = new List<MissionsConfigOdin>();
      

        [Button("Open Editor")]
        public void OpenConfigEditor()
        {
	        MetaConfigInspector.Open(this);
        }
        //CreateButtons
    }
    // Как это превпатить в MetaConfig
    
    

  
}
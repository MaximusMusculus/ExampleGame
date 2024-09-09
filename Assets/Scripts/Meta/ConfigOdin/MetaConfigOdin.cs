using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


namespace Meta.ConfigOdin
{
    [CreateAssetMenu(fileName = "NewMetaConfig", menuName = "ScriptableObject/MetaConfig/Config")]
    public class MetaConfigOdin : SerializedScriptableObject
    {
        public List<ResourceConfigOdin> Resources = new List<ResourceConfigOdin>();
        public List<UnitConfigOdin> Units = new List<UnitConfigOdin>();
        public List<QuestConfigOdin> Quests = new List<QuestConfigOdin>();
        //public List<> QuestStory - цепочки квестов

        
        [Button("Open Editor")]
        public void OpenConfigEditor()
        {
	        MetaOdinConfigInspector.Open(this);
        }
        
        //CreateButtons
    }
    

  
}
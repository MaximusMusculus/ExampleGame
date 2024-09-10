using Sirenix.OdinInspector;
using UnityEngine;

namespace Meta.ConfigOdin
{
    public class ConfigElem  : ScriptableObject
    {
        [HorizontalGroup("Split", 60)]
        [HideLabel, PreviewField(55)]
        public Texture Icon;
        
        [HorizontalGroup("Split/Right"), LabelWidth(80)]
        public string Name;
    }
}
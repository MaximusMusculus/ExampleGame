using Sirenix.OdinInspector;
using UnityEngine.Serialization;


namespace Meta.ConfigOdin
{
    public class ResourceConfigOdin : ConfigElem
    {
        public bool IsNoLimit;
        
        [FormerlySerializedAs("Limit")] [HideIf("IsNoLimit")]
        public int StartLimit;
        public int StartCount;
        
        public int MaxCount => IsNoLimit? int.MaxValue : StartLimit;
        public int DefaultCount => StartCount;
    }
}
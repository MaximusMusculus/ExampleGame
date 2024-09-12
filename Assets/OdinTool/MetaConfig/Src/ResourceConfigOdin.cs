using Sirenix.OdinInspector;


namespace Meta.ConfigOdin
{
    public class ResourceConfigOdin : ConfigElem
    {
        public bool IsNoLimit;
        
        [HideIf("IsNoLimit")]
        public int StartLimit;
        public int StartCount;
        
        public int MaxCount => IsNoLimit? int.MaxValue : StartLimit;
        public int DefaultCount => StartCount;
    }
}
using System.Collections.Generic;
using Meta.ConfigOdin;



namespace OdinTool.MetaConfig.Src
{
    /// <summary>
    /// Прогрессия стоимости апгрейда
    /// </summary>
    public class PriceProgressionConfigOdin : ConfigElem
    {
        
        public List<PriceElem> PriceElems = new List<PriceElem>();
    }

    [System.Serializable]
    public class PriceElem
    {
        public int Level;
        public EntitiesCollection Price;
    }

}
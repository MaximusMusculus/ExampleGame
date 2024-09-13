using System.Collections.Generic;

namespace Meta.Configs
{
    public class ChangesArrayConfig : ChangeConfig
    {
        public List<ChangeConfig> Changes = new List<ChangeConfig>();
        public override TypeChange TypeChange => TypeChange.ChangesArray;
    }
}
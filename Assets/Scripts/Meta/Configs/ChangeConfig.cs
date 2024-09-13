using System.Collections.Generic;
using AppRen;
using Meta.Models;

namespace Meta.Configs
{
    public enum TypeChange
    {
        AddItem,
        SpendItem,
        AddUnit,
        ChangesArray,
    }
    
    public abstract class ChangeConfig
    {
        public abstract TypeChange TypeChange { get; }
    }
    
    public class ChangesConfig : ChangeConfig
    {
        public List<ChangeConfig> Changes = new List<ChangeConfig>();
        public override TypeChange TypeChange => TypeChange.ChangesArray;
    }

    public class ConfigChangeAddItem : ChangeConfig
    {
        public override TypeChange TypeChange => TypeChange.AddItem;
        public Id TargetItem;
        public int Count;
    }

    public class ConfigChangeSpendItem : ChangeConfig
    {
        public override TypeChange TypeChange => TypeChange.SpendItem;
        public Id TargetItem;
        public int Count;
    }

    public class ConfigChangeAddUnit : ChangeConfig
    {
        public override TypeChange TypeChange => TypeChange.AddUnit;
        public Id UnitType;
        public UnitProgressionDto Progression;
        public int Count;
    }

}
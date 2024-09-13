using AppRen;

namespace Meta.Configs
{
    public class ChangeSpendItemConfig : ChangeConfig
    {
        public override TypeChange TypeChange => TypeChange.SpendItem;
        public Id TargetItem;
        public int Count;
    }
}
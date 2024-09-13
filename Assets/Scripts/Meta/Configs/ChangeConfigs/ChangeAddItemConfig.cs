using AppRen;

namespace Meta.Configs
{
    public class ChangeAddItemConfig : ChangeConfig
    {
        public override TypeChange TypeChange => TypeChange.AddItem;
        public Id TargetItem;
        public int Count;
    }
}
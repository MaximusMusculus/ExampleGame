using AppRen;

namespace Meta.Configs
{
    //это конфиг или dto? Сам по себе он не является конфигом, но может быть в конфигах.
    //чуть дальше станет ясно. Пока тут побудет.
    //как триггерить события системы в шину?
    //  нужно будет каунтеров, ачивок.
    //  возможно в квестах - по типу потратьте золото.
    //? Visitor ?
    public class ChangeAddItemConfig : ChangeConfig
    {
        public override TypeChange TypeChange => TypeChange.AddItem;
        
        public Id TargetItem;
        public int Count;

        //как еще отображать view - в стоимости. 
    }
}
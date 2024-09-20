using Meta.Configs.Actions;

namespace Meta.Configs
{
    
    // Игровое действие, имеет проверки, меняет стейт?
    public class ActionConfig
    {
        public IConditionConfig Require;
        public IActionConfig Spend => _spend;
        public IActionConfig Add => _add;

        
        private ActionCollectionConfig _spend;
        private ActionCollectionConfig _add;
    }

    //как можно представить игровые действия?
    //как можно представить группу игровых действий?
    //как их группировать?
    //как с ними работать?
    
    
    //Вызов экшена пока статичен. Когда он станет динамичным - когда появятся самостоятельные сущности по типу эвентов и баттл пассов
    //тогда будет создан их контроллер и дто.
}
using AppRen;
using Meta.Configs;
using Meta.Configs.Actions;
using Meta.Controllers;
using Meta.Models;

namespace Meta.Tests.Editor.Controllers
{
    //?? такие же группы на кондишены
    public class TestActionsProcessor
    {
        private IActionProcessor _actionProcessor;
        public void Test()
        {

            var itemAction = new ItemActionConfig
            {
                MetaAction = TypeMetaAction.InventoryItemAdd,
                TypeItem = new Id(1),
                Count = 10
            };
            _actionProcessor.Process(itemAction);
            
            var unitAction = new UnitActionConfig
            {
                MetaAction = TypeMetaAction.UnitAdd,
                TypeUnit = new Id(1),
                Progression = new UnitProgressionDto(),
                Count = 10
            };
            
            _actionProcessor.Process(unitAction);
        }
    }
}
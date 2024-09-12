using System.Collections.Generic;
using AppRen;
using Meta.Configs;
using Meta.Controllers;
using Meta.Models;
using NUnit.Framework;


namespace Meta.Tests.Editor.Controllers
{
    [TestFixture]
    public class UnitPurchasedTests
    {
        private IInventoryController _itemsController;
        private IUnitsController _unitsController;

        private readonly Id _itemRecruits = 1;
        private readonly Id _itemIron = 2;
        private readonly Id _unitType = 3;
        
        private List<ItemDto> _itemsDto;
        private List<UnitDto> _unitsDto;


        [SetUp]
        public void SetUp()
        {
            _itemsDto = new List<ItemDto>();
            _unitsDto = new List<UnitDto>();
            
            var itemsConfigs = new List<ItemConfig>
            {
                new ItemConfig {Item = _itemRecruits, DefaultCount = 100, MaxCount = int.MaxValue},
                new ItemConfig {Item = _itemIron, DefaultCount = 150, MaxCount = int.MaxValue}
            };

            var unitConfigs = new List<UnitConfig>
            {
                new UnitConfig {UnitType = _unitType, IsCanUpgrade = true},
            };
            
            _itemsController = new InventoryController(itemsConfigs, _itemsDto);
            _unitsController = new UnitsController(unitConfigs, _unitsDto);
            
            //?? сущность покупки или обмена, которая объединит эти 2 контроллера. 
        }
        
        
        
        
    }
}
using System;
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
            //?? где магазин с ценами?
        }

        public interface IChangeVisitor
        {
            void DoChange(ChangeItem changeItem);
            void DoChange(ChangeAddUnit changeAddItem);
            void DoChange(ChangeSpendUnit changeSpendUnit);
        }
        
        public class MetaControllerFacade : IChangeVisitor
        {
            private readonly IInventoryController _inventoryController;
            private readonly IUnitsController _unitsController;

            public MetaControllerFacade(IInventoryController inventoryController, IUnitsController unitsController)
            {
                _inventoryController = inventoryController;
                _unitsController = unitsController;
            }

            public void DoChange(ChangeItem changeItem)
            {
                switch (changeItem.Operation)
                {
                    case TypeOperation.Add:
                        _inventoryController.Add(changeItem.Target, changeItem.Count);
                        break;
                    case TypeOperation.Subtract:
                        _inventoryController.Spend(changeItem.Target, changeItem.Count);
                        break;
                }
            }

            public void DoChange(ChangeAddUnit changeAddItem)
            {
                _unitsController.Add(changeAddItem.UnitType, changeAddItem.Progression, changeAddItem.Count);
            }
            public void DoChange(ChangeSpendUnit changeSpendUnit)
            {
                _unitsController.Spend(changeSpendUnit.UnitModel, changeSpendUnit.Count);
            }
        }
        
        public interface IChange
        {
            void DoChange(IChangeVisitor visitor);
        }

        public class ChangeDto
        {
            public Id Target;
            public TypeOperation Operation;
            public int Count;
        }
        
        public enum TypeOperation
        {
            Set,        //установить
            Add,        //добавить +
            Subtract,   //вычесть -
            //Multiply,   //умножить *
            //Divide      //разделить /
        }
        public class ChangeItem : IChange
        {
            private ChangeDto _changeDto;//??
            
            public Id Target => _changeDto.Target;
            public TypeOperation Operation => _changeDto.Operation;
            public int Count => _changeDto.Count;
            
            public void DoChange(IChangeVisitor visitor)
            {
                visitor.DoChange(this);
            }
        }

        public class ChangeAddUnit : IChange
        {
            public Id UnitType { get; private set; }
            public UnitProgressionDto Progression { get; private set; }
            public int Count { get; private set; }

            public ChangeAddUnit(Id unitType, UnitProgressionDto progression, int count)
            {
                UnitType = unitType;
                Progression = progression;
                Count = count;
            }

            public void DoChange(IChangeVisitor visitor)
            {
                visitor.DoChange(this);
            }
        }
        
        public class ChangeSpendUnit : IChange
        {
            public IUnitModel UnitModel { get; private set; } //так же можно найти юнита по сочетанию Id  и прогрессии. Но, если я делаю таску с юнитом, то этот подход сломается =(
            public int Count { get; private set; }

            public ChangeSpendUnit(IUnitModel unitModel, int count)
            {
                UnitModel = unitModel;
                Count = count;
            }

            public void DoChange(IChangeVisitor visitor)
            {
                visitor.DoChange(this);
            }
        }


        //Тест успешной покупки юнита -наемника
        //Тест покупки юнита, когда не хватает ресурсов -
        //тест успешного создания юнита игроком
        //таст создания юнита игроком, когда не хватает ресурсов
        //тест создания юнита игроком, когда достигнут лимит (какой и где?) - статика в конфиге?
    }

    
}
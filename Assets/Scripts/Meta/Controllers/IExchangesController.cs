using System;
using System.Collections.Generic;
using System.Linq;
using AppRen;
using Meta.Configs;
using Meta.Models;

namespace Meta.Controllers
{
    public interface IExchangesControllers
    {
        Id Add(ExchangeConfig config);
        void Remove(Id item);
        void DoExchange(Id item);
        
        IEnumerable<ExchangeController> GetExchanges();  
    }


    /// <summary>
    /// Описание того, как работают покупки.
    /// Наем юнитов, покупка юнитов, покупка чего либо.
    /// Все сторонние объекты, кот хотят, могут добавлять свои кусочки в список покупок.
    /// </summary>
    public class ExchangeController
    {
        public Id ItemId { get; }
        public ExchangeConfig Config { get;}
        public ExchangeController(Id itemId)
        {
            ItemId = itemId;
        }
        
        //+DoExchange?  Spend, Add, Requirements
        //+task?<- формирует и запускает таск, который черех Х времени сделает У действие.
        //таск содержит только ту часть, в которой добавляется ресурс.

        private IChangeVisitor _spendChangeVisitor; //SpendVisitor 
        private IChangeVisitor _addChangeVisitor; //AddVisitor

        private ChangesCheckCanSpendController _changesCheckCanSpendController;

        public bool Check()
        {
            //Константные 
            using (_changesCheckCanSpendController)
            {
                _changesCheckCanSpendController.Visit(Config.Spend);
                return _changesCheckCanSpendController.Result;
            }
        }

        public void DoExchange()
        {
            //способ выполнить набор действий не создаваю мелких сущностей и не прибегая к кастингу.
            _spendChangeVisitor.Visit(Config.Spend);
            //если это таска - то создаем таску с передаче ей конфига.
            //таска сделает, то, что мы попросили через Х времени.
            _addChangeVisitor.Visit(Config.Add);
        }
    }
    
    
    
    //-- 
    
    public class ChangesCheckCanSpendController : IChangeVisitor, IDisposable
    {
        private readonly IInventoryController _inventoryController;
        private readonly IUnitsController _unitsController;
        public bool Result { get; private set; }

        public ChangesCheckCanSpendController(IInventoryController inventoryController, IUnitsController unitsController)
        {
            _inventoryController = inventoryController;
            _unitsController = unitsController;
        }

        private void Reset()
        {
            Result = false;
        }
            
        public void Dispose()
        {
            Reset();
        }
            
        public void Visit(ChangeItemConfig changeItemConfig)
        {
            Result = _inventoryController.GetCount(changeItemConfig.TypeItem) >= changeItemConfig.Count;
        }
            
        public void Visit(ChangeUnitConfig changeItem)
        {
            var unit = _unitsController.GetUnits().FirstOrDefault(s=>s.UnitType == changeItem.TypeUnit);
            if (unit == null)
            {
                Result = false;
                return;
            }
            Result = unit.Count >= changeItem.Count;
        }

        public void Visit(IEnumerable<IChangeEntityConfig> collection)
        {
            Result = true; 
            foreach (var entity in collection)
            {
                entity.Visit(this);
                if (!Result)
                {
                    break; // Выходим из цикла, если Result стал false
                }
            }
        }
            
    }
    
    /// <summary>
    /// Сущность, содержит логику взаимодействия. Конкретно эта - то, как списать предметы
    /// </summary>
    public class ChangesSpendController : IChangeVisitor
    {
        private readonly IInventoryController _inventoryController;
        private readonly IUnitsController _unitsController;

        public ChangesSpendController(IInventoryController inventoryController, IUnitsController unitsController)
        {
            _inventoryController = inventoryController;
            _unitsController = unitsController;
        }

        public void Visit(ChangeItemConfig changeItemConfig)
        {
            _inventoryController.Spend(changeItemConfig.TypeItem, changeItemConfig.Count);
        }
        
        public void Visit(ChangeUnitConfig changeItem)
        {
            //так же можно найти юнитов с учетом прогрессии.
            var unit =_unitsController.GetUnits().FirstOrDefault(s => s.UnitType == changeItem.TypeUnit);
            _unitsController.Spend(unit, changeItem.Count);
        }
        
        public void Visit(IEnumerable<IChangeEntityConfig> collection)
        {
            foreach (var entity in collection)
            {
                entity.Visit(this);
            }
        }
    }

    /// <summary>
    /// Сущность, содержит логику взаимодействия. Конкретно эта - как добавить предметы
    /// </summary>
    public class ChangesAddController : IChangeVisitor
    {
        private readonly IInventoryController _inventoryController;
        private readonly IUnitsController _unitsController;

        public ChangesAddController(IInventoryController inventoryController, IUnitsController unitsController)
        {
            _inventoryController = inventoryController;
            _unitsController = unitsController;
        }

        public void Visit(ChangeItemConfig changeItemConfig)
        {
            _inventoryController.Add(changeItemConfig.TypeItem, changeItemConfig.Count);
        }
        public void Visit(ChangeUnitConfig changeItem)
        {
            _unitsController.Add(changeItem.TypeUnit, changeItem.Progression, changeItem.Count);
        }

        public void Visit(IEnumerable<IChangeEntityConfig> collection)
        {
            foreach (var entity in collection)
            {
                entity.Visit(this);
            }
        }
    }

    
    
    
    

    public class ExchangeTaskController
    {
        //добавляет в req проверку на таск, если таск запущен - то не запускаем обмен.
        //при выполнении - делает кост. 
        //и создает такск на выполнение Адд. 
        //т.е. таска знает AddVisitor, время и id предмета.
    }

    public interface IExchangeFactory
    {
        
    }



    /// <summary>
    /// Все покупки тут. Наем юнитов - тоже покупка.
    /// </summary>
    public class ExchangesControllers : IExchangesControllers
    {
        private List<ExchangeItemDto> _exchanges;
        private List<ExchangeConfig> _exchangesConfigs;
        //idProvider
        
        public ExchangesControllers(List<ExchangeConfig> exchangesConfigs, List<ExchangeItemDto> exchanges)
        {
            _exchanges = exchanges;
            _exchangesConfigs = exchangesConfigs;
        }

        //а может быть несколько одинаковых эксченджей? Наверное - нет. В этом нет смысла.
        public Id Add(ExchangeConfig config)
        {
            //добавить айтем в пул
            throw new System.NotImplementedException();
        }
        
        public void Remove(Id item)
        {
            //удалить шопайтем из пула
            throw new System.NotImplementedException();
        }
        
        //Ui->CmdPurchase(Id,count);
        //CmdPurchase(Id,count)->PurchaseItem(Id);
        //PurchaseItem(Id)->CheckRequirements;/??
        //PurchaseItem(Id)->SpendChanges/AddChanges;
        
        public void DoExchange(Id item)
        {
            throw new System.NotImplementedException();
            //находим предмет. Юзаем его.
        }

        public IEnumerable<ExchangeController> GetExchanges()
        {
            throw new System.NotImplementedException();
        }
    }
}
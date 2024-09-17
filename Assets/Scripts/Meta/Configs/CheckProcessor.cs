using System.Collections.Generic;
using AppRen;
using Meta.Controllers;

namespace Meta.Configs.Check
{
    public enum TypeCheck
    {
        InventoryItemsCount,
        InventoryItemsLimit,
        UnitsCount,
        //Level,
        //maps
        //...
    }

    public interface IArgs
    {
        TypeCheck TypeCheck { get; }
    }

    
    public class CheckItemArgs : IArgs
    {
        public TypeCheck TypeCheck { get; set; }
        public Id TypeItem;
        public int CompareType;
        public int Value;
    }

    public interface IChecker
    {
        bool Check(IArgs args);
        //getText?
        //getToast?
    }
    public abstract class CheckAbstract<TArgs> : IChecker where TArgs : IArgs, new()
    {
        public bool Check(IArgs args)
        {
            //кастниг всего один да еще и автоматический.
            return Check((TArgs) args); 
        }
        protected abstract bool Check(TArgs args);
        
        //если потребуется
        public static TArgs CreateEmptyArgs()
        {
            return new TArgs();
        }
    }

    public class CheckInventoryItemCount: CheckAbstract<CheckItemArgs>
    {
        private readonly IInventoryController _inventoryController;

        public CheckInventoryItemCount(IInventoryController inventoryController)
        {
            _inventoryController = inventoryController;
        }

        protected override bool Check(CheckItemArgs args)
        {
            var count = _inventoryController.GetCount(args.TypeItem);
            //+compare
            return count > args.Value; 
        }
    }
    public class CheckInventoryLimit: CheckAbstract<CheckItemArgs>
    {
        private readonly IInventoryController _inventoryController;

        public CheckInventoryLimit(IInventoryController inventoryController)
        {
            _inventoryController = inventoryController;
        }

        protected override bool Check(CheckItemArgs args)
        {
            var limit = _inventoryController.GetLimit(args.TypeItem);
            return limit > args.Value;
        }
    }
    
    //может быть центральным, где все регают в него нужные чекеры
    //может быть разбит по кусочкам и реализовать интерфейс IChecker, где внутри пробегается по мелким чекерам, спрашивая, могут ли они обработать аргсу
    public class CheckProcessor : IChecker
    {
        private readonly Dictionary<TypeCheck, IChecker> _checkers = new Dictionary<TypeCheck, IChecker>();

        public CheckProcessor(IInventoryController inventoryController)
        {
            _checkers.Add(TypeCheck.InventoryItemsCount, new CheckInventoryItemCount(inventoryController));
            _checkers.Add(TypeCheck.InventoryItemsLimit, new CheckInventoryLimit(inventoryController));
        }

        public void AddChecker(TypeCheck typeCheck, IChecker checker)
        {
            _checkers.Add(typeCheck, checker);
        }

        public bool Check(IArgs args)
        {
            if (_checkers.TryGetValue(args.TypeCheck, out var checker))
            {
                return checker.Check(args);
            }
            throw new System.ArgumentException($"Checker not found for {args.TypeCheck}");
        }
    }


    public class CheckUse
    {
        private IChecker _checkProcessor;
        public void Test()
        {
            _checkProcessor.Check(new CheckItemArgs {TypeCheck = TypeCheck.InventoryItemsCount, TypeItem = 1, Value = 50});
        }
    }

}
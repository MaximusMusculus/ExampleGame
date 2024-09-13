using System.Collections.Generic;
using Meta.Configs;

namespace Meta.Controllers
{
    public interface IChangeController
    {
        void Process(ChangeConfig change);
    }
    
    public class ChangeController : IChangeController
    {
        private readonly IChangeControllersFactory _changeControllersFactory;
        private readonly Dictionary<TypeChange, IChangeController> _controllersHash;

        public ChangeController(IChangeControllersFactory changeControllersFactory)
        {
            _changeControllersFactory = changeControllersFactory;
            _controllersHash = new Dictionary<TypeChange, IChangeController>(16)
            {
                {TypeChange.ChangesArray, new ChangeArrayController(this)}
            };
        }

        public void Process(ChangeConfig change)
        {
            if (_controllersHash.TryGetValue(change.TypeChange, out var processor) == false)
            {
                processor = _changeControllersFactory.Create(change);
                _controllersHash.Add(change.TypeChange, processor);
            }
            processor.Process(change);
        }
        
        private class ChangeArrayController : IChangeController
        {
            private readonly IChangeController _changeController;

            public ChangeArrayController(IChangeController changeController)
            {
                _changeController = changeController;
            }

            public void Process(ChangeConfig change)
            {
                var config = (ChangesArrayConfig) change;
                foreach (var changeConfig in config.Changes)
                {
                    _changeController.Process(changeConfig);
                }
            }
        }
    }
}
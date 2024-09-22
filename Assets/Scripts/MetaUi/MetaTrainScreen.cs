using UnityEngine;

namespace MetaUi
{
    public enum TypeUiEvent
    {
        None,
        TrainUnit,
    }

    public interface IUiEvent
    {
        
    }
    public interface IUiEventHandler
    {
        void HandleEvent(IUiEvent uiEvent);
    }

    public interface IUiEvtHandler<in TEvent> where TEvent : IUiEvent
    {
        void Handle(TEvent uiEvent);
    }
    
    
    public class MetaTrainScreen : MonoBehaviour, IUiEvtHandler<TrainUiEvent>
    {
        private  IUiEventHandler _parentEventHandler;


        public void Handle(TrainUiEvent uiEvent)
        {
            _parentEventHandler.HandleEvent(uiEvent);
        }
    }
}
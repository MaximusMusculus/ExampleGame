using UnityEngine;


namespace MetaUi
{
    //UI events?
    public interface IUiEventHandler<in TEvent>
    {
        void HandleUiEvent(TEvent evt);
    }


    public class MetaScreen : MonoBehaviour
    {
        
    }
}
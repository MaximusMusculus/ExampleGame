using System.Linq;
using Meta.Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace MetaUi
{
    public class MetaQuestWidget : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _notifyLabel;
        [SerializeField] private GameObject _notifyVisible;
        
        private IQuests _quests;

        public void Setup(IQuests quests)
        {
            Assert.IsNull(_quests);
            _quests = quests;
            UpdateView();
        }

        public void UpdateView()
        {
            var completed = _quests.GetAll().Count(s => s.IsCompleted);
            _notifyVisible.SetActive(completed > 0);
            _notifyLabel.text = $"{completed}";
        }

        public void OnQuestWidgetClick()
        {
            
        }
        
    }
}
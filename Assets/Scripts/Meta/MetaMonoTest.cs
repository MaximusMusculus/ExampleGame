using Meta.Configs;
using Meta.Models;
using Meta.TestConfiguration;
using UnityEngine;

namespace Meta
{
    public class MetaMonoTest : MonoBehaviour
    {
        private MetaModel _model;
        private MetaActionConfig _trainUnit;
        private MetaActionConfig _costUnit;

        protected void Awake()
        {
            var config = new MetaConfigForTestGameplay().GetConfig();
            var data = new MetaDto();
            _model = new MetaModel(config, data);
            _trainUnit = config.Actions[0];
            _costUnit = config.Actions[1];
        }

        public void Update()
        {
            _model.RunAction(_trainUnit);
            _model.RunAction(_costUnit);
        }
        
        private void TestTrainAndCost(int count)
        {
            for (var i = 0; i < count; i++)
            {
                _model.RunAction(_trainUnit);
            }

            for (int i = 0; i < count; i++)
            {
                _model.RunAction(_costUnit);
            }
        }
    }
}
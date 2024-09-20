using System.Linq;
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
        private MetaDto _metaDto;

        protected void Awake()
        {
            var config = new MetaConfigForTestGameplay().GetConfig();
            _metaDto = new MetaDto();
            _model = new MetaModel(config, _metaDto);
            _trainUnit = config.Actions[0];
            _costUnit = config.Actions[1];
        }

        public void Update()
        {
            _model.RunAction(_trainUnit);
            //Debug.Log(_metaDto.Units.First().Count);
            _model.RunAction(_costUnit);
            //Debug.Log(_metaDto.Units.First().Count);
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
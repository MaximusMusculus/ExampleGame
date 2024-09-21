using System.Linq;
using Meta.Configs;
using Meta.Models;
using Meta.TestConfiguration;
using UnityEngine;
using UnityEngine.Assertions;

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
            var config = new MetaConfigProviderTestBig().GetConfig(); 
                //new MetaConfigDevelopProvider().GetConfig();
            _metaDto = new MetaDto();
            _model = new MetaModel(config, _metaDto);
            _trainUnit = config.Actions[0];
            _costUnit = config.Actions[1];
            
            var actionAddUnits1000 = config.Actions[2];
            _model.RunAction(actionAddUnits1000);
        }

        public void Update()
        {
            _model.RunAction(_trainUnit);
            Assert.AreEqual(1,_metaDto.Units.Last().Count);
            
            _model.RunAction(_costUnit);
            Assert.AreEqual(0,_metaDto.Units.Last().Count);
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
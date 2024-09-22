using System.Linq;
using AppRen;
using Meta.Models;
using Meta.TestConfiguration;
using UnityEngine;
using UnityEngine.Assertions;

namespace Meta
{
    public class MetaMonoTest : MonoBehaviour
    {
        private MetaModel _model;
        private MetaDto _metaDto;

        private Id _actionGroup;
        private int _trainUnit;
        private int _costUnit;

        protected void Awake()
        {
            var config = new MetaConfigProviderTestBig().GetConfig();
            //new MetaConfigDevelopProvider().GetConfig();
            _metaDto = new MetaDto();
            _model = new MetaModel(config, _metaDto);
            _actionGroup = MapTestId.GroupBarracs.Id();
            _trainUnit = 0;
            _costUnit = 1;
            var addUnits1000 = 2;
            _model.DoAction(_actionGroup, addUnits1000);
            //_model.RunAction(actionAddUnits1000);
        }

        public void Update()
        {
            //_model.RunAction(_trainUnit);
            _model.DoAction(_actionGroup, _trainUnit);
            Assert.AreEqual(1,_metaDto.Units.Last().Count);
            
            //_model.RunAction(_costUnit);
            _model.DoAction(_actionGroup, _costUnit);
            Assert.AreEqual(0,_metaDto.Units.Last().Count);
        }
    }
}
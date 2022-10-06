using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels
{
    public class CSmallTypeViewModel
    {
        private SmallType _smallType;
        public SmallType smallType
        {
            get { return _smallType; }
            set { _smallType = value; }
        }
        public CSmallTypeViewModel()
        {
            _smallType = new SmallType();
        }
        public int SmallTypeId 
        {
            get { return _smallType.SmallTypeId; }
            set { _smallType.SmallTypeId = value; }
        }
        public string SmallTypeName
        {
            get { return _smallType.SmallTypeName; }
            set { _smallType.SmallTypeName = value; }
        }
        public int BigTypeId
        {
            get { return _smallType.BigTypeId; }
            set { _smallType.BigTypeId = value; }
        }
    }
}

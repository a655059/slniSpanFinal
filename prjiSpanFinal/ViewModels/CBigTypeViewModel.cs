using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels
{
    public class CBigTypeViewModel
    {
        private BigType _bigType;
        public BigType bigType
        {
            get { return _bigType; }
            set { _bigType = value; }
        }
        public CBigTypeViewModel()
        {
            _bigType = new BigType();
        }
        public int BigTypeId 
        {
            get { return _bigType.BigTypeId; }
            set { _bigType.BigTypeId = value; }
        }
        public string BigTypeName
        {
            get { return _bigType.BigTypeName; }
            set { _bigType.BigTypeName = value; }
        }
    }
}

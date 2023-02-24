using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conway_s_Game_Of_Life.Core
{
    public class CellDataModel
    {
        public CellDataModel(object data)
        {
            this.Data = data;
        }

        public object Data { get; set; }
    }
}

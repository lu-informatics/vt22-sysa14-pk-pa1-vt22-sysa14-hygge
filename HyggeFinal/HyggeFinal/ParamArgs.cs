using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyggeFinal
{
    public class ParamArgs
    {
        public ParamArgs(string paramID, object val) { ParamID = paramID; Value = val; } //constructor
        public string ParamID { get; set; } //ParamID property
        public object Value { get; set; } //Value property
    }
}

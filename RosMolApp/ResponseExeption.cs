using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RosMolApp
{
    public class ResponseExeption : Exception
    {
        public ResponseExeption(string errorCode) : base(errorCode) { 
        
        }
    }
}

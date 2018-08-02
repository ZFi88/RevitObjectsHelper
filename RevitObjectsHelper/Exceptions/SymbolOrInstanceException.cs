using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitObjectsHelper.Exceptions
{
    public class SymbolOrInstanceException : ObjectHelperException
    {
        public SymbolOrInstanceException(string message) : base(message)
        {
        }
    }
}
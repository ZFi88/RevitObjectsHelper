using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitObjectsHelper.Exceptions
{
    public class ParameterNotFoundException : ObjectHelperException
    {
        public ParameterNotFoundException(string message) : base(message)
        {
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitObjectsHelper.Exceptions
{
    public class NoClassficationException : ObjectHelperException
    {
        public NoClassficationException(string message) : base(message)
        {
        }
    }
}
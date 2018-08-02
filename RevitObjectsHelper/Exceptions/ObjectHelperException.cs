using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitObjectsHelper.Exceptions
{
  public class ObjectHelperException : Exception
  {
    public ObjectHelperException(string message) : base(message)
    {
    }
  }
}
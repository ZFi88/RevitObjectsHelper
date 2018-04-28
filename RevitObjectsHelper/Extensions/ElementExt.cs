using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using RevitObjectsHelper.Exceptions;

namespace RevitObjectsHelper.Extensions
{
  /// <summary>
  /// Extensions for Revit Element class
  /// </summary>
  public static class ElementExt
  {
    /// <summary>
    /// Find parameter by name
    /// </summary>
    /// <param name="e">Revit element</param>
    /// <param name="name">Parameter name</param>
    /// <returns>Returns Parameter object or throws an exception</returns>
    public static Parameter FindParameter(this Element e, string name)
    {
      return e.LookupParameter(name) ??
             throw new ParameterNotFoundException($"No such parameter - \"{name}\"");
    }

    /// <summary>
    /// Find parameter by BuiltInParameter
    /// </summary>
    /// <param name="e">Revit element</param>
    /// <param name="parameter">BuiltInParameter</param>
    /// <returns>Returns Parameter object or throws an exception</returns>
    public static Parameter FindParameter(this Element e, BuiltInParameter parameter)
    {
      return e.get_Parameter(parameter) ??
             throw new ParameterNotFoundException($"No such parameter - \"{parameter}\"");
    }
  }
}
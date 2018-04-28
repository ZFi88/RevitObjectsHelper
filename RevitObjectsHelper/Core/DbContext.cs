using System;
using System.Linq;
using System.Reflection;
using Autodesk.Revit.DB;
using RevitObjectsHelper.Exceptions;

namespace RevitObjectsHelper.Core
{
  /// <summary>
  /// Base class for user context class
  /// </summary>
  public class DbContext
  {
    /// <summary>
    /// Creating user context class
    /// </summary>
    /// <typeparam name="T">Type of user context class</typeparam>
    /// <param name="doc">Current Revit document</param>
    /// <param name="view">Current Revit view</param>
    /// <returns></returns>
    public static T Create<T>(Document doc, View view = null) where T : DbContext, new()
    {
      var type = typeof(T);
      var context = new T();
      var props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
      if (props.Length > 0)
      {
        var dbSetType = typeof(DbObjectSet<>);
        try
        {
          foreach (var p in props)
          {
            if (!p.PropertyType.Name.Contains("DbObjectSet")) continue;
            var q = p.PropertyType.GetGenericArguments()[0];
            var seType = dbSetType.MakeGenericType(q);
            var instance = view != null
                ? Activator.CreateInstance(seType, doc, view)
                : Activator.CreateInstance(seType, doc);
            p.SetValue(context, instance);
          }
        }
        catch (Exception e)
        {
          throw GetHelperException(e) ?? e;
        }
      }

      return context;
    }

    private static Exception GetHelperException(Exception e)
    {
      if (e == null) return null;
      var eInnerException = e.InnerException;
      return e.InnerException is ObjectHelperException ? eInnerException : GetHelperException(eInnerException);
    }
  }
};
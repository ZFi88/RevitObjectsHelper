using System;
using System.Linq;
using System.Reflection;
using Autodesk.Revit.DB;

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
    /// <returns></returns>
    public static T Create<T>(Document doc) where T : DbContext, new()
    {
      var type = typeof(T);
      var context = new T();
      var props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
      if (props.Length > 0)
      {
        var dbSetType = typeof(DbObjectSet<>);
        foreach (var p in props.Where(p => p.PropertyType == dbSetType))
        {
          var q = p.PropertyType.GetGenericArguments()[0];
          var instance = Activator.CreateInstance(dbSetType.MakeGenericType(q), doc);
          p.SetValue(context, instance);
        }
      }

      return context;
    }
  }
};
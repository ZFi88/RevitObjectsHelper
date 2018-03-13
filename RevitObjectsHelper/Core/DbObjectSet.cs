using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autodesk.Revit.DB;
using RevitObjectsHelper.Revit;

namespace RevitObjectsHelper.Core
{
  /// <summary>
  /// Collection of user wrapper classes
  /// </summary>
  /// <typeparam name="T">Type of user wrapper class</typeparam>
  public class DbObjectSet<T> : IEnumerable<T> where T : DbObject, new()
  {
    /// <summary>
    /// Current Revit document
    /// </summary>
    private readonly Document _doc;

    /// <summary>
    /// External Revit event for saving changes
    /// </summary>
    private readonly RevitEvent _rEvent;

    /// <summary>
    /// List of user wrapper objects
    /// </summary>
    private List<T> objects = new List<T>();

    /// <summary>
    /// Private constructor. Initialize _rEvent object
    /// </summary>
    private DbObjectSet()
    {
      _rEvent = new RevitEvent();
    }

    /// <summary>
    /// Constructor. Create new instance of DbObjectSet
    /// </summary>
    /// <param name="doc">Current Revit document</param>
    /// <param name="view">Current view</param>
    public DbObjectSet(Document doc, View view) : this()
    {
      _doc = doc;
      var col = GetCollector(doc, view);
      Init(col);
    }

    /// <summary>
    /// Constructor. Create new instance of DbObjectSet
    /// </summary>
    /// <param name="doc">Current Revit document</param>
    public DbObjectSet(Document doc) : this()
    {
      _doc = doc;
      var col = GetCollector(doc);
      Init(col);
    }

    /// <summary>
    /// Initialize collector
    /// </summary>
    /// <param name="doc">Current Revit document</param>
    /// <param name="view">Revit view</param>
    /// <returns>FilteredElementCollector</returns>
    private FilteredElementCollector GetCollector(Document doc, View view = null)
    {
      var tempObj = new T();
      var cat = tempObj.Category;
      var type = tempObj.Type;
      var isInstance = tempObj.IsInstance();
      var col = view != null ? new FilteredElementCollector(doc, view.Id) : new FilteredElementCollector(doc);
      if (type != null && type != typeof(Element)) col.OfClass(type);
      if (cat != BuiltInCategory.INVALID) col.OfCategory(cat);
      if (isInstance) col.WhereElementIsNotElementType();
      else col.WhereElementIsElementType();
      return col;
    }

    /// <summary>
    /// Add objects in object list
    /// </summary>
    /// <param name="col">Collector of Revit elements</param>
    private void Init(FilteredElementCollector col)
    {
      foreach (var element in col.ToElements())
      {
        var obj = InitObject(element);
        objects.Add(obj);
      }
    }

    /// <summary>
    /// Initial method. Initialize private fields
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    private T InitObject(Element element)
    {
      var obj = new T();
      var privateProps = obj.GetType().BaseType
          .GetFields(BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.Instance);
      if (privateProps.Length > 0)
      {
        var elementProp = privateProps.FirstOrDefault(p => p.Name == "revitElement");
        if (elementProp != null) elementProp.SetValue(obj, element);

        var eventProp = privateProps.FirstOrDefault(p => p.Name == "revitEvent");
        if (eventProp != null) eventProp.SetValue(obj, _rEvent);
      }

      var initMethod =
          obj.GetType().BaseType.GetMethod("Init",
              BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.Instance);
      if (initMethod != null) initMethod.Invoke(obj, null);

      return obj;
    }

    /// <summary>
    /// Saving changes. Call Save method on all objects in collection
    /// </summary>
    /// <param name="saveInEvent">By default true, this is create transaction, set to false for disable creating transaction</param>
    public void Save(bool saveInEvent = true)
    {
      if (saveInEvent) _rEvent.Run(() => objects.ForEach(o => o.Save(false)), _doc, "Save");
      else objects.ForEach(o => o.Save(false));
    }

    public IEnumerator<T> GetEnumerator()
    {
      return ((IEnumerable<T>) objects).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return ((IEnumerable<T>) objects).GetEnumerator();
    }

    public T this[int i]
    {
      get => objects[i];
      set => objects[i] = value;
    }
  }
}
using System;
using System.Reflection;
using Autodesk.Revit.DB;
using RevitObjectsHelper.Attributes;
using RevitObjectsHelper.Exceptions;
using RevitObjectsHelper.Revit;

namespace RevitObjectsHelper.Core
{
  /// <summary>
  /// Base class for user wrapper classes, what represents Revit elements
  /// </summary>
  public abstract class DbObject
  {
    /// <summary>
    /// External event for saving changes 
    /// </summary>
    private RevitEvent revitEvent;

    /// <summary>
    /// Field and property for link to Revit element
    /// </summary>
    private Element revitElement;

    public Element RevitElement => revitElement;

    /// <summary>
    /// Category of element, if not setted by user in Category attribute will be setted to BuiltInCategory.INVALID
    /// </summary>
    public BuiltInCategory Category { get; protected set; } = BuiltInCategory.INVALID;

    /// <summary>
    /// Type of element, if not setted by user in Class attribute will be setted to Element
    /// </summary>
    public Type Type { get; protected set; } = typeof(Element);

    protected DbObject()
    {
      SetCategoryOrType();
    }

    /// <summary>
    /// Saving element changes
    /// </summary>
    /// <param name="saveInEvent">By default true, this is create transaction, set to false for disable creating transaction</param>
    public void Save(bool saveInEvent = true)
    {
      var doc = RevitElement.Document;
      if (saveInEvent) revitEvent.Run(UpdateElement, doc, "Save");
      else UpdateElement();
    }

    /// <summary>
    /// Coping parameters  values from Revit elements to wrapper class properties
    /// </summary>
    private void Init()
    {
      foreach (var info in GetType().GetProperties())
      {
        var parameterName = GetParameterName(info);
        var parameter = RevitElement.LookupParameter(parameterName);
        if (parameterName != null && parameter == null)
          throw new ParameterNotFoundException($"No such parameter - \"{parameterName}\"");
        if (info.PropertyType == typeof(bool))
        {
          info.SetValue(this, parameter.AsInteger() == 1);
        }
        else if (info.PropertyType == typeof(double))
        {
          info.SetValue(this, parameter.AsDouble());
        }
        else if (info.PropertyType == typeof(string))
        {
          info.SetValue(this, parameter.AsString());
        }
        else if (info.PropertyType == typeof(int) && IsElementId(info))
        {
          info.SetValue(this, parameter.AsElementId().IntegerValue);
        }
        else if (info.PropertyType == typeof(int))
        {
          info.SetValue(this, parameter.AsInteger());
        }
      }
    }

    /// <summary>
    /// Updating parameters values in Revit element
    /// </summary>
    private void UpdateElement()
    {
      foreach (var info in GetType().GetProperties())
      {
        var parameterName = GetParameterName(info);
        var parameter = RevitElement.LookupParameter(parameterName);
        if (parameterName != null && parameter == null)
          throw new ParameterNotFoundException($"No such parameter - \"{parameterName}\"");
        try
        {
          if (info.PropertyType == typeof(bool))
          {
            parameter.Set((bool) info.GetValue(this) ? 1 : 0);
          }
          else if (info.PropertyType == typeof(double))
          {
            parameter.Set((double) info.GetValue(this));
          }
          else if (info.PropertyType == typeof(string))
          {
            parameter.Set((string) info.GetValue(this));
          }
          else if (info.PropertyType == typeof(int) && IsElementId(info))
          {
            var value = (int) info.GetValue(this);
            if (value > 0) parameter.Set(new ElementId(value));
          }
          else if (info.PropertyType == typeof(int))
          {
            parameter.Set((int) info.GetValue(this));
          }
        }
        catch
        {
        }
      }
    }

    /// <summary>
    /// Gets parameter name from 
    /// </summary>
    /// <param name="prop"></param>
    /// <returns></returns>
    private string GetParameterName(PropertyInfo prop)
    {
      var attributes = prop?.GetCustomAttributes(typeof(ParameterNameAttribute), false);
      if (attributes.Length > 0) return ((ParameterNameAttribute) attributes[0])?.Name;
      return null;
    }


    /// <summary>
    /// Check what property represents ElementId
    /// </summary>
    /// <param name="prop">Property what must be checked</param>
    /// <returns>Return true if it's ElementId or false if not</returns>
    private bool IsElementId(PropertyInfo prop)
    {
      var attribute = prop?.GetCustomAttribute(typeof(ElementIdAttribute), false);
      if (attribute == null) return false;
      return true;
    }

    /// <summary>
    /// Sets Category and Type fields by reflection by reading Instance or Symbol attributes
    /// </summary>
    private void SetCategoryOrType()
    {
      var catAttributes = GetType().GetCustomAttributes(typeof(CategoryAttribute), false);
      var classAttributes = GetType().GetCustomAttributes(typeof(ClassAttribute), false);
      if (catAttributes.Length == 0 && classAttributes.Length == 0)
        throw new NoClassficationException("Don't set Class or Category attribute");

      if (catAttributes.Length > 0)
      {
        var catAttribute = catAttributes[0];
        if (catAttribute != null) Category = ((CategoryAttribute) catAttribute).Category;
      }

      if (classAttributes.Length > 0)
      {
        var classAttribute = classAttributes[0];
        if (classAttribute != null) Type = ((ClassAttribute) classAttribute).Type;
      }
    }

    /// <summary>
    /// Show what element is instance or symbol
    /// </summary>
    /// <returns>Return true if instance or false if symbol</returns>
    public bool IsInstance()
    {
      var instAttributes = GetType().GetCustomAttributes(typeof(InstanceAttribute), false);
      var symbolAttributes = GetType().GetCustomAttributes(typeof(SymbolAttribute), false);
      if (instAttributes.Length == 0 && symbolAttributes.Length == 0)
        throw new SymbolOrInstanceException("Don't set instance or symbol attribute");
      if (instAttributes.Length != 0 && symbolAttributes.Length != 0)
        throw new SymbolOrInstanceException("Setted instance and symbol attributes");
      if (instAttributes.Length > 0) return true;
      return false;
    }
  }
}
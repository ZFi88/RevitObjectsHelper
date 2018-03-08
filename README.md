# Revit object helper
Simple library for for work with Revit objects in ORM style.

## Instalation
```
PM> Install-Package RevitObjectsHelper
``` 

## Usage
```c#
  //Create class representing Revit element
  [Instance] //Get all instaces of elements, if you want to get types set [Symbol]
  [Class(typeof(Wall))] //Get only walls, also you can set [Category(BuiltInCategory.Walls)]
  public class MyWall : DbObject
  {
    [ParameterName("Comments")] //Bind Revit parameter "Comments" to property Comments
    public String Comments { get; set; }

    [ParameterName("Length")] //Bind Revit parameter "Length" to property Length
    public double Length { get; set; }
  }

  // Create DbContext class
  public class MyContext : DbContext
  {
    //Create property what represents all walls
    public DbObjectSet<MyWall> Walls { get; set; }
  }

  //Use context in your command
  [Transaction(TransactionMode.Manual)]
  [Regeneration(RegenerationOption.Manual)]
  public class CmdCommand : IExternalCommand
  {
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
      var uiApp = commandData.Application;
      var doc = uiApp.ActiveUIDocument.Document; //Get current document

      var context = DbContext.Create<MyContext>(doc); //Create context

      //Iterate walls
      foreach (var wall in context.Walls)
      {
        wall.Comments = "Hello!!!"; //Change comment
        wall.Save(); //Save it! It's generate transaction on each wall
      }

      //Or iterate walls
      foreach (var wall in context.Walls)
      {
        wall.Comments = "Hello!!!"; //Change comment
      }
      context.Walls.Save(); //Save all walls by one transaction

      return Result.Succeeded;
    }
  }
```
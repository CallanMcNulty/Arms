using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;

namespace Arms
{
  public class SaveBlazon
  {
    public int id { get; set;}
    public string name { get; set; }
    public string blazon { get; set; }
    public int shape { get; set; }

    public SaveBlazon(string Name, string Blazon, int Shape, int Id =0)
    {
      id = Id;
      name = Name;
      blazon = Blazon;
      shape = Shape;
    }
    public override bool Equals(System.Object otherSavedBlazon)
    {
      if (!(otherSavedBlazon is SaveBlazon))
      {
      return false;
      }
      else
      {
        SaveBlazon newSavedBlazon = (SaveBlazon) otherSavedBlazon;
        bool idEquality = id == newSavedBlazon.id;
        bool nameEquality = name == newSavedBlazon.name;
        bool blazonEquality = blazon == newSavedBlazon.blazon;
        bool shapeEquality = shape == newSavedBlazon.shape;
        return (idEquality && nameEquality);
      }
    }
    public void Save()
    {
      DBObjects dbo = DBObjects.CreateCommand("INSERT INTO blazons (name, blazon, shape) OUTPUT INSERTED.id VALUES (@savedBlazonName, @savedBlazonBlazon, @savedBlazonShape);", new List<string> {"@savedBlazonName", "@savedBlazonBlazon", "@savedBlazonShape"}, new List<object> {name, blazon, shape});
      SqlDataReader rdr = dbo.RDR;
      rdr = dbo.CMD.ExecuteReader();
      while (rdr.Read())
      {
        this.id = rdr.GetInt32(0);
      }
      dbo.Close();
    }

    public static void DeleteAll()
    {
      DBObjects dbo = DBObjects.CreateCommand("DELETE FROM blazons;");
      dbo.CMD.ExecuteNonQuery();
      dbo.Close();
    }
    public static SaveBlazon Find(int id)
    {
      DBObjects dbo = DBObjects.CreateCommand("SELECT * FROM blazons WHERE id=@Id;", new List<string> {"@Id"}, new List<object> {id});
      SqlDataReader rdr = dbo.RDR;
      rdr = dbo.CMD.ExecuteReader();
      int foundSavedBlazonId = 0;
      string foundSavedBlazonName = null;
      string foundSavedBlazonBlazon = null;
      int foundSavedBlazonShape = 0;
      while (rdr.Read())
      {
        foundSavedBlazonId = rdr.GetInt32(0);
        foundSavedBlazonName = rdr.GetString(1);
        foundSavedBlazonBlazon = rdr.GetString(2);
        foundSavedBlazonShape = rdr.GetInt32(3);
      }
      SaveBlazon foundSavedBlazon = new SaveBlazon(foundSavedBlazonName, foundSavedBlazonBlazon, foundSavedBlazonShape, foundSavedBlazonId);
      dbo.Close();
      return foundSavedBlazon;
    }
    public static List<SaveBlazon> GetAll()
    {
      DBObjects dbo = DBObjects.CreateCommand("SELECT * FROM blazons;");
      SqlDataReader rdr = dbo.RDR;
      rdr = dbo.CMD.ExecuteReader();
      List<SaveBlazon> allBlazons = new List<SaveBlazon>{};

      while(rdr.Read())
      {
        int blazonId = rdr.GetInt32(0);
        string blazonName = rdr.GetString(1);
        string blazonBlazon = rdr.GetString(2);
        int blazonShape = rdr.GetInt32(3);
        SaveBlazon newSavedBlazon = new SaveBlazon(blazonName, blazonBlazon, blazonShape, blazonId);
        allBlazons.Add(newSavedBlazon);
      }
      dbo.Close();
      return allBlazons;
    }
  }
}

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

    public SaveBlazon(string Name, string Blazon, int Id =0)
    {
    id = Id;
    name = Name;
    blazon = Blazon;
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
        return (idEquality && nameEquality);
      }
    }
    public void Save()
    {
      DBObjects dbo = DBObjects.CreateCommand("INSERT INTO blazons (name, blazon) OUTPUT INSERTED.id VALUES (@savedBlazonName, @savedBlazonBlazon);", new List<string> {"@savedBlazonName", "@savedBlazonBlazon"}, new List<object> {name, blazon});
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
      while (rdr.Read())
      {
        foundSavedBlazonId = rdr.GetInt32(0);
        foundSavedBlazonName = rdr.GetString(1);
        foundSavedBlazonBlazon = rdr.GetString(2);
      }
      SaveBlazon foundSavedBlazon = new SaveBlazon(foundSavedBlazonName, foundSavedBlazonBlazon, foundSavedBlazonId);
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
        SaveBlazon newSavedBlazon = new SaveBlazon(blazonName, blazonBlazon, blazonId);
        allBlazons.Add(newSavedBlazon);
      }
      dbo.Close();
      return allBlazons;
    }
  }


}

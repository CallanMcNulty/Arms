using System;
using System.Collections.Generic;

namespace Arms
{
  public class Field
  {
    private string _pattern;
    private string[] _tinctures;
    private static Dictionary<string,string> hexDict = new Dictionary<string,string>{{"gules","#ff0000"},{"argent","#ffffff"}};
    public string pattern
    {
      get
      {
        return _pattern;
      }
    }
    public string[] tinctures
    {
      get
      {
        return _tinctures;
      }
    }
    public Field(string Pattern, string[] Tinctures)
    {
      _pattern = Pattern;
      _tinctures = Tinctures;
    }
    public string GetHex(string tinct)
    {
      return hexDict[tinct];
    }
  }
}

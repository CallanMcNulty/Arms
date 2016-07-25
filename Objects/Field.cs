using System;
using System.Collections.Generic;

namespace Arms
{
  public class Field
  {
    private string _pattern;
    private string[] _tinctures;
    private static Dictionary<string,string> hexDict = new Dictionary<string,string>{{"gules","#ff0000"},{"argent","#dddddd"},
                                                                                     {"sable","#000000"},{"vert","#00ff00"}};
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
      for(int i=0; i<_tinctures.Length; i++)
      {
        _tinctures[i] = hexDict[_tinctures[i]];
      }
    }
    public string GetHex(string tinct)
    {
      return hexDict[tinct];
    }
  }
}

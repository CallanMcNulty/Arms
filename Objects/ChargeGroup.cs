using System;
using System.Collections.Generic;
using Geometry;

namespace Arms
{
  public class ChargeGroup
  {
    private Division _parent;
    private Division[] _chargesDivs;
    private string _layout;
    public Division parent
    {
      get
      {
        return _parent;
      }
    }
    public Division[] chargeDivs
    {
      get
      {
        return _chargesDivs;
      }
    }
    public string layout
    {
      get
      {
        return _layout;
      }
      set
      {
        _layout = value;
      }
    }
    public ChargeGroup(Division Parent, string chargeDevice, int Number=1, Field inputField=null, string Layout="unspecified")
    {
      _parent = Parent;
      _layout = Layout;
      _chargesDivs = new Division[Number];
      for(int i=0; i<_chargesDivs.Length; i++)
      {
        Polygon newChargeDivShape = null;
        Point parentCenter = _parent.shape.GetCenter();
        if(chargeDevice=="fess")
        {
          newChargeDivShape = new Polygon(new List<Point> {new Point(0F,0F),new Point(100F,0F),new Point(100F,100F),new Point(0F,100F)}, 100F, 25F, 0F, parentCenter.Y-12.5F);
        }
        _chargesDivs[i] = new Division(newChargeDivShape);
        _chargesDivs[i].field = inputField;
      }
    }
  }
}

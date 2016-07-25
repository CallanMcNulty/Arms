using System;
using System.Dynamic;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Geometry;

namespace Arms
{
  public class Division
  {
    private Polygon _shape;
    private Division[] _subdivisions;
    private Field _field;
    private List<ChargeGroup> _chargeGroups;
    public Polygon shape
    {
      get
      {
        return _shape;
      }
    }
    public Division[] subdivisions
    {
      get
      {
        return _subdivisions;
      }
    }
    public Field field
    {
      get
      {
        return _field;
      }
      set
      {
        _field = value;
      }
    }
    public List<ChargeGroup> chargeGroups
    {
      get
      {
        return _chargeGroups;
      }
    }
    public Division(Polygon Shape, int subs=0)
    {
      _shape = Shape;
      _subdivisions = new Division[subs];
      _field = null;
      _chargeGroups = new List<ChargeGroup> {};
    }
  }
}

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
      _field = new Field("solid", new string[]{"argent"});
      _chargeGroups = new List<ChargeGroup> {};
    }
    public Division[] ExecuteCommand(List<string> command, string commandType)
    {
      if(commandType=="tincture")
      {
        if(command[0]=="ermine" || (command[0]=="vair" || command[0]=="potent"))
        {
          _field = new Field("fur", command.ToArray());
        }
        else
        {
          _field = new Field("solid", command.ToArray());
        }
      }
      if(commandType=="charge")
      {
        ChargeGroup cg = new ChargeGroup(this, command[1], Int32.Parse(command[0]));
        _chargeGroups.Add(cg);
        return cg.chargeDivs;
      }
      if(commandType=="division")
      {
        string divType;
        if(command[0]=="quarterly")
        {
          divType = "quarterly";
        }
        else
        {
          divType = command[1];
        }
        List<Polygon> shapes = this.shape.PartyPer(divType);
        _subdivisions = new Division[shapes.Count];
        for(int i=0; i<shapes.Count; i++)
        {
          _subdivisions[i] = new Division(shapes[i]);
        }
        return _subdivisions;
      }
      return new Division[] {};
    }
  }
}

using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using Geometry;

namespace Arms
{
  public class ArmsTests
  {
    [Fact]
    public void Test_Division_and_ChargeGroup_Fess()
    {
      List<Point> points = new List<Point> {new Point(0F,0F), new Point(100F,10F), new Point(10F,100F)};
      Polygon testPoly = new Polygon(points, 100F, 100F, 0F, 0F);
      Division div = new Division(testPoly);
      Field fFess = new Field("solid", new string[] {"argent"});
      Field fShield = new Field("solid", new string[] {"gules"});
      div.field = fShield;
      ChargeGroup cg = new ChargeGroup(div, "fess", fFess);
      div.chargeGroups.Add(cg);
      Assert.Equal(4, div.chargeGroups[0].chargeDivs[0].shape.vertices.Count);
    }
  }
}

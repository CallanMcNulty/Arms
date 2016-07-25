using System.Collections.Generic;
using System.Dynamic;
using System.Collections.ObjectModel;
using System;
using Nancy;
using Nancy.ViewEngines.Razor;
using Geometry;

namespace Arms
{
  public class HomeModule : NancyModule
  {
    private string GenerateHTML(Division division)
    {
      string result = "<div style=' height:"+division.shape.height+"%; "+
                                    "width:"+division.shape.width+"%; "+
                                    "position:absolute; "+
                                    "top:"+division.shape.offsetY+"%; "+
                                    "left:"+division.shape.offsetX+"%; "+
                                    "background-color:"+division.field.tinctures[0]+"; "+
                                    "-webkit-clip-path:polygon(";
      foreach(Point vertex in division.shape.vertices)
      {
        result = result + vertex.X.ToString()+"% "+vertex.Y.ToString()+"%, ";
      }
      result = result.Substring(0, result.Length-2);
      result += ");'> ";
      //Console.WriteLine(result);
      foreach(Division sub in division.subdivisions)
      {
        result += GenerateHTML(sub);
      }
      foreach(ChargeGroup cg in division.chargeGroups)
      {
        foreach(Division charge in cg.chargeDivs)
        {
          result += GenerateHTML(charge);
        }
      }
      result += "</div>";
      return result;
    }
    public HomeModule()
    {
      Get["/"] = _ => {
        List<Point> points = new List<Point> {new Point(0F,0F), new Point(0F,80F), new Point(50F,100F), new Point(100F,80F), new Point(100F,0F)};
        Polygon testPoly = new Polygon(points, 100F, 100F, 0F, 0F);
        Division div = new Division(testPoly);
        // Point p = div.shape.GetCenter();
        // Field fFess = new Field("solid", new string[] {"argent"});
        // Field fShield = new Field("solid", new string[] {"gules"});
        // div.field = fShield;
        // ChargeGroup cg = new ChargeGroup(div, "fess", 1, fFess);
        // div.chargeGroups.Add(cg);
        Parser.Parse("per pale sable and per fess gules and vert", div);
        return View["index.cshtml", GenerateHTML(div)];
      };
    }
  }
}

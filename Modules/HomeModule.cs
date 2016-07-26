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
                                    "left:"+division.shape.offsetX+"%; ";
      if(division.subdivisions.Length == 0)
      {
        result += "background-color:"+division.field.tinctures[0]+"; ";
        result += "-webkit-clip-path:polygon(";
        foreach(Point vertex in division.shape.vertices)
        {
          result += vertex.X.ToString()+"% "+vertex.Y.ToString()+"%, ";
        }
        result = result.Substring(0, result.Length-2);
        result += ");";
      }
      result += "'> ";
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
        Parser.Parse("per pale sable one saltire argent and per fess gules one saltire argent and vert one pall argent", div);
        dynamic Model = new ExpandoObject();
        Model.html = GenerateHTML(div);
        return View["index.sshtml", Model];
      };
      Post["blazon"]= _ => {
        string newBlazon = Request.Form["blazon-string"];
        List<Point> points = new List<Point> {new Point(0F,0F), new Point(0F,80F), new Point(50F,100F), new Point(100F,80F), new Point(100F,0F)};
        Polygon testPoly = new Polygon(points, 100F, 100F, 0F, 0F);
        Division div = new Division(testPoly);
        Parser.Parse(newBlazon, div);
        dynamic Model = new ExpandoObject();
        Model.html = GenerateHTML(div);
        return View["index.sshtml", Model];
      };
    }
  }
}

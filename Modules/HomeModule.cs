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
    // private string[] shapeDimensions = {"List<Point> points = new List<Point> {new Point(50F,0F), new Point(100F,50F), new Point(50F,100F), new Point(0F,50F)};
    //     Polygon testPoly = new Polygon(points, 100F, 96F, 0F, 0F);", List<Point> points = new List<Point> {new Point(0F,50F), new Point(10F, 10F), new Point(50F, 0F), new Point(90F, 10F), new Point(100F, 50F), new Point(90F, 90F), new Point(50F, 100F), new Point(10F, 90F)};
    // Polygon testPoly = new Polygon(points, 85F, 96F, 6.5F, 0F);};
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
        Polygon testPoly = new Polygon(points, 100F, 96F, 0F, 0F);
        Division div = new Division(testPoly);
        Parser.Parse("per pale argent and or", div);
        dynamic Model = new ExpandoObject();
        Model.html = GenerateHTML(div);
        Model.shieldShape = 1;
        return View["index.sshtml", Model];
      };
      Post["/blazon"] =_=> {
        string input = Request.Form["blazon-string"];
        string shieldShape = Request.Form["shield-shape"];
        string formattedBlazon = string.Join("+",Parser.FormatBlazon(input));
        string path = "/blazon/"+formattedBlazon+"/shieldShape/"+shieldShape;
        return Response.AsRedirect(path);
      };
      Get["/blazon/{blazon}/shieldShape/{shieldShape}"]= parameter => {
        string input = parameter.blazon;
        string newBlazon = input.Replace("+"," ");
        List<Point> points = new List<Point> {new Point(0F,50F), new Point(10F, 10F), new Point(50F, 0F), new Point(90F, 10F), new Point(100F, 50F), new Point(90F, 90F), new Point(50F, 100F), new Point(10F, 90F)};
        Polygon testPoly = new Polygon(points, 85F, 96F, 6.5F, 0F);
        Division div = new Division(testPoly);
        Parser.Parse(newBlazon, div);
        dynamic Model = new ExpandoObject();
        Model.html = GenerateHTML(div);
        Model.shieldShape = parameter.shieldShape;
        Model.newBlazon = newBlazon;
        return View["index.sshtml", Model];
      };
    }
  }
}

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
        if(division.field.pattern=="solid")
        {
          result += "background-color:"+division.field.tinctures[0]+"; ";
        }
        else if(division.field.pattern=="fur")
        {
          result += "background-image: url("+division.field.imgUrl+"); ";
        }
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
        Parser.Parse("quarterly gules a pile purpure and or a bend azure and vert a chevron bleu-celeste and sable a saltire argent", div);
        dynamic Model = new ExpandoObject();
        Model.html = GenerateHTML(div);
        return View["index.sshtml", Model];
      };
      Post["/blazon"] =_=> {
        string input = Request.Form["blazon-string"];
        string formattedBlazon = string.Join("+",Parser.FormatBlazon(input));
        string path = "/blazon/"+formattedBlazon;
        return Response.AsRedirect(path);
      };
      Get["/blazon/{blazon}"]= parameter => {
        string input = parameter.blazon;
        string newBlazon = input.Replace("+"," ");
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

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
    public static Polygon GetShieldPoly(int shieldNumber)
    {
      Polygon shieldPoly;
      if (shieldNumber == 4)
      {
        List<Point> shieldShape = new List<Point> {new Point(50F,0F), new Point(100F,50F), new Point(50F,100F), new Point(0F,50F)};

         shieldPoly = new Polygon(shieldShape, 100F, 96F, 0F, 0F);
      }
      else if (shieldNumber == 1)
      {
        List<Point> shieldShape = new List<Point>{new Point(50F, 0F), new Point(90F, 10F), new Point(100F, 50F), new Point(90F, 90F), new Point(50F, 100F), new Point(10F, 90F), new Point(0F,50F), new Point(10F, 10F)};

         shieldPoly = new Polygon(shieldShape, 85F, 96F, 6.5F, 0F);
      }
      else
      {
        List<Point> shieldShape = new List<Point>{new Point(0F,0F), new Point(0F,80F), new Point(25, 100), new Point(50F,100F), new Point(75, 100), new Point(100F,80F), new Point(100F,0F)};

         shieldPoly = new Polygon(shieldShape, 85F, 96F, 6.5F, 0F);
      }
      return shieldPoly;
    }
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
      }
      result += "-webkit-clip-path:polygon(";
      foreach(Point vertex in division.shape.vertices)
      {
        result += vertex.X.ToString()+"% "+vertex.Y.ToString()+"%, ";
      }
      result = result.Substring(0, result.Length-2);
      result += ");'";
      if(division.field.pattern=="fur")
      {
        result += "class="+division.field.tinctures[0];
      }
      result += "> ";
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

        string path = "/blazon/per+pale+per+fess+sable+a+chief+or+and+purpure+overall+a+bend+argent+and+vert+11+mullets+argent+overall+a+lozenge+azure/shieldShape/1";
        return Response.AsRedirect(path);
      };
      Post["/blazon"] =_=> {
        string input = Request.Form["blazon-string"];
        string shieldShape = Request.Form["shield-shape"];
        string formattedBlazon = string.Join("+",Parser.FormatBlazon(input));
        string path = "/blazon/"+formattedBlazon+"/shieldShape/"+shieldShape;
        return Response.AsRedirect(path);
      };
      Get["/blazon/{blazon}/shieldShape/{shieldShape}"]= parameter => {
        int shieldNumber = parameter.shieldShape;
        string input = parameter.blazon;
        string newBlazon = input.Replace("+"," ");
        Division div = new Division(GetShieldPoly(shieldNumber));
        dynamic Model = new ExpandoObject();
        Model.outputString = Parser.Parse(newBlazon, div);
        Model.html = GenerateHTML(div);
        Model.shieldShape = parameter.shieldShape;
        Model.newBlazon = newBlazon;
        Model.allBlazons = SaveBlazon.GetAll();
        return View["index.cshtml", Model];
      };
      Post["/save"]= _ => {
        string blazonName = Request.Form["blazon-name"];
        string blazonBlazon = Request.Form["blazon-blazon"];
        SaveBlazon newSaveBlazon = new SaveBlazon(blazonName, blazonBlazon);
        newSaveBlazon.Save();
        string formattedBlazon = blazonBlazon.Replace(" ", "+");
        string path = "/blazon/"+formattedBlazon+"/shieldShape/0";
        return Response.AsRedirect(path);
      };
      Post["/delete"]= _ => {
        SaveBlazon.DeleteAll();
        string path = "/blazon/per+pale+per+fess+sable+a+chief+or+and+purpure+overall+a+bend+argent+and+vert+11+mullets+argent+overall+a+lozenge+azure/shieldShape/1";
        return Response.AsRedirect(path);

      };
    }
  }
}

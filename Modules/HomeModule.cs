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
    public HomeModule()
    {
      Get["/"] = _ => {
        dynamic model = new ExpandoObject();
        model.TestX = "100px";
        model.TestY = "50px";
        model.TestColor = "background-color: red";
        return View["index.sshtml", model];
      };
    }
  }
}

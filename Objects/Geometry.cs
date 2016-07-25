using System;
using System.Dynamic;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Geometry
{
  public class Point
  {
    private float _x;
    private float _y;
    public float X
    {
      get
      {
        return _x;
      }
      set
      {
        _x = value;
      }
    }
    public float Y
    {
      get
      {
        return _y;
      }
      set
      {
        _y = value;
      }
    }
    public Point(float x, float y)
    {
      _x = x;
      _y = y;
    }
    public override bool Equals(System.Object otherPoint)
    {
      if (!(otherPoint is Point))
      {
        return false;
      }
      else
      {
        Point newPoint = (Point)otherPoint;
        return (Math.Abs(_x-newPoint.X) < 0.01F && Math.Abs(_y-newPoint.Y) < 0.01F);
      }
    }
    public override string ToString()
    {
      return "("+_x.ToString()+", "+_y.ToString()+")";
    }
    public Point Copy()
    {
      return new Point(_x, _y);
    }
  }
  public class Line
  {
    private Point _p1;
    private Point _p2;
    private float _m;
    private float _b;
    public Point P1
    {
      get
      {
        return _p1;
      }
    }
    public Point P2
    {
      get
      {
        return _p2;
      }
    }
    public float m
    {
      get
      {
        return _m;
      }
    }
    public float b
    {
      get
      {
        return _b;
      }
    }
    public Line(Point NewP1, Point NewP2)
    {
      _p1 = NewP1;
      _p2 = NewP2;
      _m = (_p2.Y-_p1.Y)/(_p2.X-_p1.X);
      _b = _p1.X*m*-1.0F + _p1.Y;
    }
    public override string ToString()
    {
      return "Line: "+_p1.ToString()+" "+_p2.ToString();
    }
    public float GetLength()
    {
      return (float)Math.Sqrt(Math.Pow((_p2.X-_p1.X),2) + Math.Pow((_p2.Y-_p1.Y),2));
    }
    public float GetXAtYPos(float Y)
    {
      if(_p1.X==_p2.X)
      {
        return _p1.X;
      }
      return (Y-_b)/_m;
    }
    public float GetYAtXPos(float X)
    {
      return _m*X+_b;
    }
    public Point GetIntersection(Line otherLine)
    {
      if(Math.Abs(_m-otherLine.m) < 0.001F)
      {
        return null;
      }
      if(_m==float.PositiveInfinity)
      {
        return new Point(_p1.X, otherLine.m*_p1.X+otherLine.b);
      }
      if(otherLine.m==float.PositiveInfinity)
      {
        return new Point(otherLine.P1.X, _m*otherLine.P1.X+_b);
      }
      float px = (otherLine.b - _b)/(_m - otherLine.m);
      Point result = new Point(px, _m*px+_b);
      return result;
    }
    public bool PointIsOnLine(Point p)
    {
      if(_p1.X==_p2.X && p.X==_p1.X)
      {
        return true;
      }
      return Math.Abs(p.Y-(_m*p.X+_b)) < 0.01F;
    }
  }
  public class Polygon
  {
    private List<Point> _vertices;
    private List<Line> _sides;
    private float _width;
    private float _height;
    private float _offset_x;
    private float _offset_y;
    public List<Point> vertices
    {
      get
      {
        return _vertices;
      }
    }
    public List<Line> sides
    {
      get
      {
        return _sides;
      }
    }
    public float width
    {
      get
      {
        return _width;
      }
    }
    public float height
    {
      get
      {
        return _height;
      }
    }
    public float offsetX
    {
      get
      {
        return _offset_x;
      }
    }
    public float offsetY
    {
      get
      {
        return _offset_y;
      }
    }
    public Polygon(List<Point> Points, float Width, float Height, float OffsetX, float OffsetY)
    {
      _vertices = Points;
      _width = Width;
      _height = Height;
      _offset_x = OffsetX;
      _offset_y = OffsetY;
      _sides = new List<Line> {};
      for(int i=1; i<Points.Count; i++)
      {
        _sides.Add(new Line(Points[i-1], Points[i]));
      }
      _sides.Add(new Line(Points[Points.Count-1], Points[0]));
    }
    public Line[] GetSectionLines(int number, bool horiz)
    {
      Line[] result = new Line[number];
      float chunk = horiz ? _height/(float)(number+1) : _width/(float)(number+1);
      for(int i=1; i<number+1; i++)
      {
        float staticDimension = (float)i*chunk;
        Line straight = horiz ? new Line(new Point(0F,staticDimension), new Point(100F,staticDimension))
                              : new Line(new Point(staticDimension,0F), new Point(staticDimension,100F));
        Dictionary<string,object> intersections = this.GetSideIntersections(straight);
        Point point1 = (Point)intersections["point1"];
        Point point2 = (Point)intersections["point2"];
        float dynamicDimensionLower = horiz ? Math.Min(point1.X, point2.X)
                                            : Math.Min(point1.Y, point2.Y);
        float dynamicDimensionUpper = horiz ? Math.Max(point1.X, point2.X)
                                            : Math.Max(point1.Y, point2.Y);
        if(dynamicDimensionLower < dynamicDimensionUpper)
        {
          result[i-1] = horiz ? new Line(new Point(dynamicDimensionLower, staticDimension), new Point(dynamicDimensionUpper, staticDimension))
                            : new Line(new Point(staticDimension, dynamicDimensionLower), new Point(staticDimension, dynamicDimensionUpper));
        }
        else
        {
          result[i-1] = new Line(new Point(0, 0), new Point(0, 0));
        }
      }
      return result;
    }
    public Point GetCenter()
    {
      int resolution = 1000;
      Line[] horizLines = this.GetSectionLines(resolution, true);
      Line[] vertLines = this.GetSectionLines(resolution, false);
      float horizCenterSum = 0.0F;
      float vertCenterSum = 0.0F;
      for(int i=0; i<resolution; i++)
      {
        horizCenterSum += (horizLines[i].P1.X + horizLines[i].P2.X)/2.0F;
        vertCenterSum += (vertLines[i].P1.Y + vertLines[i].P2.Y)/2.0F;
      }
      float resultX = horizCenterSum / resolution;
      float resultY = vertCenterSum / resolution;
      return new Point(resultX, resultY);
    }
    private Dictionary<string, object> GetSideIntersections(Line intersectLine)
    {
      Point intersection1 = null;
      Line intersectSide1 = null;
      Point intersection2 = null;
      Line intersectSide2 = null;
      foreach(Line side in _sides)
      {
        Point possibleIntersection = intersectLine.GetIntersection(side);
        if(possibleIntersection!=null)
        {
          float lesserX = Math.Min(side.P1.X, side.P2.X);
          float greaterX = Math.Max(side.P1.X, side.P2.X);
          float lesserY = Math.Min(side.P1.Y, side.P2.Y);
          float greaterY = Math.Max(side.P1.Y, side.P2.Y);
          if(lesserX < possibleIntersection.X && possibleIntersection.X < greaterX
            && lesserY < possibleIntersection.Y && possibleIntersection.Y < greaterY)
          {
            if(intersection1==null)
            {
              intersection1 = possibleIntersection;
              intersectSide1 = side;
            }
            else
            {
              intersection2 = possibleIntersection;
              intersectSide2 = side;
            }
          }
        }
      }
      Dictionary<string, object> result = new Dictionary<string, object>();
      result.Add("point1", intersection1);
      result.Add("side1", intersectSide1);
      result.Add("point2", intersection2);
      result.Add("side2", intersectSide2);
      return result;
    }
    private List<Polygon> MakeDivision(Dictionary<string, object> intersections)
    {
      Point point1 = (Point)intersections["point1"];
      Point point2 = (Point)intersections["point2"];
      Line side1 = (Line)intersections["side1"];
      Line side2 = (Line)intersections["side2"];
      //test if new points are already present
      bool p1Overlap = false;
      bool p2Overlap = false;
      foreach(Point vertex in vertices)
      {
        if(vertex==point1)
        {
          p1Overlap = true;
        }
        if(vertex==point2)
        {
          p2Overlap = true;
        }
      }
      //make insertions
      List<Point> finalPointList = new List<Point> {};
      for(int i=0; i<_vertices.Count; i++)
      {
        finalPointList.Add(_vertices[i]);
        bool side1Point1Found = _vertices[i]==side1.P1 || _vertices[i]==side1.P2;
        bool side1Point2Found = _vertices[i==_vertices.Count-1 ? 0:i+1]==side1.P1 || _vertices[i==_vertices.Count-1 ? 0:i+1]==side1.P2;
        bool side2Point1Found = _vertices[i]==side2.P1 || _vertices[i]==side2.P2;
        bool side2Point2Found = _vertices[i==_vertices.Count-1 ? 0:i+1]==side2.P1 || _vertices[i==_vertices.Count-1 ? 0:i+1]==side2.P2;
        if(side1Point1Found && side1Point2Found && !p1Overlap)
        {
          finalPointList.Add(point1);
        }
        else if(side2Point1Found && side2Point2Found && !p2Overlap)
        {
          finalPointList.Add(point2);
        }
      }
      //divide
      List<Point>[] newPolygonPointLists = new List<Point>[2];
      for(int i=0; i<2; i++)
      {
        bool addingMode = (i==0);
        List<Point> currentPointList = new List<Point> {};
        foreach(Point vertex in finalPointList)
        {
          if(vertex==point1 || vertex==point2)
          {
            addingMode = !addingMode;
            currentPointList.Add(vertex.Copy());
          }
          else if(addingMode)
          {
            currentPointList.Add(vertex.Copy());
          }
        }
        newPolygonPointLists[i] = currentPointList;
      }
      //calculate new polygon data
      List<Polygon> result = new List<Polygon> {};
      foreach(List<Point> polygonPoints in newPolygonPointLists)
      {
        float minX = 100F;
        float maxX = 0F;
        float minY = 100F;
        float maxY = 0F;
        foreach(Point vertex in polygonPoints)
        {
          minX = vertex.X < minX ? vertex.X : minX;
          maxX = vertex.X > maxX ? vertex.X : maxX;
          minY = vertex.Y < minY ? vertex.Y : minY;
          maxY = vertex.Y > maxY ? vertex.Y : maxY;
        }
        float newWidth = (maxX-minX) / _width * 100F;
        float newHeight = (maxY-minY) / _height  * 100F;
        foreach(Point vertex in polygonPoints)
        {
          vertex.X = vertex.X - minX;
          vertex.Y = vertex.Y - minY;
          vertex.X = vertex.X * (100/newWidth);
          vertex.Y = vertex.Y * (100/newHeight);
        }
        result.Add(new Polygon(polygonPoints,newWidth,newHeight,minX,minY));
      }
      return result;
    }
    private List<Polygon> Party(Line partition)
    {
      return this.MakeDivision(this.GetSideIntersections(partition));
    }
    public List<Polygon> PartyPer(string partitionType)
    {
      Point center = this.GetCenter();
      if(partitionType=="pale")
      {
        return this.Party(new Line(center, new Point(center.X, 100) ));
      }
      return null;
    }
  }
}

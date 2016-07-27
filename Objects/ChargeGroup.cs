using System;
using System.Collections.Generic;
using Geometry;

namespace Arms
{
  public class ChargeGroup
  {
    private Division _parent;
    private Division[] _chargesDivs;
    private string _layout;
    public Division parent
    {
      get
      {
        return _parent;
      }
    }
    public Division[] chargeDivs
    {
      get
      {
        return _chargesDivs;
      }
    }
    public string layout
    {
      get
      {
        return _layout;
      }
      set
      {
        _layout = value;
      }
    }
    public ChargeGroup(Division Parent, string chargeDevice, int Number=1, Field inputField=null, string Layout="unspecified")
    {
      _parent = Parent;
      _layout = Layout;
      _chargesDivs = new Division[Number];
      Point center = _parent.shape.GetCenter();
      float centerOffsetX = center.X-(_parent.shape.width/2);
      float centerOffsetY = center.Y-(_parent.shape.height/2);
      for(int i=0; i<_chargesDivs.Length; i++)
      {
        Polygon newChargeDivShape = null;
        Point parentCenter = _parent.shape.GetCenter();
        if(chargeDevice=="fess")
        {
          newChargeDivShape = new Polygon(new List<Point> {new Point(0F,0F),new Point(100F,0F),new Point(100F,100F),new Point(0F,100F)}, 100F, 25F, 0F, parentCenter.Y-12.5F);
        }
        else if(chargeDevice=="chief")
        {
          newChargeDivShape = new Polygon(new List<Point> {new Point(0F,0F),new Point(100F,0F),new Point(100F,100F),new Point(0F,100F)}, 100F, 25F, 0F, 0F);
        }
        else if(chargeDevice=="pile")
        {
          newChargeDivShape = new Polygon(new List<Point> {new Point(0F,0F),new Point(center.X,100F),new Point(100F,0F)}, 100F, 80F, 0F, 0F);
        }
        else if(chargeDevice=="pale")
        {
          newChargeDivShape = new Polygon(new List<Point> {new Point(0F,0F),new Point(100F,0F),new Point(100F,100F),new Point(0F,100F)}, 25F, 100F, parentCenter.X-12.5F, 0F);
        }
        else if(chargeDevice=="bend")
        {
          float size = 20F;
          Line bendLine = new Line(center, new Point(center.X+1, center.Y+1));
          Point left = new Point(bendLine.GetXAtYPos(0F), 0F);
          Point right = new Point(bendLine.GetXAtYPos(100F), 100F);
          newChargeDivShape = new Polygon(new List<Point> {new Point(left.X-size,100F),new Point(left.X+size,100F),new Point(right.X+size,0F),new Point(right.X-size,0F)}, 100F, 100F, 0F, 0F);
        }
        else if(chargeDevice=="bend-sinister")
        {
          float size = 20F;
          Line bendLine = new Line(center, new Point(center.X+1, center.Y-1));
          Point left = new Point(bendLine.GetXAtYPos(0F), 0F);
          Point right = new Point(bendLine.GetXAtYPos(100F), 100F);
          newChargeDivShape = new Polygon(new List<Point> {new Point(left.X-size,100F),new Point(left.X+size,100F),new Point(right.X+size,0F),new Point(right.X-size,0F)}, 100F, 100F, 0F, 0F);
        }
        else if(chargeDevice=="saltire")
        {
          float size = 15F;
          Line bendLine = new Line(center, new Point(center.X+1, center.Y+1));
          Line bendSinisterLine = new Line(center, new Point(center.X-1, center.Y+1));
          Point lowerLeft = new Point(bendLine.GetXAtYPos(100F) ,100F);
          Point lowerRight = new Point(bendSinisterLine.GetXAtYPos(100F) ,100F);
          Point upperRight = new Point(bendLine.GetXAtYPos(0F) ,0F);
          Point upperLeft = new Point(bendSinisterLine.GetXAtYPos(0F) ,0F);

          newChargeDivShape = new Polygon(new List<Point> {new Point(lowerLeft.X-size,lowerLeft.Y),new Point(lowerLeft.X+size,lowerLeft.Y),new Point(center.X,center.Y-size),new Point(lowerRight.X-size,lowerRight.Y), new Point(lowerRight.X+size,lowerRight.Y),new Point(center.X+size,center.Y),new Point(upperRight.X+size,upperRight.Y),new Point(upperRight.X-size,upperRight.Y),new Point(center.X,center.Y+size),new Point(upperLeft.X+size,upperLeft.Y),new Point(upperLeft.X-size,upperLeft.Y),new Point(center.X-size,center.Y)}, 100F, 100F, 0F, 0F);
        }
        else if(chargeDevice=="cross")
        {
          float size = 15F;
          newChargeDivShape = new Polygon(new List<Point> {new Point(center.X-size,0F),new Point(center.X+size,0F),new Point(center.X+size,center.Y-size),new Point(100F,center.Y-size),new Point(100F,center.Y+size),new Point(center.X+size,center.Y+size),new Point(center.X+size,100F),new Point(center.X-size,100F),new Point(center.X-size,center.Y+size),new Point(0F,center.Y+size),new Point(0F,center.Y-size),new Point(center.X-size,center.Y-size)}, 100F, 100F, 0F, 0F);
        }
        else if(chargeDevice=="chevron")
        {
          float size = 50F;
          newChargeDivShape = new Polygon(new List<Point> {new Point(0F,100F),new Point(0F,100F-size), new Point(50F,0F),new Point(100F,100F-size),new Point(100F,100F), new Point(50F,size)}, 100F, 30F, 0F, center.Y-15F);
        }
        else if(chargeDevice=="pall")
        {
          float size = 15F;
          Line bendLine = new Line(center, new Point(center.X+1, center.Y+1));
          Line bendSinisterLine = new Line(center, new Point(center.X-1, center.Y+1));
          Point upperRight = new Point(bendLine.GetXAtYPos(0F) ,0F);
          Point upperLeft = new Point(bendSinisterLine.GetXAtYPos(0F) ,0F);

          newChargeDivShape = new Polygon(new List<Point> {new Point(upperLeft.X+size,upperLeft.Y),new Point(upperLeft.X-size,upperLeft.Y),new Point(center.X,center.Y-size),new Point(upperRight.X+size,upperRight.Y),new Point(upperRight.X-size,upperRight.Y),new Point(center.X-size,center.Y),new Point(center.X-size,100F),new Point(center.X+size,100F),new Point(center.X+size,center.Y)}, 100F, 100F, 0F, 0F);//
        }
        else if(chargeDevice=="pall-reversed")
        {
          float size = 15F;
          Line bendLine = new Line(center, new Point(center.X+1, center.Y+1));
          Line bendSinisterLine = new Line(center, new Point(center.X-1, center.Y+1));
          Point left = new Point(bendLine.GetXAtYPos(100F) ,100F);
          Point right = new Point(bendSinisterLine.GetXAtYPos(100F) ,100F);

          newChargeDivShape = new Polygon(new List<Point> {new Point(right.X+size,right.Y),new Point(right.X-size,right.Y),new Point(center.X,center.Y-size),new Point(left.X+size,left.Y),new Point(left.X-size,left.Y),new Point(center.X-size,center.Y),new Point(center.X-size,0F),new Point(center.X+size,0F),new Point(center.X+size,center.Y)}, 100F, 100F, 0F, 0F);
        }
        _chargesDivs[i] = new Division(newChargeDivShape);
        _chargesDivs[i].field = inputField;
      }
    }
  }
}

// using Xunit;
// using System.Collections.Generic;
// using System;
// using System.Data;
// using System.Data.SqlClient;
//
// namespace Geometry
// {
//   public class GeometryTests
//   {
//     [Fact]
//     public void Test_Constructors()
//     {
//       List<Point> points = new List<Point> {new Point(0F,0F), new Point(100F,10F), new Point(10F,100F)};
//       Polygon testPoly = new Polygon(points, 100, 100, 0, 0);
//       Assert.Equal(3, testPoly.sides.Count);
//     }
//     [Fact]
//     public void Test_GetSectionLines()
//     {
//       List<Point> points = new List<Point> {new Point(0F,0F), new Point(100F,10F), new Point(10F,100F)};
//       Polygon testPoly = new Polygon(points, 100, 100, 0, 0);
//       Line[] sections = testPoly.GetSectionLines(4, false);
//       foreach(Line line in sections)
//       {
//         Console.WriteLine("Point 1: "+line.P1.X.ToString()+", "+line.P1.Y.ToString());
//         Console.WriteLine("Point 2: "+line.P2.X.ToString()+", "+line.P2.Y.ToString());
//       }
//       Point center = testPoly.GetCenter();
//       Console.WriteLine("Center: "+center.X.ToString()+", "+center.Y.ToString());
//       Assert.Equal(4, sections.Length);
//     }
//     [Fact]
//     public void Test_PartyPer()
//     {
//       List<Point> points = new List<Point> {new Point(0F,0F), new Point(100F,10F), new Point(10F,100F)};
//       Polygon testPoly = new Polygon(points, 100F, 100F, 0F, 0F);
//       List<Polygon> subs = testPoly.PartyPer("pale");
//       foreach(Polygon sub in subs)
//       {
//         Console.WriteLine("Subdivision Points");
//         foreach(Point vertex in sub.vertices)
//         {
//           Console.WriteLine(vertex.ToString());
//         }
//         Console.WriteLine("Width: "+sub.width.ToString());
//         Console.WriteLine("Height: "+sub.height.ToString());
//         Console.WriteLine("Offset X: "+sub.offsetX.ToString());
//         Console.WriteLine("Offset Y: "+sub.offsetY.ToString());
//       }
//       Assert.Equal(2, subs.Count);
//     }
//   }
// }

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slicer
{
    public class Point
    {
        // coordinates of point in 3D
        public double x;
        public double y;
        public double z;

        // constructor
        public Point(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        // function to subtract two points
        public static Point Subtract(Point point1, Point point2)
        {
            return new Point(point1.x - point2.x,point1.y - point2.y,point1.z - point2.z);
        }
        
    }

    public class Line
    {// line represented as P = t*v + P0
        //  x = a*t + P0.x
        //  y = b*t + P0.y
        //  z = c*t + P0.z
        Point P0;// P0 is a point on the line
        Point v;// v is a vector along direction of line, (a,b,c)

        public Line(Point point1, Point point2)
        {
            P0 = point1;
            v = Point.Subtract(point2,point1);
        }

        /// <summary>
        /// This function calculates the intersection point between a plane and a line. Replace the x,y,z components of the line equation into the plane equation and solve for t, then replace t in the line equation to get the x,y,z
        /// </summary>
        /// <param name="line"></param>
        /// <param name="plane"></param>
        /// <returns></returns>
        public static Point IntersectionLinePlane(Line line, Plane plane)
        {// could write in one line but it'd be too hard to debug, or read!
            // replace the x,y,z components of the line equation into the plane equation and solve for t
            double numerator = -plane.d - plane.a * line.P0.x - plane.b * line.P0.y - plane.c * line.P0.z;
            double denominator = plane.a * line.v.x + plane.b * line.v.y + plane.c * line.v.z;
            double t = numerator / denominator;

            // replace t in the line equation to get the x,y,z
            double x = t * line.v.x + line.P0.x;
            double y = t * line.v.y + line.P0.y;
            double z = t * line.v.z + line.P0.z;

            return new Point(x, y, z);

        }
    }

    public class Segment
    {// segment represented by its start and end points
        Point point1;
        Point point2;

        // constructor
        public Segment(Point point1, Point point2)
        {
            this.point1 = point1;
            this.point2 = point2;
        }
    }

    public class Plane
    {
        // parameters for equation of a plane ax + by + cz + d = 0;
        public double a;
        public double b;
        public double c;
        public double d;

        /// <summary>
        /// get the normal equation of the plane from the 3 points
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <param name="point3"></param>
        public Plane(Point point1, Point point2, Point point3)
        {
            // get the normal equation because that is what we will use
            NormalEquation(point1, point2, point3, out this.a, out this.b, out this.c, out this.d);
        }

        /// <summary>
        /// a plane can also be created with the coefficient of the normal equation
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        public Plane(double a, double b, double c, double d)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
        }

        /// <summary>
        /// this function finds the normal equation for a plane from 3 points inputted. Using 3 points, we can generate 2 lines and by performing the cross-product and obtaining a vector normal to the plane. Then we can solve the normal equation for "d" using one of the points.
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <param name="point3"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        public void NormalEquation(Point point1,Point point2,Point point3, out double a, out double b, out double c, out double d)
        {   // formula for getting the normal equation of a plane in which (a,b,c) is the vector normal to the plane
            // this is the cross-product of two of the lines defined by the 3 points
            // each of the vectors are written from the points
            // we use vector from point 1 to 2 and 1 to 3
            a = (point2.y - point1.y) * (point3.z - point1.z)
                - (point3.y - point1.y) * (point2.z - point1.z);
            b = (point2.z - point1.z) * (point3.x - point1.x)
                - (point3.z - point1.z) * (point2.x - point1.x);
            c = (point2.x - point1.x) * (point3.y - point1.y)
                - (point3.x - point1.x) * (point2.y - point1.y); 

            // solve for d by plugging point1 into the equation ax + by + cz + d = 0;
            d = -(a * point1.x + b * point1.y + c * point1.z);
        }

        /// <summary>
        /// this is the same as the distance but we only want the sign so we skip the division part which is time consuming
        /// </summary>
        /// <param name="point"></param>
        /// <param name="plane"></param>
        /// <returns></returns>
        public static bool SidePointPlane(Point point, Plane plane)
        {
            // side is a bit arbitrary because it depends on definition of the plane's normal vector
            // the equation is distance = (a*x0 + b*y0 + c*z0 + d)/sqrt(a*a + b*b + c*c)
            // since we only want the sign, we can ignore the division part
            // sign(a*x0 + b*y0 + c*z0 + d)
            return (plane.a * point.x + plane.b * point.y + plane.c * point.z + plane.d) >= 0; // return true if positive, false if negative
        }

        /// <summary>
        /// this function calculates th distance between a point and a plane. Using the normal equation, we plug the coordinates of the point and get the distance
        /// </summary>
        /// <param name="point"></param>
        /// <param name="plane"></param>
        /// <returns></returns>
        public double DistancePointPlane(Point point, Plane plane)
        {   // the equation is distance = (a*x0 + b*y0 + c*z0 + d)/sqrt(a*a + b*b + c*c)
            // split the equation in two for ease of reading
            double numerator = (plane.a * point.x + plane.b * point.y + plane.c * point.z + plane.d);
            double denominator = Math.Sqrt(plane.a * plane.a + plane.b * plane.b + plane.c * plane.c);

            return numerator / denominator;
        }

    }

    public class Triangle : Plane // Triangle is a plane that has limits
    {// Triangle represented by its 3 vertices
        Point point1;
        Point point2;
        Point point3;

        public Triangle(Point point1, Point point2, Point point3)
            : base(point1, point2, point3)
        {
            this.point1 = point1;
            this.point2 = point2;
            this.point3 = point3;
        }

        /// <summary>
        /// this function finds the intersection between the triangle and the plane
        /// --> checks if the triangle intersects the plane
        /// --> if it intersects, it will look like a segment
        /// --> the ends of the segment are the intersections of the two edges of the triangle going through the plane and the plane
        /// </summary>
        /// <param name="triangle">the input triangle</param>
        /// <param name="plane">the input plane</param>
        /// <returns>segment if there is intersection, else return null</returns>
        public static Segment TriangleInPlane(Triangle triangle, Plane plane)
        {//
            // find side +/- of each of 3 points of triangle
            bool SidePoint1 = SidePointPlane(triangle.point1, plane);
            bool SidePoint2 = SidePointPlane(triangle.point2, plane);
            bool SidePoint3 = SidePointPlane(triangle.point3, plane);

            Point p1, p2;

            
            // point1 is lonely
            if (SidePoint1 != SidePoint2
                && SidePoint2 == SidePoint3)
            {
                // get lines made by Point1-2 and Point1-3
                Line l12 = new Line(triangle.point1, triangle.point2);
                Line l13 = new Line(triangle.point1, triangle.point3);

                // get intersection of lines and plane
                p1 = Line.IntersectionLinePlane(l12,plane);
                p2 = Line.IntersectionLinePlane(l13, plane);

                // return the segment made of those two points
                return new Segment(p1,p2);
            }
            else if (SidePoint2 != SidePoint1
                && SidePoint1 == SidePoint3)// point2 is lonely
            {
                // get lines made by Point1-2 and Point1-3
                Line l21 = new Line(triangle.point2, triangle.point1);
                Line l23 = new Line(triangle.point2, triangle.point3);

                // get intersection of lines and plane
                p1 = Line.IntersectionLinePlane(l21, plane);
                p2 = Line.IntersectionLinePlane(l23, plane);

                // return the segment made of those two points
                return new Segment(p1, p2);
            }
            else if (SidePoint3 != SidePoint2
                && SidePoint1 == SidePoint2)// point3 is lonely
            {
                // get lines made by Point1-2 and Point1-3
                Line l31 = new Line(triangle.point3, triangle.point1);
                Line l32 = new Line(triangle.point3, triangle.point2);

                // get intersection of lines and plane
                p1 = Line.IntersectionLinePlane(l31, plane);
                p2 = Line.IntersectionLinePlane(l32, plane);

                // return the segment made of those two points
                return new Segment(p1, p2);
            }
            else
            {// both points are on same side of plane, meaning that the triangle is not intersecting the plane
                return null;
            }
        }
    }


}




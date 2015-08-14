using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Slicer
{
    public partial class TestSlicer : Form
    {
        public TestSlicer()
        {
            InitializeComponent();
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {// test the function
            
            // setup test with one triangle and one plane

            // input a set of triangles
            List<Triangle> triangles = new List<Triangle>();
            triangles.Add(new Triangle(new Point(-5, 10, 0), new Point(10, 10, 0), new Point(5, -10, 0)));
            triangles.Add(new Triangle(new Point(-5, 10, 5), new Point(10, 10, 5), new Point(5, -10, 5)));
            triangles.Add(new Triangle(new Point(-3, 10, 4), new Point(1, 10, -3), new Point(10, -13, 8)));
            triangles.Add(new Triangle(new Point(-5, 100, 0), new Point(10, 100, 0), new Point(5, 100, 0)));
            triangles.Add(new Triangle(new Point(-50, 13, 2), new Point(0, -4, 0), new Point(2, -10, 0)));

            // define the plane
            Plane plane = new Plane(-0.5, 1, 0.5, 0);

            // slice using a plane defined by normal equation, the plane does not have bounds
            List<Segment> segments = new List<Segment>();
            foreach (Triangle triangle in triangles)
            {
                try
                {
                    Segment segment = Triangle.TriangleInPlane(triangle, plane);
                    if (segment != null)
                    {
                        segments.Add(segment);
                    }
                }
                catch (Exception ex)
                {
                    textBoxDebug.Text = ex.ToString();
                }
            }

            // segments contain polylines from slicing a set of triangles
            textBoxDebug.Text = "done";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    public LinkedList<Rectangle> Rectangles { get; }

    public CircularCloudLayouter(Point center)
    {
        Rectangles = new LinkedList<Rectangle>();
    }

    public Rectangle PutNextRectangle(Size size)
    {
        var newRectangle = new Rectangle(size);
        Rectangles.AddLast(newRectangle);
        return Rectangles.Last?.Value ?? newRectangle;
    }
}


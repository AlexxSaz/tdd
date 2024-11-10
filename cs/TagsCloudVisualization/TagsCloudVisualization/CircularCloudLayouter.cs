using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private readonly Point _center;
    public LinkedList<Rectangle> Rectangles { get; } = new();

    public CircularCloudLayouter(Point center)
    {
        _center = center;
    }

    public Rectangle PutNextRectangle(Size size)
    {
        Rectangle newRectangle;
        if (Rectangles.Count == 0)
            newRectangle = new Rectangle(size, _center);
        else
            newRectangle = new Rectangle(size, GetFreeCenter(size));
        
        Rectangles.AddLast(newRectangle);
        return Rectangles.Last?.Value ?? newRectangle;
    }

    private static Point GetFreeCenter(Size size)
    {
        throw new NotImplementedException();
    }
}


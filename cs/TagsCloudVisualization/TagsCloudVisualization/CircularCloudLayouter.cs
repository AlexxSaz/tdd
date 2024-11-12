using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter(Point center)
{
    private readonly PointGenerator _pointGenerator = new PointGenerator(center);
    private readonly List<Rectangle> _rectangles = [];

    public Rectangle PutNextRectangle(Size size)
    {
        if (size.Width < 1 || size.Height < 1)
            throw new ArgumentOutOfRangeException(
                $"{nameof(size.Width)} and {nameof(size.Height)} should be more than zero");

        Rectangle newRectangle;
        while (true)
        {
            newRectangle = new Rectangle(_pointGenerator.GetNewPoint(), size);
            if (!_rectangles.Any(rec => newRectangle.IntersectsWith(rec)))
                break;
        }
        _rectangles.Add(newRectangle);
        return newRectangle;
    }
}


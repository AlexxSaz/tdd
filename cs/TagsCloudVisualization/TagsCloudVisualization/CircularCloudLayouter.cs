using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{

    private readonly Point _center;
    private readonly double _spiralStep = 10;
    private readonly double _spiralRotation = 10;
    private double _radius = 0;
    private double _angle = 0;
    private List<Rectangle> _rectangles = new();


    public CircularCloudLayouter(Point center)
    {
        _center = center;
    }

    public Rectangle PutNextRectangle(Size size)
    {
        if (_rectangles.Count == 0)
        {
            _radius = GetRadiusFromSize(size);
            _rectangles.Add(new Rectangle(_center, size));
            return _rectangles[^1];
        }

        var newRectangle = FindFreeSpaceOnSpiral(size);

        _rectangles.Add(newRectangle);
        return _rectangles[^1];
    }

    private Rectangle FindFreeSpaceOnSpiral(Size rectangleSize)
    {
        var angle = 0d;
        var offset = _radius;
        var spiralLength = 0d;
        var spiralStep = GetRadiusFromSize(rectangleSize);

        while (true)
        {
            var center = GetFreeCenter(rectangleSize, offset, spiralLength);
            var rect = new Rectangle(center, rectangleSize);

            if (!_rectangles.Any(r => r.IntersectsWith(rect)))
            {
                rect = MoveTowardsCenter(rect, rectangleSize);
                _angle = angle;
                _radius = offset;
                return rect;
            }

            if (offset > _radius + spiralStep)
                angle += _spiralRotation;
            offset += spiralStep;
            spiralLength += spiralStep;
        }
    }

    private Rectangle MoveTowardsCenter(Rectangle rect, Size rectangleSize)
    {
        var distanceToCenter = Math.Sqrt(Math.Pow(rect.X - _center.X, 2) + Math.Pow(rect.Y - _center.Y, 2));

        var shift = Math.Min(distanceToCenter / 2, rectangleSize.Width / 4);

        return new Rectangle( new Point((int)(rect.X - shift), (int)(rect.Y - shift)), rect.Size);
    }

    private static double GetRadiusFromSize(Size size)
    {
        return Math.Sqrt(Math.Pow(size.Width, 2) + Math.Pow(size.Height, 2)) / 2;
    }

    private Point GetFreeCenter(Size size, double radius, double spiralLength)
    {
        var angleRad = _angle * Math.PI / 180;

        var x = (int)(_center.X + radius * Math.Cos(angleRad) + spiralLength * Math.Sin(angleRad));
        var y = (int)(_center.Y + radius * Math.Sin(angleRad) - spiralLength * Math.Cos(angleRad));

        return new Point(x, y);
    }
}


namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private readonly Point _center;
    private double _radius = 0;
    private double _angle = 0;

    private LinkedList<Rectangle> _rectangles { get; } = new();

    public CircularCloudLayouter(Point center)
    {
        _center = center;
    }

    public Rectangle PutNextRectangle(Size size)
    {
        Rectangle newRectangle;
        if (_rectangles.Count == 0)
        {
            newRectangle = new Rectangle(size, _center);
            _radius = Math.Sqrt(Math.Pow(newRectangle.Size.Width, 2) + Math.Pow(newRectangle.Size.Height, 2)) / 2;
        }
        else
            newRectangle = new Rectangle(size, GetFreeCenter(size));
        
        _rectangles.AddLast(newRectangle);
        return _rectangles.Last?.Value ?? newRectangle;
    }

    private Point GetFreeCenter(Size size)
    {
        var newX = (int)(_center.X + _radius * Math.Cos(_angle));
        var newY = (int)(_center.Y + _radius * Math.Sin(_angle));
        return new Point(newX, newY);
    }
}


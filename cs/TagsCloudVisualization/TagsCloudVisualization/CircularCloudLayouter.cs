namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private readonly Point _center;
    private double _radius = 0;
    private double _angle = 0;
    private double _spiralStep = 10; // Шаг спирали
    private double _spiralRotation = 10; // Угол вращения спирали на каждый шаг

    private LinkedList<Rectangle> _rectangles { get; } = new();

    public CircularCloudLayouter(Point center)
    {
        _center = center;
    }

    public Rectangle PutNextRectangle(Size size)
    {
        if (_rectangles.Count == 0)
        {
            _radius = GetRadiusFromSize(size);
            _rectangles.AddLast(new Rectangle(size, _center));
            return _rectangles.Last.Value;
        }

        var newRectangle = FindFreeSpaceOnSpiral(size);
        
        _rectangles.AddLast(newRectangle);
        return _rectangles.Last?.Value ?? newRectangle;
    }

    private Rectangle FindFreeSpaceOnSpiral(Size rectangleSize)
    {
        double angle = 0;
        double offset = _radius;
        double spiralLength = 0;

        while (true)
        {
            var center = GetFreeCenter(rectangleSize, offset, spiralLength);
            var rect = new Rectangle(rectangleSize, center);

            if (!_rectangles.Any(r => r.IntersectsWith(rect)))
            {
                rect = MoveTowardsCenter(rect, rectangleSize);
                _angle = angle;
                _radius = offset;
                return rect;
            }

            // 5. Поиск нового смещения
            angle += _spiralRotation;
            offset += _spiralStep;
            spiralLength += _spiralStep;
        }
    }

    private Rectangle MoveTowardsCenter(Rectangle rect, Size rectangleSize)
    {
        var distanceToCenter = Math.Sqrt(Math.Pow(rect.Center.X + rect.Size.Width / 2 - _center.X, 2) + Math.Pow(rect.Center.Y + rect.Size.Height / 2 - _center.Y, 2));

        // 2. Расчет сдвига к центру
        var shift = Math.Min(distanceToCenter / 2, rectangleSize.Width / 4);

        // 3. Сдвиг прямоугольника к центру
        return new Rectangle(rect.Size, new Point((rect.Center.X - shift), (rect.Center.Y - shift)));
    }

    private static double GetRadiusFromSize(Size size)
    {
        return Math.Sqrt(Math.Pow(size.Width, 2) + Math.Pow(size.Height, 2)) / 2;
    }
    private Point GetFreeCenter(Size size, double radius, double spiralLength)
    {
        var angleRad = _angle * Math.PI / 180;

        // Вычисление координат точки на спирали
        var x = _center.X + radius * Math.Cos(angleRad) + spiralLength * Math.Sin(angleRad);
        var y = _center.Y + radius * Math.Sin(angleRad) - spiralLength * Math.Cos(angleRad);

        return new Point(x, y);
    }
}


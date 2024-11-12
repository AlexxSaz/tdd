using System.Drawing;

namespace TagsCloudVisualization;
public class PointGenerator(Point centerPoint)
{
    private const double AngleStep = Math.PI / 360;
    private const double RadiusStep = 0.01;
    private double _radius;
    private double _angle;
    private readonly Size _centerSize = new Size(centerPoint);

    public Point GetNewPoint()
    {
        var newX = (int)(_radius * Math.Cos(_angle));
        var newY = (int)(_radius * Math.Sin(_angle));
        var newPoint = new Point(newX, newY);
        newPoint = Point.Add(newPoint, _centerSize);

        _angle += AngleStep;
        _radius += RadiusStep;

        return newPoint;
    }
}

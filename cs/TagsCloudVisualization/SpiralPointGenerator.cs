using System.Drawing;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization;

public class SpiralPointGenerator : IPointGenerator
{
    private const double AngleStep = Math.PI / 12;
    private readonly double _radiusStep;
    private readonly Size _center;
    private double _radius;
    private double _angle;

    public SpiralPointGenerator(Point centerPoint, double radiusStep = 0.01, double startRadius = 0)
    {
        if (radiusStep <= 0)
            throw new ArgumentOutOfRangeException($"{nameof(radiusStep)} should be greater than 0");
        if (startRadius < 0)
            throw new ArgumentOutOfRangeException($"{nameof(startRadius)} should be greater than or equal 0");

        _radius = startRadius;
        _radiusStep = radiusStep;
        _center = new Size(centerPoint);
    }

    public Point GeneratePoint()
    {
        var newX = (int)(_radius * Math.Cos(_angle));
        var newY = (int)(_radius * Math.Sin(_angle));
        var newPoint = new Point(newX, newY).MoveTo(_center);

        _angle += AngleStep;
        _radius += _radiusStep;

        return newPoint;
    }
}

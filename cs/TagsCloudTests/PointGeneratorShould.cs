using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudTests;

[TestFixture]
public class PointGeneratorShould
{
    private IPointGenerator _defaultPointGenerator;
    private Point _defaultCenter;

    [SetUp]
    public void SetUp()
    {
        _defaultCenter = new Point(5, 2);
        _defaultPointGenerator = GetPointGenerator(_defaultCenter);
    }

    public virtual IPointGenerator GetPointGenerator(Point center) =>
        new SpiralPointGenerator(center);

    [Test]
    public void GetNewPoint_ReturnCenter_AfterFirstExecution()
    {
        var point = _defaultPointGenerator.GeneratePoint();

        point.Should().BeEquivalentTo(_defaultCenter);
    }

    [Test]
    public void GetNewPoint_ReturnPointWithGreaterRadius_AfterManyExecutions()
    {
        var prevSpiralRadius = 0d;
        const int radiusCheckPeriod = 200;

        for (var i = 1; i < 10001; i++)
        {
            var currPoint = _defaultPointGenerator.GeneratePoint();
            if (i % radiusCheckPeriod != 0) continue;
            var currSpiralRadius = GetDistanceBetween(_defaultCenter, currPoint);
            currSpiralRadius.Should().BeGreaterThan(prevSpiralRadius);
            prevSpiralRadius = currSpiralRadius;
        }
    }

    private static double GetDistanceBetween(Point point1, Point point2) =>
        Math.Sqrt(Math.Pow((point1.X - point2.X), 2) + Math.Pow((point1.Y - point2.Y), 2));
}
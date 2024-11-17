using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudTests;

[TestFixture]
[Parallelizable(scope: ParallelScope.All)]
public class PointGeneratorShould
{
    private readonly Point _defaultCenter = new(1, 1);
    private readonly Random _random = new();

    public virtual IPointGenerator GetPointGenerator(Point center) =>
        new SpiralPointGenerator(center);

    [Test]
    public void GetNewPoint_ReturnCenter_AfterFirstExecution()
    {
        var pointGenerator = GetPointGenerator(_defaultCenter);
        var newPointIterator = pointGenerator.GeneratePoint().GetEnumerator();
        newPointIterator.MoveNext();
        var point = newPointIterator.Current;

        point.Should().BeEquivalentTo(_defaultCenter);
    }

    [TestCase(0)]
    [TestCase(-1)]
    public void ThrowArgumentOutOfRangeException_AfterExecutionWith(double radiusStep)
    {
        var pointGeneratorCreate = () => new SpiralPointGenerator(_defaultCenter, radiusStep);

        pointGeneratorCreate.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Test]
    [Repeat(5)]
    public void GetNewPoint_ReturnPointWithGreaterRadius_AfterManyExecutions()
    {
        const double radiusStep = 0.02;
        const double radiusCheckPeriod = 1 / radiusStep;
        var newPointGenerator = new SpiralPointGenerator(_defaultCenter, radiusStep);
        var pointIterator = newPointGenerator.GeneratePoint().GetEnumerator();
        var prevSpiralRadius = 0;
        var pointsCount = radiusCheckPeriod * _random.Next(10, 100);

        for (var i = 1; i <= pointsCount; i++)
        {
            pointIterator.MoveNext();
            var currPoint = pointIterator.Current;
            if (i % radiusCheckPeriod != 0) continue;
            var currSpiralRadius = (int)currPoint.GetDistanceTo(_defaultCenter);
            currSpiralRadius.Should().BeGreaterThanOrEqualTo(prevSpiralRadius);
            prevSpiralRadius = currSpiralRadius;
        }
    }
}
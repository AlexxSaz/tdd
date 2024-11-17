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
    private IPointGenerator _defaultPointGenerator;

    [SetUp]
    public void SetUp()
    {
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

    [TestCase(0, 1)]
    [TestCase(-1, 1)]
    [TestCase(1, -1)]
    public void ThrowArgumentOutOfRangeException_AfterExecutionWith(double radiusStep, double startRadius)
    {
        var pointGeneratorCreate = () => new SpiralPointGenerator(_defaultCenter, radiusStep, startRadius);

        pointGeneratorCreate.Should().Throw<ArgumentOutOfRangeException>();
    }


    [Test]
    [Repeat(5)]
    public void GetNewPoint_ReturnPointWithGreaterRadius_AfterManyExecutions()
    {
        const double radiusStep = 0.02;
        const double startRadius = 0;
        const double radiusCheckPeriod = 1 / radiusStep;
        var newPointGenerator = new SpiralPointGenerator(_defaultCenter, radiusStep);
        var prevSpiralRadius = (int)startRadius;
        var pointsCount = radiusCheckPeriod * _random.Next(10, 100);

        for (var i = 1; i <= pointsCount; i++)
        {
            var currPoint = newPointGenerator.GeneratePoint();
            if (i % radiusCheckPeriod != 0) continue;
            var currSpiralRadius = (int)currPoint.GetDistanceTo(_defaultCenter);
            currSpiralRadius.Should().BeGreaterThanOrEqualTo(prevSpiralRadius);
            prevSpiralRadius = currSpiralRadius;
        }
    }
}
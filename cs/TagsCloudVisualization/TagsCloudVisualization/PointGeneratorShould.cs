using NUnit.Framework;
using FluentAssertions;
using System.Drawing;

namespace TagsCloudVisualization;

[TestFixture]
public class PointGeneratorShould
{
    private PointGenerator _defaultPointGenerator;
    private Point _defaultCenter;

    [SetUp]
    public void SetUp()
    {
        _defaultCenter = new Point(5, 2);
        _defaultPointGenerator = new PointGenerator(_defaultCenter);
    }

    [Test]
    public void GetNewPoint_ReturnCenter_AfterFirstExecution()
    {
        var point = _defaultPointGenerator.GetNewPoint();

        point.Should().BeEquivalentTo(_defaultCenter);
    }

    [TestCase(1000)]
    [TestCase(0)]
    [TestCase(10000)]
    public void GetNewPoint_ReturnUniqPointEvery100Step_AfterExecution(int stepCount)
    {
        const int stepToCheck = 100;
        var pointsHashset = new HashSet<Point>();

        for (var i = 1; i <= stepCount; i++)
        {
            var currPoint = _defaultPointGenerator.GetNewPoint();
            if (i % stepToCheck == 0)
                pointsHashset.Add(currPoint);
        }

        pointsHashset.Should().HaveCount(stepCount / stepToCheck);
    }
}
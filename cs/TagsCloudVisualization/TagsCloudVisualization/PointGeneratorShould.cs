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

    [Test]
    public void ReturnUniqPointEveryTime_AfterExecution()
    {
        const int count = 101;
        const int stepForCheck = 10;
        var pointsHashset = new HashSet<Point>();

        for (var i = 0; i < count; i++)
        {
            var currPoint = _defaultPointGenerator.GetNewPoint();
            if (i % stepForCheck == 0)
                pointsHashset.Add(currPoint);
        }

        pointsHashset.Should().HaveCount(count / stepForCheck);
    }
}
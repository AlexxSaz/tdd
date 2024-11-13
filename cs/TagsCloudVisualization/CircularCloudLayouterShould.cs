using FluentAssertions;
using NUnit.Framework;
using System.Drawing;

namespace TagsCloudVisualization.Tests;

[TestFixture]
public class CircularCloudLayouterShould
{
    private Point _defaultCenter;
    private ICircularCloudLayouter _defaultCircularCloudLayouter;

    [SetUp]
    public void SetUp()
    {
        _defaultCenter = new Point(0, 0);
        _defaultCircularCloudLayouter = GetCircularCloudLayouter(_defaultCenter);
    }

    public virtual ICircularCloudLayouter GetCircularCloudLayouter(Point center) =>
        new CircularCloudLayouter(center);

    [TestCase(1, 1, 4, 2)]
    public void PutNextRectangle_ReturnRectangleWithBaseCenter_AfterFirstExecutionWith(int x, int y, int width, int height)
    {
        var point = new Point(x, y);
        var size = new Size(width, height);
        var circularCloudLayouter = new CircularCloudLayouter(point);
        var expectedRectangle = new Rectangle(point, size);

        circularCloudLayouter.PutNextRectangle(size).Should().BeEquivalentTo(expectedRectangle);
    }

    [TestCase(-1, 1)]
    [TestCase(1, -1)]
    [TestCase(0, 0)]
    public void PutNextRectangle_ThrowArgumentOutOfRangeException_AfterExecutionWith(int width, int height)
    {
        var size = new Size(width, height);
        var action = () => _defaultCircularCloudLayouter.PutNextRectangle(size);

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Test]
    public void PutNextRectangle_ReturnRectangleThatNotCrossingWithOther_AfterFewExecution()
    {
        var rnd = new Random();
        var rectangleList = new List<Rectangle>();
        var width = rnd.Next(5, 20);
        var height = rnd.Next(2, 5);
        var size = new Size(width, height);
        for (var i = 0; i < 10; i++)
        {
            var currRectangle =
                _defaultCircularCloudLayouter.PutNextRectangle(size);
            rectangleList.Add(currRectangle);
        }
        var lastRectangle = rectangleList[^1];

        for (var i = 0; i < rectangleList.Count - 1; i++)
        {
            var currRectangle = rectangleList[i];
            lastRectangle.IntersectsWith(currRectangle).Should().BeFalse();
        }
    }
}


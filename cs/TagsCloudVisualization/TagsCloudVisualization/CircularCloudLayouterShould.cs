using FluentAssertions;
using NUnit.Framework;
using System.Drawing;

namespace TagsCloudVisualization;

[TestFixture]
public class CircularCloudLayouterShould
{
    private Point _defaultCenter;
    private CircularCloudLayouter _defaultCircularCloudLayouter;

    [SetUp]
    public void SetUp()
    {
        _defaultCenter = new Point(0, 0);
        _defaultCircularCloudLayouter = new CircularCloudLayouter(_defaultCenter);
    }

    [TestCase(1, 1)]
    public void PutNextRectangle_ReturnRectangleWithBaseCenter_AfterFirstExecutionWith(int x, int y)
    {
        var point = new Point(x, y);
        var size = new Size(5, 2);
        var circularCloudLayouter = new CircularCloudLayouter(point);
        var expectedCenter = new Point(x, y);

        circularCloudLayouter.PutNextRectangle(size).Should().BeEquivalentTo(expectedCenter);
    }

    [Test]
    public void PutNextRectangle_ReturnRectangleThatNotCrossingWithOther_AfterFewExecution()
    {
        var rnd = new Random();
        var rectangleList = new List<Rectangle>();
        for (var i = 0; i < 10; i++)
        {
            var currRectangle =
                _defaultCircularCloudLayouter.PutNextRectangle(new Size(rnd.Next(5, 20), rnd.Next(2, 5)));
            rectangleList.Add(currRectangle);
        }

        var lastRectangle = _defaultCircularCloudLayouter.PutNextRectangle(new Size(rnd.Next(5, 20), rnd.Next(2, 5)));

        foreach (var rectangle in rectangleList.Where(rectangle => rectangle != lastRectangle))
        {
            lastRectangle.IntersectsWith(rectangle).Should().BeFalse();
        }
    }
}


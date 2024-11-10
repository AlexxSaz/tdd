using FluentAssertions;
using NUnit.Framework;

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

    [Test]
    public void HaveZeroRectangles_AfterCreation()
    {
        _defaultCircularCloudLayouter.Rectangles.Should().HaveCount(0);
    }

    [TestCase(1, 1)]
    public void PutNextRectangle_ReturnAddedRectangle_AfterExecutionWith(int width, int height)
    {
        var size = new Size(width, height);
        var expectedRectangle = new Rectangle(size, _defaultCenter);

        _defaultCircularCloudLayouter.PutNextRectangle(size).Should().BeEquivalentTo(expectedRectangle);
    }

    [TestCase(1, 1)]
    public void PutNextRectangle_ReturnRectangleWithBaseCenter_AfterFirstExecutionWith(int x, int y)
    {
        var point = new Point(x, y);
        var size = new Size(5, 2);
        var circularCloudLayouter = new CircularCloudLayouter(point);
        var expectedCenter = new Point(x, y);

        circularCloudLayouter.PutNextRectangle(size).Center.Should().BeEquivalentTo(expectedCenter);
    }

    [Test]
    public void PutNextRectangle_ReturnRectangleThatNotCrossingWithOther_AfterFewExecution()
    {
        var rnd = new Random();
        _defaultCircularCloudLayouter.PutNextRectangle(new Size(rnd.Next(5, 20), rnd.Next(2, 5)));
        _defaultCircularCloudLayouter.PutNextRectangle(new Size(rnd.Next(5, 20), rnd.Next(2, 5)));
        _defaultCircularCloudLayouter.PutNextRectangle(new Size(rnd.Next(5, 20), rnd.Next(2, 5)));

        var lastRectangle = _defaultCircularCloudLayouter.PutNextRectangle(new Size(rnd.Next(5, 20), rnd.Next(2, 5)));

        foreach (var rectangle in _defaultCircularCloudLayouter.Rectangles.Where(rectangle => rectangle != lastRectangle))
        {
            lastRectangle.Center.Should().NotBeEquivalentTo(rectangle.Center);
        }
    }
}


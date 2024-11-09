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
        var expectedRectangle = new Rectangle(size);

        _defaultCircularCloudLayouter.PutNextRectangle(size).Should().BeEquivalentTo(expectedRectangle);
    }
}


using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization;

namespace TagsCloudTests;

[TestFixture]
public class TagCloudShould
{
    [TestCase(0, 0)]
    public void PlacedInSelectedCenter_AfterCreation(int x, int y)
    {
        var center = new Point(x, y);
        var firstRectangleSize = new Size(2, 2);
        var circularCloudLayouter = new CircularCloudLayouter(center);
        var tagCloud = new TagCloud(circularCloudLayouter);

        tagCloud.PutNextRectangle(firstRectangleSize);

        tagCloud.Rectangles[0].X.Should().Be(x);
        tagCloud.Rectangles[0].Y.Should().Be(y);
    }

    [Test]
    public void HaveNoRectangles_AfterCreation()
    {
        var center = new Point(0, 0);
        var layouter = new CircularCloudLayouter(center);
        var tagCloud = new TagCloud(layouter);

        tagCloud.Rectangles.Should().HaveCount(0);
    }

    [TestCase(5, ExpectedResult = 5)]
    public int HaveExpectedWidth_AfterAddedFirstRectangleWith(int width)
    {
        var newSize = new Size(width, 2);
        var center = new Point(0, 0);
        var layouter = new CircularCloudLayouter(center);
        var tagCloud = new TagCloud(layouter);

        tagCloud.PutNextRectangle(newSize);

        return tagCloud.Width;
    }

    [TestCase(5, ExpectedResult = 5)]
    public int HaveExpectedHeight_AfterAddedFirstRectangleWith(int height)
    {
        var newSize = new Size(2, height);
        var center = new Point(0, 0);
        var layouter = new CircularCloudLayouter(center);
        var tagCloud = new TagCloud(layouter);

        tagCloud.PutNextRectangle(newSize);

        return tagCloud.Height;
    }

    [Test]
    public void HaveCircularForm_WhenLayoutIsCircular()
    {

    }
}
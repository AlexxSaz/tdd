using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudTests;

[TestFixture]
[Parallelizable(scope: ParallelScope.All)]
public class TagCloudShould
{
    private Random _random;
    private Point _defaultCenter;

    public TagCloudShould()
    {
        _random = new Random();
        _defaultCenter = new Point(1, 1);
    }

    [Test]
    [Repeat(5)]
    public void PlacedInSelectedCenter_AfterCreation()
    {
        var x = _random.Next(0, 10);
        var y = _random.Next(0, 10);
        var center = new Point(x, y);
        var firstRectangleSize = new Size(2, 2);
        var circularCloudLayouter = new CircularCloudLayouter(center);
        var tagCloud = new TagCloud(circularCloudLayouter);

        tagCloud.AddNextRectangleWith(firstRectangleSize);

        tagCloud.Rectangles[0].GetCentralPoint().Should().Be(center);
    }

    [Test]
    public void HaveZeroSize_AfterCreation()
    {
        var circularCloudLayouter = new CircularCloudLayouter(_defaultCenter);
        var currTagCloud = new TagCloud(circularCloudLayouter);

        currTagCloud.Width.Should().Be(0);
        currTagCloud.Height.Should().Be(0);
    }

    [Test]
    [Repeat(5)]
    public void HaveExpectedWidth_AfterAddedFirstRectangleWith()
    {
        var width = _random.Next(1, 100);
        var newSize = new Size(width, 2);
        var circularCloudLayouter = new CircularCloudLayouter(_defaultCenter);
        var currTagCloud = new TagCloud(circularCloudLayouter);

        currTagCloud.AddNextRectangleWith(newSize);

        currTagCloud.Width.Should().Be(width);
    }

    [Test]
    [Repeat(5)]
    public void HaveExpectedHeight_AfterAddedFirstRectangleWith()
    {
        var height = _random.Next(1, 100);
        var newSize = new Size(2, height);
        var circularCloudLayouter = new CircularCloudLayouter(_defaultCenter);
        var currTagCloud = new TagCloud(circularCloudLayouter);

        currTagCloud.AddNextRectangleWith(newSize);

        currTagCloud.Height.Should().Be(height);
    }

    [Test]
    [Repeat(5)]
    public void AddNextRectangleWith_AddedExpectedNumberOfRectangles_AfterManyExecutions()
    {
        var expectedCount = _random.Next(0, 10);
        var unionSize = new Size(2, 2);
        var circularCloudLayouter = new CircularCloudLayouter(_defaultCenter);
        var currTagCloud = new TagCloud(circularCloudLayouter);

        for (var i = 0; i < expectedCount; i++)
            currTagCloud.AddNextRectangleWith(unionSize);

        currTagCloud.Rectangles.Should().HaveCount(expectedCount);
    }
}
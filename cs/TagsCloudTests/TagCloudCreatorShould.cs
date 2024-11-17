using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization;

namespace TagsCloudTests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class TagCloudCreatorShould
{
    private readonly Point _defaultCenter = new(0, 0);
    private readonly Random _random = new();

    [Test]
    [Repeat(5)]
    public void Create_ReturnTagCloudEquivalentToConstructor_AfterExecution()
    {
        var rectangleSizes = Enumerable.Range(_random.Next(2, 10), _random.Next(20, 100)).
            Select(width => new Size(width, width / 2)).Reverse();
        var currCloudLayouter = new CircularCloudLayouter(_defaultCenter);
        var expectedCloudLayouter = new CircularCloudLayouter(_defaultCenter);
        var defaultTagCloud = new TagCloud(currCloudLayouter);
        foreach (var rectangleSize in rectangleSizes)
            defaultTagCloud.AddNextRectangleWith(rectangleSize);

        var expectedTagCloud = TagCloudCreator.Create(rectangleSizes, expectedCloudLayouter);

        defaultTagCloud.Should().BeEquivalentTo(expectedTagCloud);
    }
}
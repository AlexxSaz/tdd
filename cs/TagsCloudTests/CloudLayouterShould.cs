using FluentAssertions;
using NUnit.Framework.Interfaces;
using System.Drawing;
using TagsCloudVisualization;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudTests;

[TestFixture]
[Parallelizable(scope: ParallelScope.All)]
public class CloudLayouterShould
{
    private readonly Point _defaultCenter = new(0, 0);
    private readonly Random _random = new();

    public virtual ICloudLayouter GetCloudLayouter(Point center) =>
        new CircularCloudLayouter(center);

    [Test]
    [Repeat(5)]
    public void PutNextRectangle_ReturnRectangleWithExpectedLocation_AfterFirstExecution()
    {
        var expectedCenter = new Point(_random.Next(-10, 10), _random.Next(-10, 10));
        var rectangleWidth = _random.Next(5, 100);
        var rectangleSize = new Size(rectangleWidth, rectangleWidth / 2);
        var cloudLayouter = GetCloudLayouter(expectedCenter);

        var actualRectangle = cloudLayouter.PutNextRectangle(rectangleSize);

        actualRectangle.GetCentralPoint().Should().BeEquivalentTo(expectedCenter);
    }

    [TestCase(-1, 1)]
    [TestCase(1, -1)]
    [TestCase(0, 0)]
    public void PutNextRectangle_ThrowArgumentOutOfRangeException_AfterExecutionWith(int width, int height)
    {
        var rectangleSize = new Size(width, height);
        var circularCloudLayouter = new CircularCloudLayouter(_defaultCenter);

        var executePutNewRectangle = () => circularCloudLayouter.PutNextRectangle(rectangleSize);

        executePutNewRectangle.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Test]
    [Repeat(5)]
    public void PutNextRectangle_ReturnRectangleThatNotIntersectsWithOther_AfterManyExecution()
    {
        var seenRectangles = new HashSet<Rectangle>();
        var rectangleSizes = GetSizes(_random.Next(10, 100), _random.Next(100, 200));
        var cloudLayouter = GetCloudLayouter(_defaultCenter);

        var rectangleList = rectangleSizes
            .Select(size => cloudLayouter.PutNextRectangle(size));

        foreach (var rectangle in rectangleList)
        {
            seenRectangles.Add(rectangle);
            rectangleList
                .Where(rect => !seenRectangles.Contains(rect))
                .All(rect => rect.IntersectsWith(rectangle))
                .Should().BeFalse();
        }
    }

    [Test]
    [Repeat(5)]
    public void PutNextRectangle_ReturnLastRectanglesWithCloseRadius_AfterManyExecution()
    {
        var largestSide = _random.Next(100, 200);
        var rectangleSizes = GetSizes(_random.Next(5, 10), largestSide);
        var radii = new List<double>();
        var circularCloudLayouter = new CircularCloudLayouter(_defaultCenter);
        var lastIndex = (int)(largestSide * 0.9);
        var expectedDifference = (int)(largestSide * 0.05);

        foreach (var rectangleSize in rectangleSizes)
        {
            var currSquare = circularCloudLayouter.PutNextRectangle(rectangleSize);
            var squareCenter = currSquare.GetCentralPoint();
            radii.Add(Math.Round(squareCenter.GetDistanceTo(_defaultCenter)));
        }

        for (var i = lastIndex; i < largestSide; i++)
        {
            (radii[i] - radii[i - 1]).Should().BeLessOrEqualTo(expectedDifference);
        }
    }

    [Test]
    [Repeat(5)]
    public void PutNextRectangle_ReturnRectangleWithMaximumDensity_AfterManyExecution()
    {
        var rectangleWidth = _random.Next(5, 1000);
        var rectangleSize = new Size(rectangleWidth, rectangleWidth / 2);
        var diagonal = Math.Sqrt(Math.Pow(rectangleSize.Height, 2) + Math.Pow(rectangleSize.Width, 2));
        var circularCloudLayouter = new CircularCloudLayouter(_defaultCenter);
        var radii = new List<double>();
        var rectangleCount = _random.Next(10, 200);

        for (var i = 0; i < rectangleCount; i++)
        {
            var currSquare = circularCloudLayouter.PutNextRectangle(rectangleSize);
            var squareCenter = currSquare.GetCentralPoint();
            radii.Add(Math.Round(squareCenter.GetDistanceTo(_defaultCenter)));
        }

        var radiusDifferences = radii
            .Skip(1)
            .Zip(radii, (current, previous) => current - previous);
        foreach (var difference in radiusDifferences)
            difference.Should().BeLessOrEqualTo(diagonal);
    }

    private static IEnumerable<Size> GetSizes(int lowest, int largest) =>
        Enumerable.Range(lowest, largest)
            .Select(number => new Size(number, number / 2))
            .Reverse();
}
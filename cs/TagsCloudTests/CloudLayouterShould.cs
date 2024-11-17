using FluentAssertions;
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
        var rectangleSizes = GenerateInfiniteSizes().Take(_random.Next(10, 200));
        var cloudLayouter = GetCloudLayouter(_defaultCenter);

        var rectangles = rectangleSizes
            .Select(size => cloudLayouter.PutNextRectangle(size)).ToArray();

        for (var i = 0; i < rectangles.Length; i++)
            for (var j = i + 1; j < rectangles.Length; j++)
                rectangles[i].IntersectsWith(rectangles[j]).Should().BeFalse();
    }

    [Test]
    [Repeat(20)]
    public void PutNextRectangle_ReturnRectanglesInCircle_AfterManyExecution()
    {
        var rectangleSizes = GenerateInfiniteSizes().Take(_random.Next(10, 100));
        var circularCloudLayouter = new CircularCloudLayouter(_defaultCenter);

        var rectanglesList = rectangleSizes.Select(rectangleSize => circularCloudLayouter.PutNextRectangle(rectangleSize)).ToList();

        var pointOnCircle = rectanglesList[^1].GetCentralPoint();
        var circleRadius = pointOnCircle.GetDistanceTo(_defaultCenter);
        var fromRectangleToCenterDistances =
            rectanglesList.Select(rectangle => rectangle.GetCentralPoint().GetDistanceTo(_defaultCenter));

        var sumRectanglesSquare = rectanglesList.Sum(rectangle => rectangle.Width * rectangle.Height);
        var circleSquare = circleRadius * circleRadius * Math.PI;
        var precision = circleSquare * 0.475;

        circleSquare.Should().BeApproximately(sumRectanglesSquare, precision);
        foreach (var distanceToCenter in fromRectangleToCenterDistances)
            distanceToCenter.Should().BeLessOrEqualTo(circleRadius + 2);
    }

    private static IEnumerable<Size> GenerateInfiniteSizes()
    {
        var random = new Random();
        while (true)
        {
            var rectangleWidth = random.Next(10, 100);
            var rectangleHeight = random.Next(1, 25);
            yield return new Size(rectangleWidth, rectangleHeight);
        }
    }
}
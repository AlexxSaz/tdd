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
    private Point _defaultCenter;
    private Random _random;

    public CloudLayouterShould()
    {
        _random = new Random();
        _defaultCenter = new Point(0, 0);
    }

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
    public void PutNextRectangle_ReturnFirstFourRectangleWithEqualRadius_AfterExecutionWithSquares() //TODO: Сделать тест на окружность
    {
        var squareSide = _random.Next(5, 50);
        var squareSize = new Size(squareSide, squareSide);
        var radii = new List<double>();
        var circularCloudLayouter = new CircularCloudLayouter(_defaultCenter);

        for (var i = 0; i < 5; i++)
        {
            var currSquare = circularCloudLayouter.PutNextRectangle(squareSize);
            var squareCenter = currSquare.GetCentralPoint();
            radii.Add(Math.Round(squareCenter.GetDistanceTo(_defaultCenter)));
        }

        for (var i = 2; i < 5; i++)
        {
            radii[i - 1].Should().Be(radii[i]);
        }
    }

    [Test]
    [Repeat(5)]
    public void PutNextRectangle_ReturnRectangleWithMaximumDensity_AfterManyExecution()
    {
        var rectangleWidth = _random.Next(5, 1000);
        var rectangleSize = new Size(rectangleWidth, rectangleWidth / 2);
        var halfOfDiagonal = Math.Sqrt(Math.Pow(rectangleSize.Height, 2) + Math.Pow(rectangleSize.Width, 2)) / 2;
        var circularCloudLayouter = new CircularCloudLayouter(_defaultCenter);
        var radii = new List<double>();
        var rectangleCount = _random.Next(10, 200);

        for (var i = 0; i < rectangleCount; i++)
        {
            var currSquare = circularCloudLayouter.PutNextRectangle(rectangleSize);
            var squareCenter = currSquare.GetCentralPoint();
            radii.Add(squareCenter.GetDistanceTo(_defaultCenter));
        }

        var radiusDifferences = radii
            .Skip(1)
            .Zip(radii, (current, previous) => current - previous);
        foreach (var difference in radiusDifferences)
            difference.Should().BeLessOrEqualTo(halfOfDiagonal);
    }

    private static IEnumerable<Size> GetSizes(int lowest, int largest) =>
        Enumerable.Range(lowest, largest)
            .Select(number => new Size(number, number / 2))
            .Reverse();
}
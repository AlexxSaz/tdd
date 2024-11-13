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
        var center = new Point(x, y);
        var size = new Size(width, height);
        var circularCloudLayouter = GetCircularCloudLayouter(center);
        var expectedRectangle = new Rectangle(center, size);

        var actualRectangle = circularCloudLayouter.PutNextRectangle(size);

        actualRectangle.Should().BeEquivalentTo(expectedRectangle);
    }

    [TestCase(-1, 1)]
    [TestCase(1, -1)]
    [TestCase(0, 0)]
    public void PutNextRectangle_ThrowArgumentOutOfRangeException_AfterExecutionWith(int width, int height)
    {
        var rectangleSize = new Size(width, height);

        var executePutNewRectangle = () => _defaultCircularCloudLayouter.PutNextRectangle(rectangleSize);

        executePutNewRectangle.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestCase(10,100)]
    [TestCase(2,40)]
    public void PutNextRectangle_ReturnRectangleThatNotIntersectsWithOther_AfterManyExecution(int lowestWidth, int largestWidth)
    {
        var rectangleSizes = GetSizes(lowestWidth, largestWidth);

        var rectangleList = rectangleSizes
            .Select(size => _defaultCircularCloudLayouter.PutNextRectangle(size))
            .ToList();

        foreach (var rectangle in rectangleList)
        {
            rectangleList
                .Where(rect => rect != rectangle)
                .All(rect => rect.IntersectsWith(rectangle))
                .Should().BeFalse();
        }
    }

    [TestCase(10, 20, ExpectedResult = 1.0)]
    [TestCase(5, 20, ExpectedResult = 2.0)]
    public double PutNextRectangle_ReturnRectangleThatCloseToCircle_AfterManyExecution(int lowestWidth, int largestWidth)
    {
        int minX = 0, minY = 0, maxX = 0, maxY = 0;
        var rectangleSizes = GetSizes(lowestWidth, largestWidth);

        var rectangleList = rectangleSizes
            .Select(size => _defaultCircularCloudLayouter.PutNextRectangle(size))
            .ToList();

        foreach (var rectangle in rectangleList)
        {
            maxX = Math.Max(maxX, rectangle.Right);
            maxY = Math.Max(maxY, rectangle.Bottom);
            minX = Math.Max(minX, rectangle.Left);
            minY = Math.Max(minY, rectangle.Top);
        }

        var width = maxX - minX;
        var height = maxY - minY;

        return width / height;
    }

    [TestCase(10, 20)]
    [TestCase(5, 20)]
    public void PutNextRectangle_ReturnRectanglesWithMaxDensity_AfterManyExecutions(int lowest, int largest)
    {
        var rectangleSizes = GetSizes(lowest, largest);

        var rectangleList = rectangleSizes
            .Select(size => _defaultCircularCloudLayouter.PutNextRectangle(size))
            .ToList();

        var fullSquare = rectangleList.Sum(rect => rect.Width * rect.Height);
        var radius = 0.0;
        foreach (var rect in rectangleList)
        {
            var corners = new List<Point>
            {
                new(rect.Left, rect.Top),
                new(rect.Right, rect.Top),
                new(rect.Left, rect.Bottom),
                new(rect.Right, rect.Bottom)
            };

            radius = corners
                .Select(corner => 
                    Math.Sqrt(Math.Pow(_defaultCenter.X - corner.X, 2) + Math.Pow(_defaultCenter.Y - corner.Y, 2)))
                .Prepend(radius)
                .Max();
        }

        var circleSquare = Math.PI * radius * radius;
        var density = fullSquare / circleSquare;
        density.Should().BeGreaterThan(0.4);
    }

    private static IEnumerable<Size> GetSizes(int lowest, int largest) =>
        Enumerable.Range(lowest, largest)
            .Select(number => new Size(number, number / 2))
            .Reverse();
}


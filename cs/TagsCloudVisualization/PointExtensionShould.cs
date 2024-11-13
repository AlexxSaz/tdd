using System.Drawing;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization;

public class PointExtensionShould
{
    private Point _defaultPoint;

    [SetUp]
    public void SetUp()
    {
        _defaultPoint = new Point(0, 0);
    }

    [TestCase(5, 5)]
    public void MoveTo_ReturnMovedPoint_AfterExecutionWith(int width, int height)
    {
        var direction = new Size(width, height);

        var newPoint = _defaultPoint.MoveTo(direction);

        newPoint.Should().NotBe(_defaultPoint);
    }
}


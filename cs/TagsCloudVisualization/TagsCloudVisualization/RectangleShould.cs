using NUnit.Framework;

namespace TagsCloudVisualization;

[TestFixture]
public class RectangleShould
{
    private Size _defaultSize;
    private Point _defaultPoint;
    private Rectangle _defaultRectangle;

    [SetUp]
    public void SetUp()
    {
        _defaultSize = new Size(4, 2);
        _defaultPoint = new Point(0, 0);
        _defaultRectangle = new Rectangle(_defaultSize, _defaultPoint);
    }

    [TestCase(2, 2, 4, 0, ExpectedResult = false)]
    [TestCase(4, 2, 4, 0, ExpectedResult = false)]
    [TestCase(2, 2, 2, 0, ExpectedResult = true)]
    [TestCase(2, 2, 0, 2, ExpectedResult = false)]
    [TestCase(2, 2, 3, 2, ExpectedResult = false)]
    [TestCase(2, 2, 0, 0, ExpectedResult = true)]
    public bool IntersectsWith_Return_AfterExecutingWith(int width, int height, int x, int y)
    {
        var newSize = new Size(width, height);
        var newPoint = new Point(x, y);
        var newRectangle = new Rectangle(newSize, newPoint);

        return newRectangle.IntersectsWith(_defaultRectangle);
    }
}


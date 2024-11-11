using NUnit.Framework;
using FluentAssertions;
using System.Drawing;

namespace TagsCloudVisualization;

[TestFixture]
public class PointsGeneratorShould
{
    private PointsGenerator _pointsGenerator;

    [SetUp]
    public void SetUp()
    {
        _pointsGenerator = new PointsGenerator();
    }

    [TestCase()]
    public void GetBestPoint_ReturnPoint_AfterExecutingWith()
    {
        _pointsGenerator.GetBestPoint().Should().NotBeNull();
    }
}
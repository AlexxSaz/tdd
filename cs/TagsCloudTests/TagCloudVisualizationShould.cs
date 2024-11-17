using FluentAssertions;
using System.Drawing;
using TagsCloudVisualization;

namespace TagsCloudTests;

[TestFixture]
public class TagCloudVisualizationShould
{
    private readonly Random _random = new();

    [Test]
    public void SaveTagCloudAsBitmap_SaveFile_AfterExecution()
    {
        const string tempBmpFile = "temp.bmp";
        var cloudLayouter = new CircularCloudLayouter(new Point(-5, 5));
        var sizes = Enumerable.Range(_random.Next(2, 10), _random.Next(100, 500)).
            Select(width => new Size(width, width / 2)).Reverse();
        var tagCloud = TagCloudCreator.Create(sizes, cloudLayouter);
        File.Delete(tempBmpFile);
        File.Exists(tempBmpFile).Should().BeFalse($"file {tempBmpFile} should be deleted");
        
        TagCloudVisualization.SaveTagCloudAsBitmap(tagCloud, tempBmpFile);

        File.Exists(tempBmpFile).Should().BeTrue($"file {tempBmpFile} should be generated");
    }
}
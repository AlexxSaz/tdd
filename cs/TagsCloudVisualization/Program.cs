using System.Drawing;

namespace TagsCloudVisualization;

public static class Program
{
    public static void Main(string[] args)
    {
        const string tempBmpFile = "temp.bmp";
        var cloudLayouter = new CircularCloudLayouter(new Point(-5, 5));
        var sizes = GenerateInfiniteSizes().Take(500);
        var tagCloud = TagCloudCreator.Create(sizes, cloudLayouter);

        TagCloudVisualization.SaveTagCloudAsBitmap(tagCloud, tempBmpFile);
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
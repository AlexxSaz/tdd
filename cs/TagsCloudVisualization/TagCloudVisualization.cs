using System.Drawing;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization;

public class TagCloudVisualization
{
    private static readonly Random _random = new();

    public static void SaveTagCloudAsBitmap(TagCloud tagCloud, string file)
    {
        const int rectangleOutline = 1;

        var bitmap = new Bitmap(
            tagCloud.Width + rectangleOutline,
            tagCloud.Height + rectangleOutline);

        var frameShift = new Size(-tagCloud.LeftBound, -tagCloud.TopBound);

        using (var graphics = Graphics.FromImage(bitmap))
        {
            foreach (var rectangle in tagCloud.Rectangles)
            {
                var rectangleInFrame = MoveRectangleToImageFrame(rectangle, frameShift);
                graphics.DrawRectangle(new Pen(GetRandomBrush()), rectangleInFrame);
            }
        }
        bitmap.Save(file);
    }

    private static Rectangle MoveRectangleToImageFrame(Rectangle rectangle, Size imageCenter) =>
        new(rectangle.Location.MoveTo(imageCenter), rectangle.Size);

    private static Brush GetRandomBrush() =>
        new SolidBrush(GetRandomColor());

    private static Color GetRandomColor()
    {
        var knownColors = (KnownColor[])Enum.GetValues(typeof(KnownColor));
        var randomColorName = knownColors[_random.Next(knownColors.Length)];
        return Color.FromKnownColor(randomColorName);
    }
}
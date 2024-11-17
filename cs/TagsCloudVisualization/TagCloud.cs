using System.Drawing;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization;

public class TagCloud(ICloudLayouter? layouter)
{
    private readonly ICloudLayouter _layouter = layouter ?? new CircularCloudLayouter(new Point(0, 0));
    public List<Rectangle> Rectangles { get; } = [];

    public void AddNextRectangleWith(Size size)
    {
        var nextRectangle = _layouter.PutNextRectangle(size);
        Rectangles.Add(nextRectangle);
    }

    public int Width
    {
        get
        {
            if (Rectangles.Count == 0)
                return 0;

            return Rectangles.Max(rect => rect.Right) -
                   Rectangles.Min(rect => rect.Left);
        }
    }

    public int Height
    {
        get
        {
            if (Rectangles.Count == 0)
                return 0;

            return Rectangles.Max(rect => rect.Bottom) -
                   Rectangles.Min(rect => rect.Top);
        }
    }

    public int LeftBound =>
        Rectangles.Min(r => r.Left);

    public int TopBound =>
        Rectangles.Min(r => r.Top);
}
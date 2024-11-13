using System.Drawing;

namespace TagsCloudVisualization;

public static class PointExtension
{
    public static Point MoveTo(this Point point, Size direction) =>
        Point.Add(point, direction);
}


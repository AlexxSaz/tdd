using System.Drawing;

namespace TagsCloudVisualization;
public class PointsGenerator
{
    public PointsGenerator(Point centerPoint)
    {
        var size = new Size(centerPoint);
    }

    public Point GetBestPoint()
    {
        return new Point(0, 0);
    }
}

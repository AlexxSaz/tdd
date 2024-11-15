using System.Drawing;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization;

public class CircularCloudLayouter(Point center) : ICloudLayouter
{
    private readonly IPointGenerator _pointGenerator = new SpiralPointGenerator(center);
    private readonly List<Rectangle> _rectangles = [];

    public Rectangle PutNextRectangle(Size size)
    {
        if (size.Width < 1 || size.Height < 1)
            throw new ArgumentOutOfRangeException(
                $"{nameof(size.Width)} and {nameof(size.Height)} should be greater than zero");

        Rectangle newRectangle;
        do newRectangle = new Rectangle(_pointGenerator.GeneratePoint(), size);
        while (_rectangles.Any(rec => newRectangle.IntersectsWith(rec)));

        _rectangles.Add(newRectangle);
        return newRectangle;
    }
}
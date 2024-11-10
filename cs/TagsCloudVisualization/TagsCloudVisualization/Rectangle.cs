namespace TagsCloudVisualization
{
    public class Rectangle(Size size, Point center)
    {
        public Size Size { get; } = size;
        public Point Center { get; } = center;

        public bool IntersectsWith(Rectangle? other)
        {
            if (other == null)
                return false;

            var topRight = new Point((Center.X + Size.Width / 2), (Center.Y + Size.Height / 2));
            var bottomLeft = new Point((Center.X - Size.Width / 2), (Center.Y - Size.Height / 2));

            var otherTopRight = new Point((other.Center.X + other.Size.Width / 2), (other.Center.Y + other.Size.Height / 2));
            var otherBottomLeft = new Point((other.Center.X - other.Size.Width / 2), (other.Center.Y - other.Size.Height / 2));

            var bottomLeftIntersectX = Math.Max(bottomLeft.X, otherBottomLeft.X);
            var bottomLeftIntersectY = Math.Max(bottomLeft.Y, otherBottomLeft.Y);

            var topRightIntersectX = Math.Min(topRight.X, otherTopRight.X);
            var topRightIntersectY = Math.Min(topRight.Y, otherTopRight.Y);

            return bottomLeftIntersectX < topRightIntersectX && bottomLeftIntersectY < topRightIntersectY;
        }
    }
}

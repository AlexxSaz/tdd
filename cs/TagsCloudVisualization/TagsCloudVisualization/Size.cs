namespace TagsCloudVisualization;

public class Size
{
    private int _width;
    private int _height;

    public int Width
    {
        get => _width;
        private set
        {
            if (value is < 1 or > 100)
                throw new ArgumentOutOfRangeException($"{nameof(Width)} can't be less than 1 or large than 100");
            _width = value;
        }
    }

    public int Height
    {
        get => _height;
        private set
        {
            if (value is < 1 or > 100)
                throw new ArgumentOutOfRangeException($"{nameof(Height)} can't be less than 1 or large than 100");
            _height = value;
        } 
    }

    public Size(int width, int height)
    {
        Width = width;
        Height = height;
    }
}


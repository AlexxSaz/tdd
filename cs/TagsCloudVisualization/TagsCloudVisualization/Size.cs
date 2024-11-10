using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization;

public class Size(int width, int height)
{
    private int _width = width;
    private int _height = height;

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
}


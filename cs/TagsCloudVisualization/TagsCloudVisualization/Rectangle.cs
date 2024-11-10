using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class Rectangle(Size size, Point center)
    {
        public Size Size { get; } = size;
        public Point Center { get; } = center;
    }
}

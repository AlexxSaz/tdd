using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class SizeShould
    {
        [TestCase(1,0)]
        [TestCase(0,1)]
        [TestCase(101,0)]
        [TestCase(0,101)]
        public void ThrowArgumentOutOfRangeException_AfterCreationWith(int width, int height)
        {
            var createSize = () => new Size(width, height);

            createSize.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}

using Changes.Domain;
using FluentAssertions;
using Xunit;

namespace Changes.Tests
{
    public class TransformerShould
    {
        [Fact]
        public void SwapAllPairsWhenInitialisedWithX()
        {
            var transformer = Transformer.FromPlaceNotation("x");

            transformer.ApplyTo(new object[] {1, 2}).Should().Equal(2, 1);
            transformer.ApplyTo(new object[] {1, 2, 3}).Should().Equal(2, 1, 3);
            transformer.ApplyTo(new object[] {1, 2, 3, 4}).Should().Equal(2, 1, 4, 3);
            transformer.ApplyTo(new object[] {1, 2, 3, 4, 5, 6, 7, 8}).Should().Equal(2, 1, 4, 3, 6, 5, 8, 7);
        }

        [Fact]
        public void SwapAllButFirstWhenInitialisedWith1()
        {
            var transformer = Transformer.FromPlaceNotation("1");

            transformer.ApplyTo(new object[] {1, 2, 3}).Should().Equal(1, 3, 2);
            transformer.ApplyTo(new object[] {1, 2, 3, 4}).Should().Equal(1, 3, 2, 4);
            transformer.ApplyTo(new object[] {1, 2, 3, 4, 5, 6, 7, 8}).Should().Equal(1, 3, 2, 5, 4, 7, 6, 8);
        }

        [Fact]
        public void SkipBell3WhenInitialisedWith3()
        {
            var transformer = Transformer.FromPlaceNotation("3");

            transformer.ApplyTo(new object[] {1, 2, 3}).Should().Equal(2, 1, 3);
            transformer.ApplyTo(new object[] {1, 2, 3, 4, 5, 6, 7, 8}).Should().Equal(2, 1, 3, 5, 4, 7, 6, 8);
        }
        
        [Fact]
        public void SkipBells3And4WhenInitialisedWith34()
        {
            var transformer = Transformer.FromPlaceNotation("34");

            transformer.ApplyTo(new object[] {1, 2, 3, 4, 5, 6, 7, 8}).Should().Equal(2, 1, 3, 4, 6, 5, 8, 7);
        }
    }
}
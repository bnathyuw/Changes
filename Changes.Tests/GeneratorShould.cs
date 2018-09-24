using System.Linq;
using FluentAssertions;
using NSubstitute;
using Xunit;
using Changes.Domain;

namespace Changes.Tests
{
    public class GeneratorShould
    {
        private readonly object[] _seedSequence = {1, "Hello", true};
        private readonly object[] _firstSequence = {3, 'X', "Banana"};
        private readonly object[] _secondSequence = {0xF, false, "microphone"};

        private readonly Generator _generator;

        public GeneratorShould()
        {
            var firstTransformation = Substitute.For<ITransformation>();
            var secondTransformation = Substitute.For<ITransformation>();

            firstTransformation.ApplyTo(_seedSequence).Returns(_firstSequence);
            secondTransformation.ApplyTo(_firstSequence).Returns(_secondSequence);

            var enumerable = new []{firstTransformation, secondTransformation};
            var transformations = Substitute.For<ITransformations>();
            transformations.GetEnumerator().Returns(info => transformations.GetEnumerator());

            _generator = new Generator(_seedSequence, new Transformations(enumerable));
        }
        
        [Fact]
        public void ReturnSeedSequenceFirst()
        {
            _generator.First().Should().Equal(1, "Hello", true);
        }

        [Fact]
        public void ReturnResultOfApplyingFirstTransformationToSeedSequence()
        {
            _generator.Skip(1).First().Should().Equal(_firstSequence);
        }
    }
}
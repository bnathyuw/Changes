using System.Collections.Generic;
using System.Linq;

namespace Changes.Domain
{
    public class Transformer : ITransformation
    {
        private static readonly Dictionary<char, int> Names = new Dictionary<char, int>
        {
            {'1', 0},
            {'2', 1},
            {'3', 2},
            {'4', 3},
            {'5', 4},
            {'6', 5},
            {'7', 6},
            {'8', 7},
            {'9', 8},
            {'T', 9},
            {'E', 10}
        };

        private readonly IEnumerable<int> _bellsToSkip;

        private Transformer(IEnumerable<int> bellsToSkip) => _bellsToSkip = bellsToSkip;

        private static IEnumerable<int> Parse(char arg) => Names.Where(x => x.Key == arg).Select(x => x.Value);

        public static ITransformation FromPlaceNotation(string placeNotation) => new Transformer(placeNotation.SelectMany(Parse));

        public IEnumerable<object> ApplyTo(object[] seedSequence) => new SequenceGenerator(seedSequence, _bellsToSkip).Generate();
    }
}
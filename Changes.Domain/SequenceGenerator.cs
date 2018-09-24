using System.Collections.Generic;
using System.Linq;

namespace Changes.Domain
{
    public class SequenceGenerator
    {
        public SequenceGenerator(object[] seedSequence, IEnumerable<int> bellsToSkip)
        {
            _seedSequence = seedSequence;
            _bellsToSkip = bellsToSkip;
        }

        private readonly object[] _seedSequence;
        private readonly IEnumerable<int> _bellsToSkip;

        public IEnumerable<object> Generate()
        {
            var length = _seedSequence.Length;
            for (var i = 0; i < length;)
            {
                if (_bellsToSkip.Contains(i))
                {
                    yield return _seedSequence[i];
                    i++;
                    continue;
                }

                if (i + 1 == length)
                {
                    yield return _seedSequence[i];
                    break;
                }

                yield return _seedSequence[i + 1];
                yield return _seedSequence[i];

                i += 2;
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Changes.Domain
{
    public class Generator : IEnumerable<object[]>
    {
        private readonly Transformations _transformations;
        private readonly object[] _seedSequence;

        public Generator(object[] seedSequence, Transformations transformations)
        {
            _seedSequence = seedSequence; 
            _transformations = transformations;
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            var currentSequence = _seedSequence;
            foreach (var transformation in _transformations)
            {
                yield return currentSequence;
                currentSequence = transformation.ApplyTo(currentSequence).ToArray();
                if (currentSequence.SequenceEqual(_seedSequence))
                {
                    yield break;
                }                
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
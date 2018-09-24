using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Changes.Domain
{
    public class Transformations : ITransformations
    {
        private readonly IEnumerable<ITransformation> _values;

        public Transformations(IEnumerable<ITransformation> transformations) => _values = transformations;

        public IEnumerator<ITransformation> GetEnumerator()
        {
            while (true)
            {
                foreach (var transformation in _values)
                {
                    yield return transformation;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static Transformations FromPlaceNotation(string placeNotation) => new Transformations(Parse(placeNotation));

        private static IEnumerable<ITransformation> Parse(string placeNotation) =>
            placeNotation.Contains(",")
                ? ParsePalindromic(placeNotation)
                : ParseSimple(placeNotation);

        private static IEnumerable<ITransformation> ParseSimple(string placeNotation) => 
            Normalise(placeNotation)
                .Split('.', StringSplitOptions.RemoveEmptyEntries)
                .Select(Transformer.FromPlaceNotation);

        private static string Normalise(string placeNotation) => placeNotation.Replace("x", ".x.");

        private static IEnumerable<ITransformation> ParsePalindromic(string placeNotation)
        {
            var parts = Normalise(placeNotation).Split(',');
            var foo = parts[0];
            var bar = parts[1];
            var changes = foo.Split('.', StringSplitOptions.RemoveEmptyEntries);
            return changes.Concat(changes.Reverse().Skip(1)).Concat(new[] {bar}).Select(Transformer.FromPlaceNotation);
        }

        public static Transformations operator *(Transformations multiplicand, int multiplier) => new Transformations(multiplicand.MultiplyValues(multiplier));

        private IEnumerable<ITransformation> MultiplyValues(int multiplier) => Enumerable.Repeat(this, multiplier).SelectMany(x => x._values);

        public static Transformations operator +(Transformations lhs, Transformations rhs) => new Transformations(lhs.AddValues(rhs));

        private IEnumerable<ITransformation> AddValues(Transformations rhs) => _values.Concat(rhs._values);
    }
}
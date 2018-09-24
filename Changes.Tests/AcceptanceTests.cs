using System.Collections.Generic;
using System.Linq;
using ApprovalTests.Reporters;
using FluentAssertions;
using Xunit;
using static System.Environment;
using static System.String;
using static ApprovalTests.Approvals;
using static Changes.Domain.Transformations;
using Changes.Domain;

namespace Changes.Tests
{
    [UseReporter(typeof(XUnit2Reporter))]
    public class AcceptanceTests
    {
        private static readonly Transformations OnceOnlyTransformations = FromPlaceNotation("x");
        private static readonly Transformations PlainHuntTransformations = FromPlaceNotation("x1");
        private static readonly Transformations StNicholasBobTransformations = FromPlaceNotation("34.14x34,12");
        private static readonly Transformations StedmanDoublesTransformations = FromPlaceNotation("3.1.5.3.1.3,1");
        private static readonly Transformations StedmanDoublesWithSingle = FromPlaceNotation("3.1.5.3.1.345,1");
        private static readonly Transformations StedmanDoublesWithSingleInLastLead = 
            StedmanDoublesTransformations * 4 + StedmanDoublesWithSingle;

        [Fact]
        public void TwoElementSequence()
        {
            var changes = new Generator(new object[] {1, 2}, OnceOnlyTransformations)
                .ToArray();

            changes.Should().HaveCount(2);
            Verify(changes.ToText());
        }


        [Fact]
        public void ThreeElementSequence()
        {
            var changes = new Generator(new object[] {'a', 'b', 'c'}, PlainHuntTransformations)
                .ToArray();

            changes.Should().HaveCount(6);
            Verify(changes.ToText());
        }

        [Fact]
        public void StedmanDoubles()
        {
            var changes = new Generator(new object[] {1, 2, 3, 4, 5}, StedmanDoublesTransformations)
                .ToArray();

            changes.Should().HaveCount(60);
            Verify(changes.ToText());
        }

        [Fact]
        public void StedmanDoublesWithSingles()
        {
            var changes = new Generator(new object[] {1, 2, 3, 4, 5}, StedmanDoublesWithSingleInLastLead)
                .ToArray();

            changes.Should().HaveCount(120);
            Verify(changes.ToText());
        }

        [Fact]
        public void StNicholasBobMinimus()
        {
            var changes = new Generator(new object[] {1, 2, 3, 4, 5}, StNicholasBobTransformations).ToArray();

            changes.Should().HaveCount(24);
            Verify(changes.ToText());
        }

        [Fact]
        public void EmmaThomas()
        {
            var one = new Generator(new object[] {"mas"}, OnceOnlyTransformations).ToArray();
            var two = new Generator(new object[] {"Tho", "mas"}, OnceOnlyTransformations).ToArray();
            var three = new Generator(new object[] {"et", "Tho", "mas"}, PlainHuntTransformations).ToArray();
            var four = new Generator(new object[] {"ma", "et", "Tho", "mas"}, StNicholasBobTransformations).ToArray();
            var five = new Generator(new object[] {"Em", "ma", "et", "Tho", "mas"}, StedmanDoublesWithSingleInLastLead).ToArray();
            var six = new Generator(new object[] {"Em", "ma", "et", "Tho"}, StNicholasBobTransformations).ToArray();
            var seven = new Generator(new object[] {"Em", "ma", "et"}, PlainHuntTransformations).ToArray();
            var eight = new Generator(new object[] {"Em", "ma"}, OnceOnlyTransformations).ToArray();
            var nine = new Generator(new object[] {"Em"}, OnceOnlyTransformations).ToArray();

            Verify(FormatChanges.ToTabbedText(one, two, three, four, five, six, seven, eight, nine));
        }
    }

    public static class FormatChanges
    {
        public static string ToText(this IEnumerable<object[]> changes) => Join(NewLine, changes.Select(ToUndelimitedList));
        private static string ToUndelimitedList(object[] x) => Join("", x);
        public static string ToTabbedText(params object[][][] peals) => Join(NewLine, peals.Select(AsLines));
        private static string AsLines(object[][] changes) => Join(NewLine, changes.Select(TabbedList));
        private static string TabbedList(object[] x) => Join("\t", x);
    }
}
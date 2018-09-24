using System.Collections.Generic;

namespace Changes.Domain
{
    public interface ITransformation
    {
        IEnumerable<object> ApplyTo(object[] seedSequence);
    }
}
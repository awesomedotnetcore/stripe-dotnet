namespace Stripe
{
    /// <summary>
    /// Represents a value that may be of one of several different types.
    /// </summary>
    public interface IAnyOf
    {
        /// <summary>Gets the current value.</summary>
        /// <returns>The current value.</returns>
        object GetValue();
    }
}

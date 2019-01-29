namespace Stripe
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Abstract base class for <c>AnyOf&lt;&gt;</c> generic classes.
    /// </summary>
    public abstract class AnyOf : IAnyOf
    {
        /// <summary>Gets the current value.</summary>
        /// <returns>The current value.</returns>
        public abstract object GetValue();

        public override bool Equals(object obj) => this.GetValue().Equals(obj);

        public override int GetHashCode() => this.GetValue().GetHashCode();

        public override string ToString() => this.GetValue().ToString();
    }

    /// <summary>
    /// <see cref="AnyOf{T1, T2}"/> is a generic class that can hold a value of one of two different
    /// types. It uses implicit conversion operators to seamlessly accept or return either type.
    /// This is used to represent polymorphic request parameters, i.e. parameters that can
    /// be different types (typically a string or an options class).
    /// </summary>
    /// <typeparam name="T1">The first possible type of the value.</typeparam>
    /// <typeparam name="T2">The second possible type of the value.</typeparam>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Generic variant")]
    public class AnyOf<T1, T2> : AnyOf
    {
        private readonly T1 value1;
        private readonly T2 value2;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnyOf{T1, T2}"/> class with type <c>T1</c>.
        /// </summary>
        /// <param name="value">The value to hold.</param>
        public AnyOf(T1 value)
        {
            this.value1 = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnyOf{T1, T2}"/> class with type <c>T2</c>.
        /// </summary>
        /// <param name="value">The value to hold.</param>
        public AnyOf(T2 value)
        {
            this.value2 = value;
        }

        /// <summary>
        /// Converts a value of type <c>T1</c> to an <see cref="AnyOf{T1, T2}"/> object.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>An <see cref="AnyOf{T1, T2}"/> object that holds the value.</returns>
        public static implicit operator AnyOf<T1, T2>(T1 value) => new AnyOf<T1, T2>(value);

        /// <summary>
        /// Converts a value of type <c>T2</c> to an <see cref="AnyOf{T1, T2}"/> object.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>An <see cref="AnyOf{T1, T2}"/> object that holds the value.</returns>
        public static implicit operator AnyOf<T1, T2>(T2 value) => new AnyOf<T1, T2>(value);

        /// <summary>
        /// Converts an <see cref="AnyOf{T1, T2}"/> object to a value of type <c>T1</c>,
        /// </summary>
        /// <param name="anyOf">The <see cref="AnyOf{T1, T2}"/> object to convert.</param>
        /// <returns>
        /// A value of type <c>T1</c>. If the <see cref="AnyOf{T1, T2}"/> object currently
        /// holds a value of a different type, the default value for type <c>T1</c> is returned.
        /// </returns>
        public static implicit operator T1(AnyOf<T1, T2> anyOf) => anyOf.value1;

        /// <summary>
        /// Converts a value of type <c>T2</c> to an <see cref="AnyOf{T1, T2}"/> object.
        /// </summary>
        /// <param name="anyOf">The <see cref="AnyOf{T1, T2}"/> object to convert.</param>
        /// <returns>
        /// A value of type <c>T2</c>. If the <see cref="AnyOf{T1, T2}"/> object currently
        /// holds a value of a different type, the default value for type <c>T2</c> is returned.
        /// </returns>
        public static implicit operator T2(AnyOf<T1, T2> anyOf) => anyOf.value2;

        /// <summary>Gets the current value.</summary>
        /// <returns>The current value.</returns>
        public override object GetValue()
        {
            if (!EqualityComparer<T1>.Default.Equals(this.value1, default(T1)))
            {
                return this.value1;
            }
            else if (!EqualityComparer<T2>.Default.Equals(this.value2, default(T2)))
            {
                return this.value2;
            }
            else
            {
                return null;
            }
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Generic variant")]
    public class AnyOf<T1, T2, T3> : AnyOf
    {
        private readonly T1 value1;
        private readonly T2 value2;
        private readonly T3 value3;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnyOf{T1, T2, T3}"/> class with type
        /// <c>T1</c>.
        /// </summary>
        /// <param name="value">The value to hold.</param>
        public AnyOf(T1 value)
        {
            this.value1 = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnyOf{T1, T2, T3}"/> class with type
        /// <c>T2</c>.
        /// </summary>
        /// <param name="value">The value to hold.</param>
        public AnyOf(T2 value)
        {
            this.value2 = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnyOf{T1, T2, T3}"/> class with type
        /// <c>T3</c>.
        /// </summary>
        /// <param name="value">The value to hold.</param>
        public AnyOf(T3 value)
        {
            this.value3 = value;
        }

        /// <summary>
        /// Converts a value of type <c>T1</c> to an <see cref="AnyOf{T1, T2, T3}"/> object.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>An <see cref="AnyOf{T1, T2, T3}"/> object that holds the value.</returns>
        public static implicit operator AnyOf<T1, T2, T3>(T1 value) => new AnyOf<T1, T2, T3>(value);

        /// <summary>
        /// Converts a value of type <c>T2</c> to an <see cref="AnyOf{T1, T2, T3}"/> object.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>An <see cref="AnyOf{T1, T2, T3}"/> object that holds the value.</returns>
        public static implicit operator AnyOf<T1, T2, T3>(T2 value) => new AnyOf<T1, T2, T3>(value);

        /// <summary>
        /// Converts a value of type <c>T3</c> to an <see cref="AnyOf{T1, T2, T3}"/> object.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>An <see cref="AnyOf{T1, T2, T3}"/> object that holds the value.</returns>
        public static implicit operator AnyOf<T1, T2, T3>(T3 value) => new AnyOf<T1, T2, T3>(value);

        /// <summary>
        /// Converts an <see cref="AnyOf{T1, T2, T3}"/> object to a value of type <c>T1</c>,
        /// </summary>
        /// <param name="anyOf">The <see cref="AnyOf{T1, T2, T3}"/> object to convert.</param>
        /// <returns>
        /// A value of type <c>T1</c>. If the <see cref="AnyOf{T1, T2, T3}"/> object currently
        /// holds a value of a different type, the default value for type <c>T3</c> is returned.
        /// </returns>
        public static implicit operator T1(AnyOf<T1, T2, T3> anyOf) => anyOf.value1;

        /// <summary>
        /// Converts an <see cref="AnyOf{T1, T2, T3}"/> object to a value of type <c>T2</c>,
        /// </summary>
        /// <param name="anyOf">The <see cref="AnyOf{T1, T2, T3}"/> object to convert.</param>
        /// <returns>
        /// A value of type <c>T2</c>. If the <see cref="AnyOf{T1, T2, T3}"/> object currently
        /// holds a value of a different type, the default value for type <c>T3</c> is returned.
        /// </returns>
        public static implicit operator T2(AnyOf<T1, T2, T3> anyOf) => anyOf.value2;

        /// <summary>
        /// Converts an <see cref="AnyOf{T1, T2, T3}"/> object to a value of type <c>T3</c>,
        /// </summary>
        /// <param name="anyOf">The <see cref="AnyOf{T1, T2, T3}"/> object to convert.</param>
        /// <returns>
        /// A value of type <c>T3</c>. If the <see cref="AnyOf{T1, T2, T3}"/> object currently
        /// holds a value of a different type, the default value for type <c>T3</c> is returned.
        /// </returns>
        public static implicit operator T3(AnyOf<T1, T2, T3> anyOf) => anyOf.value3;

         /// <summary>Gets the current value.</summary>
        /// <returns>The current value.</returns>
       public override object GetValue()
        {
            if (!EqualityComparer<T1>.Default.Equals(this.value1, default(T1)))
            {
                return this.value1;
            }
            else if (!EqualityComparer<T2>.Default.Equals(this.value2, default(T2)))
            {
                return this.value2;
            }
            else if (!EqualityComparer<T3>.Default.Equals(this.value3, default(T3)))
            {
                return this.value3;
            }
            else
            {
                return null;
            }
        }
    }
}

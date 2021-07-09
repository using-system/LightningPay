using System;
using System.Diagnostics.CodeAnalysis;

namespace LightningPay
{
    /// <summary>
    /// Provides options for JSON.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.All)]
    public sealed class JsonAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonAttribute"/> class.
        /// </summary>
        public JsonAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonAttribute"/> class.
        /// </summary>
        /// <param name="name">The name to use for JSON serialization and deserialization.</param>
        public JsonAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets or sets the name to use for JSON serialization and deserialization.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to ignore this instance's owner when serializing.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance's owner must be ignored when serializing; otherwise, <c>false</c>.
        /// </value>
        public bool IgnoreWhenSerializing { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to ignore this instance's owner when deserializing.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance's owner must be ignored when deserializing; otherwise, <c>false</c>.
        /// </value>
        public bool IgnoreWhenDeserializing { get; set; }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        public object DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has a default value. In this case, it's defined by the DefaultValue property.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has default value; otherwise, <c>false</c>.
        /// </value>
        public bool HasDefaultValue { get; set; }
    }
}

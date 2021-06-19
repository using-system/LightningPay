using System;

namespace LightningPay
{
    public class ApiException : Exception
    {
        public string Code { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DryBikeDomainException"/> class.
        /// </summary>
        public ApiException()
        { }


        /// <summary>
        /// Initializes a new instance of the <see cref="DryBikeDomainException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ApiException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DryBikeDomainException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ApiException(string message, string code)
            : base(message)
        {
            this.Code = code;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DryBikeDomainException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public ApiException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

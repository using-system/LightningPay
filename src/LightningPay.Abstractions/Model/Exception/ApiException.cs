﻿using System;
using System.Net;

namespace LightningPay
{
    /// <summary>
    ///  Exception thrown by LightningPay
    /// </summary>
    public class ApiException : Exception
    {
        /// <summary>Gets or sets the http status code.</summary>
        /// <value>The http status code.</value>
        public HttpStatusCode Code { get; set; }

        /// <summary>Gets or sets the response data.</summary>
        /// <value>The response data.</value>
        public string ResponseData { get; set; }

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
        public ApiException(string message, 
            HttpStatusCode code,
            string responseData = null,
            Exception innerException = null)
            : base(message, innerException)
        {
            this.Code = code;
            this.ResponseData = responseData;
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

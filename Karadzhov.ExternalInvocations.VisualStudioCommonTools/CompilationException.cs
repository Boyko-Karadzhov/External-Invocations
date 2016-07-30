using System;
using System.Runtime.Serialization;

namespace Karadzhov.ExternalInvocations.VisualStudioCommonTools
{
    /// <summary>
    /// This is an exception that is thrown when compilation fails.
    /// </summary>
    [Serializable]
    public class CompilationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompilationException"/> class.
        /// </summary>
        public CompilationException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompilationException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public CompilationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompilationException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public CompilationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompilationException"/> class.
        /// </summary>
        /// <param name="serializationInfo">The serialization information.</param>
        /// <param name="context">The context.</param>
        protected CompilationException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
            this.CompilerOutput = serializationInfo.GetString(nameof(this.CompilerOutput));
            this.CompilerError = serializationInfo.GetString(nameof(this.CompilerError));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompilationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="compilerOutput">The compiler output.</param>
        /// <param name="compilerError">The compiler error.</param>
        public CompilationException(string message, string compilerOutput, string compilerError) 
            : base(message)
        {
            this.CompilerOutput = compilerOutput;
            this.CompilerError = compilerError;
        }

        /// <summary>
        /// Gets the compiler output.
        /// </summary>
        /// <value>
        /// The compiler output.
        /// </value>
        public string CompilerOutput { get; private set; }

        /// <summary>
        /// Gets the compiler error.
        /// </summary>
        /// <value>
        /// The compiler error.
        /// </value>
        public string CompilerError { get; private set; }

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        /// <exception cref="System.ArgumentNullException">info</exception>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter" />
        /// </PermissionSet>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (null == info)
                throw new ArgumentNullException(nameof(info));

            info.AddValue(nameof(this.CompilerOutput), this.CompilerOutput);
            info.AddValue(nameof(this.CompilerError), this.CompilerError);

            base.GetObjectData(info, context);
        }
    }
}

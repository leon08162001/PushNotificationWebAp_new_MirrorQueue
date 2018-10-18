using System;
using System.Runtime.Serialization;

namespace WebApiAp
{
    public enum LASErrror
    {
        None,
        NoAnyRow,
        ObjectNotFound,
        SystemError,
        Other
    }
    public class LASException : Exception, ISerializable
    {
        protected LASErrror _ErrorCode = LASErrror.None;
        protected string _ErrorMessage;

        public LASErrror ErrorCode
        {
            get { return _ErrorCode; }
        }

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
        }

        public LASException()
            : base()
        {
            _ErrorMessage = Enum.GetName(typeof(LASErrror), _ErrorCode);
        }
        public LASException(LASErrror ErrorCode)
        {
            _ErrorCode = ErrorCode;
            _ErrorMessage = Enum.GetName(typeof(LASErrror), _ErrorCode);
        }
        public LASException(string message)
            : base(message)
        {
            _ErrorCode = LASErrror.Other;
            _ErrorMessage = message;
        }
        public LASException(string message, Exception inner)
            : base(message, inner)
        {
            _ErrorCode = LASErrror.Other;
            _ErrorMessage = message;

        }
        // This constructor is needed for serialization.
        protected LASException(SerializationInfo info, StreamingContext context)
                        : base(info, context)
        {

        }
    }
}
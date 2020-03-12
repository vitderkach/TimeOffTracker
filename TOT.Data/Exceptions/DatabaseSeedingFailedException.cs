using System;

namespace TOT.Data.Exceptions
{
    // Throws when there is a problem with database seeding
    [Serializable]
    public class DatabaseSeedingFailedException: ApplicationException
    {

        public DatabaseSeedingFailedException() { }

        public DatabaseSeedingFailedException(string message) : base(message) { }

        public DatabaseSeedingFailedException(string message, System.Exception inner)
            : base(message, inner) { }

        protected DatabaseSeedingFailedException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
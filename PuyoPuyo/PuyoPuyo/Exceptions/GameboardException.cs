using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PuyoPuyo.Exceptions
{
    public class PlayerException : Exception
    {
        public enum OfType
        {
            NotAlive,
            NoMoveAvailable,
            SpawnError,
            NotCreated,
        }

        private static string notAliveMessage = "Player not alive !";
        private static string noMoveAvailableMessage = "No move available !";
        private static string spawnErrorMessage = "Can't spawn player !";
        private static string notCreatedMessage = "Can't spawn player !";

        private static Dictionary<OfType, string> keyValuePairs = new Dictionary<OfType, string>()
        {
            { OfType.NotAlive, notAliveMessage },
            { OfType.NoMoveAvailable, noMoveAvailableMessage },
            { OfType.SpawnError, spawnErrorMessage },
            { OfType.NotCreated, notCreatedMessage },
        };

        public OfType ExceptionType { get; private set; }

        public PlayerException(OfType exceptionType) : base(keyValuePairs[exceptionType])
        {
        }

        public PlayerException(OfType exceptionType, Exception innerException) : base(keyValuePairs[exceptionType], innerException)
        {
        }

        protected PlayerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class InvalidActionException : Exception
    {
        private static string errorMessage = "Invalid action";

        public InvalidActionException() : base(errorMessage)
        {
        }

        public InvalidActionException(Exception innerException) : base(errorMessage, innerException)
        {
        }

        protected InvalidActionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

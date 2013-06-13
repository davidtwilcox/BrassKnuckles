using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BrassKnuckles
{
    /// <summary>
    /// Format for passing messages between systems without tightly coupling them.
    /// </summary>
    [Serializable]
    public sealed class Message
    {
        #region Constants

        private const int expiredId = -1;
        private const int maxTimeToLive = 60;

        #endregion

        #region Private fields

        private static int _nextId;
        private static Queue<int> _availableIds = new Queue<int>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the maximum time to live allowed for any message.
        /// </summary>
        /// <remarks>Measured in game ticks. With default settings, one game tick is 1/60 second.</remarks>
        public static int MaxTimeToLive
        {
            get { return maxTimeToLive; }
        }

        /// <summary>
        /// Gets a unique ID for the message.
        /// </summary>
        /// <remarks>Message IDs will be recycled over time; this value is unique only
        /// until the message is expired.</remarks>
        public int Id { get; private set; }

        /// <summary>
        /// Gets a numeric value indicating the type of message.
        /// </summary>
        public int Code { get; private set; }

        private int _timeToLive;
        /// <summary>
        /// Gets the maximum number of game ticks a message will remain
        /// available before being deleted.
        /// </summary>
        public int TimeToLive
        {
            get { return _timeToLive; }
            private set { _timeToLive = Math.Min(MaxTimeToLive, Math.Max(0, value)); }
        }

        /// <summary>
        /// Gets a value indicating whether or not the message has been expired.
        /// </summary>
        public bool IsExpired
        {
            get { return Id == expiredId; }
        }

        /// <summary>
        /// Gets data associated with the message.
        /// </summary>
        /// <remarks>Type of data is dependent on the message code and publisher
        /// of the message. This should be detailed in the documentation for the
        /// message code.</remarks>
        public object Data { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Message instance with the specified code and data,
        /// and a time to live of 1 game tick.
        /// </summary>
        /// <param name="code">Numeric value indicating the type of message.</param>
        /// <param name="data">Data associated with the message.</param>
        public Message(int code, object data)
            : this(code, 0, data)
        {
        }

        /// <summary>
        /// Creates a new Message instance with the specified code, time to live and data.
        /// </summary>
        /// <param name="code">Numeric value indicating the type of message.</param>
        /// <param name="timeToLive">Maximum number of game ticks a message will remain
        /// available before being deleted.</param>
        /// <param name="data">Data associated with the message.</param>
        public Message(int code, int timeToLive, object data)
        {
            Id = GenerateId();
            Code = code;
            TimeToLive = timeToLive;
            Data = data;
        }

        #endregion

        #region Private methods

        private static int GenerateId()
        {
            if (_availableIds.Count > 0)
            {
                return _availableIds.Dequeue();
            }

            return ++_nextId;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Expires a message, making it invalid and recycling its ID.
        /// </summary>
        /// <param name="message">Message to expire.</param>
        public static void ExpireMessage(Message message)
        {
            lock (message)
            {
                _availableIds.Enqueue(message.Id);
                message.Id = expiredId;
            }
        }

        #endregion
    }
}

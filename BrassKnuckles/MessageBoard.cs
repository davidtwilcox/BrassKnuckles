using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BrassKnuckles.Utility;
using System.Threading.Tasks;
using System.Threading;

namespace BrassKnuckles
{
    /// <summary>
    /// Interface for classes handling <see cref="Message"/> instances being passed between game systems.
    /// </summary>
    /// <remarks>Thread-safe (hopefully).</remarks>
    public interface IMessageBoard : IGameComponent
    {
        #region Methods

        /// <summary>
        /// Posts a new message to the message board with the specified code and data.
        /// </summary>
        /// <param name="code">Numeric code identifying the type of message.</param>
        /// <param name="data">Data associated with the message.</param>
        void PostMessage(int code, object data);

        /// <summary>
        /// Posts a new message to the message board with the specified code, time to live, and data.
        /// </summary>
        /// <param name="code">Numeric code identifying the type of message.</param>
        /// <param name="timeToLive">Maximum number of game ticks the message will exist before being expired.</param>
        /// <param name="data">Data associated with the message.</param>
        void PostMessage(int code, int timeToLive, object data);

        /// <summary>
        /// Posts the provided <see cref="Message"/> instance to the message board.
        /// </summary>
        /// <param name="message"><see cref="Message"/> instance to post.</param>
        void PostMessage(Message message);

        /// <summary>
        /// Gets all messages on the message board with the specified code.
        /// </summary>
        /// <param name="code">Numeric code identifying the type of message to retrieve.</param>
        /// <returns>A <see cref="Bag"/> instance containing all messages with the specified code, or null if none are found.</returns>
        List<Message> GetMessages(int code);

        #endregion
    }

    /// <summary>
    /// Central repository for <see cref="Message"/> instances being passed between game systems.
    /// </summary>
    /// <remarks>Thread-safe (hopefully).</remarks>
    public class MessageBoard : GameComponent, IMessageBoard
    {
        #region Private fields

        private int _gameTick;
        private ConcurrentDictionary<int, Message> _messageTable;
        private ConcurrentDictionary<int, List<Message>> _messagesByExpiration;
        private ConcurrentDictionary<int, List<Message>> _cachedMessagesByCode;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new MessageBoard instance.
        /// </summary>
        public MessageBoard(Game game)
            : base(game)
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Posts a new message to the message board with the specified code and data.
        /// </summary>
        /// <param name="code">Numeric code identifying the type of message.</param>
        /// <param name="data">Data associated with the message.</param>
        public void PostMessage(int code, object data)
        {
            PostMessage(new Message(code, data));
        }

        /// <summary>
        /// Posts a new message to the message board with the specified code, time to live, and data.
        /// </summary>
        /// <param name="code">Numeric code identifying the type of message.</param>
        /// <param name="timeToLive">Maximum number of game ticks the message will exist before being expired.</param>
        /// <param name="data">Data associated with the message.</param>
        public void PostMessage(int code, int timeToLive, object data)
        {
            PostMessage(new Message(code, timeToLive, data));
        }

        /// <summary>
        /// Posts the provided <see cref="Message"/> instance to the message board.
        /// </summary>
        /// <param name="message"><see cref="Message"/> instance to post.</param>
        public void PostMessage(Message message)
        {
            _messageTable.AddOrUpdate(message.Id, message,
                (key, existingValue) =>
                {
                    if (message.Code != existingValue.Code)
                    {
                        throw new ArgumentException("Duplicate unexpired messages with the same ID are not allowed");
                    }
                    return message;
                });

            int expiration = (_gameTick + message.TimeToLive + 1) % Message.MaxTimeToLive;
            if (_messagesByExpiration[expiration] == null)
            {
                _messagesByExpiration[expiration] = new List<Message>();
            }
            _messagesByExpiration[expiration].Add(message);
        }

        /// <summary>
        /// Gets all messages on the message board with the specified code.
        /// </summary>
        /// <param name="code">Numeric code identifying the type of message to retrieve.</param>
        /// <returns>A <see cref="List"/> instance containing all messages with the specified code, or null if none are found.</returns>
        public List<Message> GetMessages(int code)
        {
            List<Message> messages = null;
            if (!_cachedMessagesByCode.TryGetValue(code, out messages))
            {
                var query = from m in _messageTable.Values
                            where m.Code == code
                            select m;
                if (query.Any())
                {
                    messages = query.ToList<Message>();
                    _cachedMessagesByCode.AddOrUpdate(code, messages,
                        (key, existingValue) =>
                        {
                            return messages;
                        });
                }
            }

            return messages;
        }

        /// <summary>
        /// Initializes the MessageBoard instance.
        /// </summary>
        public override void Initialize()
        {
            _gameTick = 0;

            _messageTable = new ConcurrentDictionary<int, Message>();

            _messagesByExpiration = new ConcurrentDictionary<int, List<Message>>();
            for (int i = 0; i < Message.MaxTimeToLive; ++i)
            {
                _messagesByExpiration.AddOrUpdate(i, new List<Message>(),
                    (key, existingValue) =>
                    {
                        return new List<Message>();
                    });
            }

            _cachedMessagesByCode = new ConcurrentDictionary<int, List<Message>>();

            base.Initialize();
        }

        /// <summary>
        /// Updates the message board.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of game timing.</param>
        public override void Update(GameTime gameTime)
        {
            _cachedMessagesByCode.Clear();

            _gameTick = (_gameTick + 1) % Message.MaxTimeToLive;

            List<Message> expiringMessages = _messagesByExpiration[_gameTick];
            Message tempMessage;
            foreach (Message message in expiringMessages)
            {
                if (_messageTable.TryRemove(message.Id, out tempMessage))
                {
                    Message.ExpireMessage(message);
                }
            }
            expiringMessages.Clear();
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrassKnuckles.AI
{
    /// <summary>
    /// Stores arbitrary data used by AI as parameters to actions and behavior tree nodes.
    /// </summary>
    public class Blackboard
    {
        #region Private fields

        private Dictionary<string, object> _dataTable;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the blackboard's parent.
        /// </summary>
        /// <remarks>Can be null.</remarks>
        public Blackboard Parent { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Blackboard instance with the specified parent.
        /// </summary>
        /// <param name="parent">Blackboard instance that is the parent of
        /// the new instance. Can be null.</param>
        public Blackboard(Blackboard parent)
        {
            Parent = parent;
            _dataTable = new Dictionary<string, object>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets the data stored with the specified key.
        /// </summary>
        /// <param name="key">Unique key used to look up data.</param>
        /// <returns>The data stored with the specified key, or null if not found.</returns>
        /// <remarks>If the blackboard has a non-null parent and this instance does not have data
        /// stored with the specified key, the request will be passed to the parent.</remarks>
        public object Read(string key)
        {
            if (_dataTable.ContainsKey(key))
            {
                return _dataTable[key];
            }
            else if (Parent != null)
            {
                return Parent.Read(key);
            }

            return null;
        }

        /// <summary>
        /// Stores the provided data using the specified key.
        /// </summary>
        /// <param name="key">Unique key used to look up data.</param>
        /// <param name="data">Value to be stored.</param>
        /// <remarks>If the key already exists on this blackboard, the data
        /// stored with that key is replaced with the new data.</remarks>
        public void Write(string key, object data)
        {
            if (_dataTable.ContainsKey(key))
            {
                _dataTable[key] = data;
            }
            else
            {
                _dataTable.Add(key, data);
            }
        }

        /// <summary>
        /// Removes the data item associated with the specified key.
        /// </summary>
        /// <param name="key">Key of item to remove.</param>
        /// <returns>True if the item existed and was removed, or false if not.</returns>
        public bool Remove(string key)
        {
            return _dataTable.Remove(key);
        }

        #endregion
    }
}

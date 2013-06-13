using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrassKnuckles.EntityFramework.Managers
{
    /// <summary>
    /// Manages <see cref="Entity"/> tags.
    /// </summary>
    public sealed class TagManager : IManager
    {
        #region Private fields

        private EntityWorld _world;
        private Dictionary<string, Entity> _entitiesByTag;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new TagManager instance associated with the provided <see cref="EntityWorld"/>.
        /// </summary>
        /// <param name="world">The <see cref="EntityWorld"/> instance to associate with.</param>
        public TagManager(EntityWorld world)
        {
            _world = world;
            _entitiesByTag = new Dictionary<string, Entity>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Assigns a tag to an <see cref="Entity"/>.
        /// </summary>
        /// <param name="tag">Tag to assign.</param>
        /// <param name="entity"><see cref="Entity"/> instance to which the tag will be assigned.</param>
        public void AssignTag(string tag, Entity entity)
        {
            if (_entitiesByTag.ContainsKey(tag))
            {
                _entitiesByTag[tag] = entity;
            }
            else
            {
                _entitiesByTag.Add(tag, entity);
            }
        }

        /// <summary>
        /// Removes a tag from an <see cref="Entity"/>.
        /// </summary>
        /// <param name="tag">Tag to remove.</param>
        public void RemoveTag(string tag)
        {
            _entitiesByTag.Remove(tag);
        }

        /// <summary>
        /// Checks if the specified tag has been assigned to an entity.
        /// </summary>
        /// <param name="tag">Tag to look up.</param>
        /// <returns>True if the tag has been assigned to an entity, false if not.</returns>
        public bool IsTagAssigned(string tag)
        {
            return _entitiesByTag.ContainsKey(tag);
        }

        /// <summary>
        /// Gets the <see cref="Entity"/> instance assigned to the specified tag.
        /// </summary>
        /// <param name="tag">Tag to look up.</param>
        /// <returns>The <see cref="Entity"/> instance assigned to the specified tag, or null if none is assigned.</returns>
        public Entity GetEntity(string tag)
        {
            Entity entity;
            _entitiesByTag.TryGetValue(tag, out entity);
            if ((entity == null) || entity.IsActive)
            {
                return entity;
            }
            else
            {
                RemoveTag(tag);
                return null;
            }
        }

        /// <summary>
        /// Gets the tag assigned to the provided <see cref="Entity"/>.
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> instance to look up.</param>
        /// <returns>The tag assigned to the <see cref="Entity"/> provided, or an empty string if none is assigned.</returns>
        public string GetTagOfEntity(Entity entity)
        {
            foreach (var pair in _entitiesByTag)
            {
                if (pair.Value.Equals(entity))
                {
                    return pair.Key;
                }
            }

            return string.Empty;
        }

        #endregion
    }
}

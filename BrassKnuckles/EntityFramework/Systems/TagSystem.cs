using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrassKnuckles.EntityFramework.Systems
{
    /// <summary>
    /// System for processing all entities with a particular tag.
    /// </summary>
    public abstract class TagSystem : EntitySystem
    {
        #region Properties

        /// <summary>
        /// Gets or sets the tag to use.
        /// </summary>
        protected string Tag { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new TagSystem instance associated with the provided tag.
        /// </summary>
        /// <param name="tag">Tag to use.</param>
        public TagSystem(string tag)
        {
            Tag = tag;
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Processes all entities with a specified tag.
        /// </summary>
        /// <param name="entities"></param>
        /// <remarks>Does not process entities passed, but instead retrieves entities to process from the 
        /// shared <see cref="TagManager"/> instance.</remarks> 
        protected override void ProcessEntities(Dictionary<int, Entity> entities)
        {
            Entity entity = World.TagManager.GetEntity(Tag);
            if (entity != null)
            {
                Process(entity);
            }
        }

        #endregion
    }
}

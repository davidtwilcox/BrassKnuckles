using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrassKnuckles.EntityFramework;
using BrassKnuckles.EntityFramework.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BrassKnuckles.Geometry;

namespace BrassKnuckles.Renderer
{
    /// <summary>
    /// Renders on-screen objects.
    /// </summary>
    public sealed class RenderSystem : EntitySystem
    {
        #region Private fields

        private ComponentMapper<ImageComponent> _imageMapper;
        private ComponentMapper<PositionComponent> _positionMapper;

        #endregion

        #region Properties

#if DEBUG
        public int ImagesProcessed { get; private set; }

        public int ImagesRendered { get; private set; }
#endif

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new RenderSystem instance.
        /// </summary>
        public RenderSystem()
            : base(typeof(ImageComponent))
        {
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Processes all provided entities.
        /// </summary>
        /// <param name="entities">Dictionary containing the <see cref="Entity"/> instances to process,
        /// using their ID as the key.</param>
        protected override void ProcessEntities(Dictionary<int, Entity> entities)
        {
#if DEBUG
            ImagesProcessed = 0;
            ImagesRendered = 0;
#endif
            base.ProcessEntities(entities);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Initializes the render system.
        /// </summary>
        public override void Initialize()
        {
            _imageMapper = new ComponentMapper<ImageComponent>(World);
            _positionMapper = new ComponentMapper<PositionComponent>(World);

            base.Initialize();
        }

        /// <summary>
        /// Renders the provided <see cref="Entity"/> instance.
        /// </summary>
        /// <param name="entity"><see cref="Entity"/> instance to render.</param>
        public override void Process(Entity entity)
        {
            ImageComponent imageComponent = _imageMapper.Get(entity);

            if ((imageComponent == null) && !imageComponent.IsVisible)
            {
                return;
            }

            Vector2 position = imageComponent.Position;
            if (imageComponent.PositionMode == PositionMode.External)
            {
                PositionComponent positionComponent = _positionMapper.Get(entity);
                if (positionComponent != null)
                {
                    position += positionComponent.Position;
                }
            }

            float rotation = imageComponent.Rotation;
            if (imageComponent.RotationMode == RotationMode.External)
            {
                PositionComponent positionComponent = _positionMapper.Get(entity);
                if (positionComponent != null)
                {
                    rotation += positionComponent.Rotation;
                }
            }

            Director.SharedDirector.SpriteBatch.Draw(imageComponent.Texture,
                                                        position,
                                                        imageComponent.Source,
                                                        imageComponent.Tint,
                                                        rotation,
                                                        imageComponent.Origin,
                                                        imageComponent.Scale,
                                                        imageComponent.Effects,
                                                        imageComponent.Layer);
#if DEBUG
            ++ImagesRendered;
            ++ImagesProcessed;
#endif
        }

        #endregion
    }
}

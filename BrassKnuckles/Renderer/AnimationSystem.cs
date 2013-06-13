using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrassKnuckles.EntityFramework.Systems;
using BrassKnuckles.EntityFramework;
using BrassKnuckles.EntityFramework.Managers;
using BrassKnuckles.Utility;

namespace BrassKnuckles.Renderer
{
    /// <summary>
    /// Handles animation on-screen objects. Works with <see cref="RenderSystem"/>.
    /// </summary>
    public class AnimationSystem : ParallelIntervalEntitySystem
    {
        #region Private fields

        private ComponentMapper<AnimationComponent> _animationMapper;
        private ComponentMapper<ImageComponent> _imageMapper;
        private float _frameRate;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new AnimationSystem instance with default 1/30 second frame rate 
        /// (i.e., called every other game tick).
        /// </summary>
        public AnimationSystem()
            : this(1.0f / 30.0f)
        {
        }

        /// <summary>
        /// Creates a new AnimationSystem instance with the specified frame rate.
        /// </summary>
        /// <param name="frameRate">Animation frame rate specified as seconds
        /// each animation frame is displayed before changing to the next.</param>
        public AnimationSystem(float frameRate)
            : base(frameRate, typeof(AnimationComponent), typeof(ImageComponent))
        {
            _frameRate = frameRate;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Initializes the animation system.
        /// </summary>
        public override void Initialize()
        {
            _animationMapper = new ComponentMapper<AnimationComponent>(World);
            _imageMapper = new ComponentMapper<ImageComponent>(World);

            base.Initialize();
        }

        /// <summary>
        /// Updates the animation for the provided <see cref="Entity"/> instance.
        /// </summary>
        /// <param name="entity"><see cref="Entity"/> instance to render.</param>
        public override void Process(Entity entity)
        {
            if (!entity.IsActive)
            {
                return;
            }

            AnimationComponent animation = _animationMapper.Get(entity);

            if ((animation != null) && (animation.IsPlaying))
            {
                if (animation.ElapsedTimeSinceLastFrameChange >= animation.Frames[animation.FrameIndex].FrameLength)
                {
                    animation.ElapsedTimeSinceLastFrameChange = 0.0f;
                    if (animation.IsReversed)
                    {
                        if (animation.FrameIndex == 0)
                        {
                            animation.IsReversed = false;
                            ++animation.FrameIndex;
                        }
                        else
                        {
                            --animation.FrameIndex;
                        }
                    }
                    else
                    {
                        if (animation.FrameIndex == animation.Frames.Length - 1)
                        {
                            switch (animation.PlayMode)
                            {
                                case AnimationPlayMode.PlayOnce:
                                    animation.IsPlaying = false;
                                    break;
                                case AnimationPlayMode.PlayOnceReset:
                                    animation.IsPlaying = false;
                                    animation.FrameIndex = 0;
                                    break;
                                case AnimationPlayMode.Loop:
                                    animation.FrameIndex = 0;
                                    break;
                                case AnimationPlayMode.LoopReverse:
                                    --animation.FrameIndex;
                                    animation.IsReversed = true;
                                    break;
                            }
                        }
                        else
                        {
                            ++animation.FrameIndex;
                        }
                    }

                    ImageComponent image = _imageMapper.Get(entity);
                    if (image != null)
                    {
                        FrameInfo frame = animation.Frames[animation.FrameIndex];
                        image.Source = frame.Source;
                        image.Scale = frame.Scale;
                        image.Rotation = frame.Rotation;
                        image.Tint = frame.Tint;
                        image.Effects = frame.Effects;
                        image.Origin = new Microsoft.Xna.Framework.Vector2(frame.Source.Width/2, frame.Source.Height/2);
                    }
                }
                else
                {
                    animation.ElapsedTimeSinceLastFrameChange += ElapsedTimeSinceLastProcess;
                }
            }
        }

        #endregion
    }
}

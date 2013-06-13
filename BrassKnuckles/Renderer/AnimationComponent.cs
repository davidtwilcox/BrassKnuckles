using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrassKnuckles.EntityFramework;

namespace BrassKnuckles.Renderer
{
    /// <summary>
    /// Animation playback behavior. 
    /// </summary>
    public enum AnimationPlayMode
    {
        /// <summary>
        /// Animation will continuously loop, returning to the first frame
        /// after reaching the end.
        /// </summary>
        Loop = 0,
        /// <summary>
        /// Animation will continuously loop, but running in reverse
        /// after reaching the end, then playing forward again upon reaching the
        /// first frame.
        /// </summary>
        LoopReverse,
        /// <summary>
        /// Animation will play once and remain on the last frame.
        /// </summary>
        PlayOnce,
        /// <summary>
        /// Animation will play once and return to the first frame.
        /// </summary>
        PlayOnceReset
    }

    /// <summary>
    /// Handles animation data for an entity.
    /// </summary>
    public class AnimationComponent : IComponent
    {
        #region Properties

        /// <summary>
        /// Gets or sets an array of <see cref="FrameInfo"/> instances describing
        /// each frame of animation.
        /// </summary>
        public FrameInfo[] Frames { get; set; }

        /// <summary>
        /// Gets or sets the index of the current frame.
        /// </summary>
        public int FrameIndex { get; set; }

        /// <summary>
        /// Gets or sets the animation play mode.
        /// </summary>
        public AnimationPlayMode PlayMode { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating the animation is playing.
        /// </summary>
        public bool IsPlaying { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating the animation is playing in reverse.
        /// </summary>
        public bool IsReversed { get; set; }

        /// <summary>
        /// Gets or sets the amount of time in seconds that has elapsed since the last frame change.
        /// </summary>
        public float ElapsedTimeSinceLastFrameChange { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new AnimationComponent instance.
        /// </summary>
        public AnimationComponent()
        {
            FrameIndex = 0;
            PlayMode = AnimationPlayMode.Loop;
            IsPlaying = true;
            IsReversed = false;
            ElapsedTimeSinceLastFrameChange = 0.0f;
        }

        #endregion
    }
}

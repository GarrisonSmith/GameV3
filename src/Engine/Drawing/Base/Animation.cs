using Microsoft.Xna.Framework;
using System;

namespace Engine.Drawing.Base
{
	/// <summary>
	/// Represents an animation.
	/// </summary>
	public class Animation
	{
		private bool isPlaying;

		/// <summary>
		/// Gets or sets the guid.
		/// </summary>
		public Guid Guid { get; private set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not this animation is playing.
		/// </summary>
		public bool IsPlaying 
		{
			get => isPlaying;
			set
			{
				if (value == this.isPlaying)
				{
					return;
				}

				if (!value)
				{
					this.ResetAnimation();
				}

				this.isPlaying = value;
			}
		}

		/// <summary>
		/// Gets or sets the current frame index.
		/// </summary>
		public int CurrentFrameIndex { get; set; }

		/// <summary>
		/// Gets or sets the current frame duration in milliseconds.
		/// </summary>
		public int FrameDuration { get; set; }

		/// <summary>
		/// Gets or sets the frame min duration in milliseconds.
		/// </summary>
		public int? FrameMinDuration { get; set; }

		/// <summary>
		/// Gets or sets the frame max duration in milliseconds.
		/// </summary>
		public int? FrameMaxDuration { get; set; }

		/// <summary>
		/// Gets or sets current frame start time in milliseconds.
		/// </summary>
		public double? FrameStartTime { get; set; }

		/// <summary>
		/// Gets or sets the current frame.
		/// </summary>
		public DrawData CurrentFrame { get => this.Frames[this.CurrentFrameIndex]; }

		/// <summary>
		/// Gets or sets the frames.
		/// </summary>
		public DrawData[] Frames { get; set; }

		/// <summary>
		/// Initializes a new instance of the Animation class.
		/// </summary>
		/// <param name="currentFrameIndex">The current frame index.</param>
		/// <param name="frameDuration">The frame duration.</param>
		/// <param name="frames">The frames.</param>
		public Animation(int currentFrameIndex, int frameDuration, DrawData[] frames)
		{
			this.isPlaying = true;
			this.CurrentFrameIndex = currentFrameIndex;
			this.FrameDuration = frameDuration;
			this.Frames = frames;
			this.Guid = Guid.NewGuid();
			this.FrameDuration = frameDuration;
			this.FrameStartTime = null;
			Managers.DrawManager.Animations.Add(this.Guid, this);
		}

		/// <summary>
		/// Initializes a new instance of the Animation class.
		/// </summary>
		/// <param name="isPlaying">Gets or sets a value indicating whether or not this animation is playing.</param>
		/// <param name="currentFrameIndex">The current frame index.</param>
		/// <param name="frameDuration">The frame duration.</param>
		/// <param name="frames">The frames.</param>
		public Animation(bool isPlaying, int currentFrameIndex, int frameDuration, DrawData[] frames)
		{
			this.isPlaying = isPlaying;
			this.CurrentFrameIndex = currentFrameIndex;
			this.FrameDuration = frameDuration;
			this.Frames = frames;
			this.Guid = Guid.NewGuid();
			this.FrameDuration = frameDuration;
			this.FrameStartTime = null;
			Managers.DrawManager.Animations.Add(this.Guid, this);
		}

		/// <summary>
		/// Initializes a new instance of the Animation class.
		/// </summary>
		/// <param name="currentFrameIndex">The current frame index.</param>
		/// <param name="frameMinDuration">The frame min duration.</param>
		/// <param name="frameMaxDuration">The frame max duration.</param>
		/// <param name="frames">The frames.</param>
		public Animation(int currentFrameIndex, int frameMinDuration, int frameMaxDuration, DrawData[] frames)
		{
			this.isPlaying = true;
			this.CurrentFrameIndex = currentFrameIndex;
			this.FrameMinDuration = frameMinDuration;
			this.FrameMaxDuration = frameMaxDuration;
			this.Frames = frames;
			this.Guid = Guid.NewGuid();
			this.FrameStartTime = null;
			this.FrameDuration = this.GetNextFrameDuration();
			Managers.DrawManager.Animations.Add(this.Guid, this);
		}

		/// <summary>
		/// Gets the next frame duration.
		/// </summary>
		/// <returns>The frame duration.</returns>
		private int GetNextFrameDuration()
		{
			if (this.FrameMinDuration.HasValue && this.FrameMaxDuration.HasValue)
			{
				return Managers.RandomManager.GetRandomInt(this.FrameMinDuration.Value, this.FrameMaxDuration.Value);
			}

			return this.FrameDuration;
		}

		/// <summary>
		/// Updates the current frame.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		public void UpdateFrame(GameTime gameTime)
		{
			if (this.FrameStartTime == null || !this.IsPlaying)
			{
				this.FrameStartTime = gameTime.TotalGameTime.TotalMilliseconds;
				return;
			}

			if (this.FrameStartTime + this.FrameDuration < gameTime.TotalGameTime.TotalMilliseconds)
			{
				this.FrameDuration = this.GetNextFrameDuration();
				if (this.CurrentFrameIndex < this.Frames.Length - 1)
				{
					this.CurrentFrameIndex++;
				}
				else
				{
					this.CurrentFrameIndex = 0;
				}

				this.FrameStartTime = gameTime.TotalGameTime.TotalMilliseconds;
			}
		}

		/// <summary>
		/// Creates a clone of the animation.
		/// </summary>
		/// <returns>A clone of the animation.</returns>
		public Animation CloneAnimation()
		{
			if (this.FrameMinDuration.HasValue && this.FrameMaxDuration.HasValue)
			{
				return new Animation(this.CurrentFrameIndex, this.FrameMinDuration.Value, this.FrameMaxDuration.Value, this.Frames);
			}

			return new Animation(this.CurrentFrameIndex, this.FrameDuration, this.Frames);
		}

		/// <summary>
		/// Resets the animation.
		/// </summary>
		/// <param name="frameIndex">The frame index to reset to.</param>
		private void ResetAnimation(int frameIndex = 0)
		{
			this.FrameStartTime = null;
			this.CurrentFrameIndex = frameIndex;
		}
	}
}

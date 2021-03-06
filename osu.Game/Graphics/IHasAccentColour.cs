﻿// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using OpenTK.Graphics;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Transforms;

namespace osu.Game.Graphics
{
    /// <summary>
    /// A type of drawable that has an accent colour.
    /// The accent colour is used to colorize various objects inside a drawable
    /// without colorizing the drawable itself.
    /// </summary>
    public interface IHasAccentColour : IDrawable
    {
        Color4 AccentColour { get; set; }
    }

    public static class AccentedColourExtensions
    {
        /// <summary>
        /// Tweens the accent colour of a drawable to another colour.
        /// </summary>
        /// <param name="accentedDrawable">The drawable to apply the accent colour to.</param>
        /// <param name="newColour">The new accent colour.</param>
        /// <param name="duration">The tween duration.</param>
        /// <param name="easing">The tween easing.</param>
        public static TransformSequence<T> FadeAccent<T>(this T accentedDrawable, Color4 newColour, double duration = 0, Easing easing = Easing.None)
            where T : IHasAccentColour
            => accentedDrawable.TransformTo(nameof(accentedDrawable.AccentColour), newColour, duration, easing);
    }
}

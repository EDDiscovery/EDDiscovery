/*
 * Copyright © 2016 - 2017 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 * 
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

public static class ObjectExtensionsColours
{
    /// <summary>
    /// Determine if two colors are both fully transparent or are otherwise equal in value, ignoring
    /// any stupid naming comparisons or RGB comparisons when both are fully transparent.
    /// </summary>
    /// <param name="other">The color to compare to.</param>
    /// <returns><c>true</c> if the colors are both fully transparent or are equal in value; <c>false</c> otherwise.</returns>
    public static bool IsEqual(this Color c, Color other)
    {
        return ((c.A == 0 && other.A == 0) || c.ToArgb() == other.ToArgb());
    }

    /// <summary>
    /// Determine whether or not a <see cref="Color"/> is completely transparent.
    /// </summary>
    /// <returns><c>true</c> if the <see cref="Color"/> is completely transparent; <c>false</c> otherwise.</returns>
    public static bool IsFullyTransparent(this Color c)
    {
        return (c.A == 0);
    }

    /// <summary>
    /// Determine whether or not a <see cref="Color"/> contains an alpha component.
    /// </summary>
    /// <returns><c>true</c> if the color is at least partially transparent; <c>false</c> if it is fully opaque.</returns>
    public static bool IsSemiTransparent(this Color c)
    {
        return (c.A < 255);
    }

    /// <summary>
    /// Average two <see cref="Color"/> objects together, complete with alpha, with <paramref name="ratio"/>
    /// determining the ratio of the original color to the <paramref name="other"/> <see cref="Color"/>.
    /// </summary>
    /// <param name="other">The <see cref="Color"/> to average with this one.</param>
    /// <param name="ratio">(0.0f-1.0f) The strength of the original <see cref="Color"/> in the resulting value. 1.0f means
    /// all original, while 0.0f means all <paramref name="other"/>, with 0.5f being an equal mix between the two.</param>
    /// <returns>The average of the two <see cref="Color"/>s given the ratio specified by <paramref name="ratio"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If <paramref name="ratio"/> is NaN.</exception>
    public static Color Average(this Color c, Color other, float ratio = 0.5f)
    {
        if (float.IsNaN(ratio))
            throw new ArgumentOutOfRangeException("ratio", "must be between 0.0f and 1.0f.");

        float left = Math.Min(Math.Max(ratio, 0.0f), 1.0f);
        float right = 1.0f - left;
        return Color.FromArgb(
            (byte)Math.Max(Math.Min(Math.Round((float)c.A * left + (float)other.A * right), 255), 0),
            (byte)Math.Max(Math.Min(Math.Round((float)c.R * left + (float)other.R * right), 255), 0),
            (byte)Math.Max(Math.Min(Math.Round((float)c.G * left + (float)other.G * right), 255), 0),
            (byte)Math.Max(Math.Min(Math.Round((float)c.B * left + (float)other.B * right), 255), 0));
    }

    /// <summary>
    /// Multiply a color. That is, brighten or darken all three channels by the same <paramref name="amount"/>, without modification to the alpha channel.
    /// </summary>
    /// <param name="amount"><c>1.0</c> will yield an identical color, <c>0.0</c> will yield black, <c>0.5</c> will
    /// be half as bright, <c>2.0</c> will be twice as bright. Negative values will be treated as positive.</param>
    /// <returns>The multiplied color with the new brightness. If <paramref name="amount"/> is
    /// <c>float.NaN</c>, the value returned will be unchanged.</returns>
    /// <remarks>Any components with a zero value (such as Black) will always return a zero value component.</remarks>
    public static Color Multiply(this Color c, float amount = 1.0f)
    {
        if (float.IsNaN(amount))
            return c;

        float val = Math.Abs(amount);
        return Color.FromArgb(c.A,
            (byte)Math.Max(Math.Min(Math.Round((float)c.R * val), 255), 0),
            (byte)Math.Max(Math.Min(Math.Round((float)c.G * val), 255), 0),
            (byte)Math.Max(Math.Min(Math.Round((float)c.B * val), 255), 0));
    }

    public static Color MultiplyBrightness(this Color c, float amount = 1.0f)        // if too dark, multiple white.
    {
        if (float.IsNaN(amount))
            return c;

        float val = Math.Abs(amount);

        float brightness = c.GetBrightness();
        if (brightness < 0.1)
            c = Color.White;
        return Color.FromArgb(c.A,
            (byte)Math.Max(Math.Min(Math.Round((float)c.R * val), 255), 0),
            (byte)Math.Max(Math.Min(Math.Round((float)c.G * val), 255), 0),
            (byte)Math.Max(Math.Min(Math.Round((float)c.B * val), 255), 0));
    }

    public static Color InvertBrightness(this Color c, float amount = 0.5f)        // multiply up/down by this amount, dep. on brightness
    {
        if (float.IsNaN(amount))
            return c;

        float val = Math.Abs(amount);

        float brightness = c.GetBrightness();
        if (brightness > 0.5)
            val = 1.0F / val;

        return Color.FromArgb(c.A,
            (byte)Math.Max(Math.Min(Math.Round((float)c.R * val), 255), 0),
            (byte)Math.Max(Math.Min(Math.Round((float)c.G * val), 255), 0),
            (byte)Math.Max(Math.Min(Math.Round((float)c.B * val), 255), 0));
    }
}



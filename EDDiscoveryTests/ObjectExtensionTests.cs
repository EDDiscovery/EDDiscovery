﻿/*
 * Copyright © 2017 EDDiscovery development team
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
using NFluent;
using NUnit.Framework;
using System;
using System.Drawing;

namespace EDDiscoveryTests
{
    [TestFixture(TestOf = typeof(ObjectExtensionsColours))]
    public class ObjectExtensionsTests
    {
        [Test]
        public void ObjectExtensions_Color_IsEqual()
        {
            //some obvious checks
            Check.That(Color.Black.IsEqual(Color.Black)).IsTrue();
            Check.That(Color.Black.IsEqual(Color.White)).IsFalse();
            Check.That(Color.Gray.IsEqual(Color.FromArgb(128, 128, 128))).IsTrue();
            Check.That(Color.Red.IsEqual(Color.Red)).IsTrue();
            Check.That(Color.Red.IsEqual(Color.Green)).IsFalse();
            Check.That(Color.Red.IsEqual(Color.Blue)).IsFalse();
            Check.That(Color.Green.IsEqual(Color.Red)).IsFalse();
            Check.That(Color.Green.IsEqual(Color.Green)).IsTrue();
            Check.That(Color.Green.IsEqual(Color.Blue)).IsFalse();
            Check.That(Color.Blue.IsEqual(Color.Red)).IsFalse();
            Check.That(Color.Blue.IsEqual(Color.Green)).IsFalse();
            Check.That(Color.Blue.IsEqual(Color.Blue)).IsTrue();

            //fully transparent checks
            Check.That(Color.Transparent.IsEqual(Color.FromArgb(0, 64, 128, 196))).IsTrue();
            Check.That(Color.FromArgb(0, 64, 128, 196).IsEqual(Color.Transparent)).IsTrue();

            //sensitivity checks
            Check.That(Color.FromArgb(0, Color.Gray).IsEqual(Color.FromArgb(1, Color.Gray))).IsFalse();
            Check.That(Color.FromArgb(255, Color.Gray).IsEqual(Color.FromArgb(254, Color.Gray))).IsFalse();
            Check.That(Color.FromArgb(128, 128, 128, 128).IsEqual(Color.FromArgb(128, 128, 127, 128))).IsFalse();
        }

        [Test]
        public void ObjectExtensions_Color_IsTransparent()
        {
            Check.That(Color.Transparent.IsFullyTransparent()).IsTrue();
            Check.That(Color.Transparent.IsSemiTransparent()).IsTrue();
            Check.That(Color.White.IsFullyTransparent()).IsFalse();
            Check.That(Color.Black.IsFullyTransparent()).IsFalse();
            Check.That(Color.Red.IsFullyTransparent()).IsFalse();
            Check.That(Color.White.IsSemiTransparent()).IsFalse();
            Check.That(Color.Black.IsSemiTransparent()).IsFalse();
            Check.That(Color.Red.IsSemiTransparent()).IsFalse();
            Check.That(Color.FromArgb(255, 0, 0, 0).IsFullyTransparent()).IsFalse();
            Check.That(Color.FromArgb(255, 0, 0, 0).IsSemiTransparent()).IsFalse();
            Check.That(Color.FromArgb(128, 0, 0, 0).IsFullyTransparent()).IsFalse();
            Check.That(Color.FromArgb(128, 0, 0, 0).IsSemiTransparent()).IsTrue();
            Check.That(Color.FromArgb(0, 0, 0, 0).IsFullyTransparent()).IsTrue();
            Check.That(Color.FromArgb(0, 0, 0, 0).IsSemiTransparent()).IsTrue();
        }

        [Test]
        public void ObjectExtensions_Color_Average()
        {
            // Note that these tests explicitly do not check using the IsEqual comparer.

            Check.That(Color.Black.Average(Color.Black).ToArgb()).Equals(Color.Black.ToArgb());
            Check.That(Color.Black.Average(Color.Gray).ToArgb()).Equals(Color.FromArgb(64, 64, 64).ToArgb());
            Check.That(Color.Black.Average(Color.White).ToArgb()).Equals(Color.Gray.ToArgb());
            Check.That(Color.Gray.Average(Color.White).ToArgb()).Equals(Color.FromArgb(192, 192, 192).ToArgb());

            Check.That(Color.Black.Average(Color.White, float.NegativeInfinity).ToArgb()).Equals(Color.White.ToArgb());
            Check.That(Color.Black.Average(Color.White, -20.0f).ToArgb()).Equals(Color.White.ToArgb());
            Check.That(Color.Black.Average(Color.White, -1.0f).ToArgb()).Equals(Color.White.ToArgb());
            Check.That(Color.Black.Average(Color.White, 0.0f).ToArgb()).Equals(Color.White.ToArgb());
            Check.That(Color.Black.Average(Color.White, 1.0f).ToArgb()).Equals(Color.Black.ToArgb());
            Check.That(Color.Black.Average(Color.White, 20.0f).ToArgb()).Equals(Color.Black.ToArgb());
            Check.That(Color.Black.Average(Color.White, float.PositiveInfinity).ToArgb()).Equals(Color.Black.ToArgb());

            Check.That(Color.Black.Average(Color.Yellow).ToArgb()).Equals(Color.Olive.ToArgb());
            Check.That(Color.Red.Average(Color.Blue).ToArgb()).Equals(Color.Purple.ToArgb());
            Check.That(Color.Lime.Average(Color.Blue).ToArgb()).Equals(Color.Teal.ToArgb());

            Check.ThatCode(() => { Color.Black.Average(Color.Black, float.NaN); }).Throws<ArgumentOutOfRangeException>();
        }

        [Test]
        public void ObjectExtensions_Color_Multiply()
        {
            // Note that these tests explicitly do not check using the IsEqual comparer.

            // Edge cases should return the original color.
            Check.That(Color.White.Multiply(float.NaN).ToArgb()).Equals(Color.White.ToArgb());
            Check.That(Color.Black.Multiply(float.NaN).ToArgb()).Equals(Color.Black.ToArgb());
            Check.That(Color.Red.Multiply(float.NaN).ToArgb()).Equals(Color.Red.ToArgb());
            Check.That(Color.Blue.Multiply(float.NaN).ToArgb()).Equals(Color.Blue.ToArgb());

            //00 00 00 - Black is like zero: you're only going to get zero back.
            Check.That(Color.Black.Multiply(float.NegativeInfinity).ToArgb()).Equals(Color.Black.ToArgb());
            Check.That(Color.Black.Multiply(float.MinValue).ToArgb()).Equals(Color.Black.ToArgb());
            Check.That(Color.Black.Multiply(0.0f).ToArgb()).Equals(Color.Black.ToArgb());
            Check.That(Color.Black.Multiply(1.0f).ToArgb()).Equals(Color.Black.ToArgb());
            Check.That(Color.Black.Multiply(float.MaxValue).ToArgb()).Equals(Color.Black.ToArgb());
            Check.That(Color.Black.Multiply(float.PositiveInfinity).ToArgb()).Equals(Color.Black.ToArgb());
            //00 00 FF
            Check.That(Color.Blue.Multiply(float.NegativeInfinity).ToArgb()).Equals(Color.Blue.ToArgb());
            Check.That(Color.Blue.Multiply(0.25f).ToArgb()).Equals(Color.FromArgb(255, 0, 0, 64).ToArgb());
            Check.That(Color.Blue.Multiply(0.5f).ToArgb()).Equals(Color.FromArgb(255, 0, 0, 128).ToArgb());
            Check.That(Color.Blue.Multiply(1.0f).ToArgb()).Equals(Color.Blue.ToArgb());
            Check.That(Color.Blue.Multiply(2.0f).ToArgb()).Equals(Color.Blue.ToArgb());
            Check.That(Color.Blue.Multiply(float.PositiveInfinity).ToArgb()).Equals(Color.Blue.ToArgb());
            //00 FF 00
            Check.That(Color.Lime.Multiply(0.25f).ToArgb()).Equals(Color.FromArgb(255, 0, 64, 0).ToArgb());
            Check.That(Color.Lime.Multiply(0.5f).ToArgb()).Equals(Color.FromArgb(255, 0, 128, 0).ToArgb());
            Check.That(Color.Lime.Multiply(-1.0f).ToArgb()).Equals(Color.Lime.ToArgb());
            Check.That(Color.Lime.Multiply(2.0f).ToArgb()).Equals(Color.Lime.ToArgb());
            //FF 00 00
            Check.That(Color.Red.Multiply(0.25f).ToArgb()).Equals(Color.FromArgb(255, 64, 0, 0).ToArgb());
            Check.That(Color.Red.Multiply(0.5f).ToArgb()).Equals(Color.FromArgb(255, 128, 0, 0).ToArgb());
            Check.That(Color.Red.Multiply(1.0f).ToArgb()).Equals(Color.Red.ToArgb());
            Check.That(Color.Red.Multiply(2.0f).ToArgb()).Equals(Color.Red.ToArgb());
            //00 FF FF
            Check.That(Color.Aqua.Multiply(0.25f).ToArgb()).Equals(Color.FromArgb(255, 0, 64, 64).ToArgb());
            Check.That(Color.Aqua.Multiply(0.5f).ToArgb()).Equals(Color.FromArgb(255, 0, 128, 128).ToArgb());
            Check.That(Color.Aqua.Multiply(1.0f).ToArgb()).Equals(Color.Aqua.ToArgb());
            Check.That(Color.Aqua.Multiply(-2.0f).ToArgb()).Equals(Color.Aqua.ToArgb());
            //FF 00 FF
            Check.That(Color.Magenta.Multiply(0.25f).ToArgb()).Equals(Color.FromArgb(255, 64, 0, 64).ToArgb());
            Check.That(Color.Magenta.Multiply(0.5f).ToArgb()).Equals(Color.FromArgb(255, 128, 0, 128).ToArgb());
            Check.That(Color.Magenta.Multiply(1.0f).ToArgb()).Equals(Color.Magenta.ToArgb());
            Check.That(Color.Magenta.Multiply(2.0f).ToArgb()).Equals(Color.Magenta.ToArgb());
            //FF FF 00
            Check.That(Color.Yellow.Multiply(0.25f).ToArgb()).Equals(Color.FromArgb(255, 64, 64, 0).ToArgb());
            Check.That(Color.Yellow.Multiply(0.5f).ToArgb()).Equals(Color.FromArgb(255, 128, 128, 0).ToArgb());
            Check.That(Color.Yellow.Multiply(1.0f).ToArgb()).Equals(Color.Yellow.ToArgb());
            Check.That(Color.Yellow.Multiply(2.0f).ToArgb()).Equals(Color.Yellow.ToArgb());
            //FF FF FF
            Check.That(Color.White.Multiply(-0.0f).ToArgb()).Equals(Color.Black.ToArgb());
            Check.That(Color.White.Multiply(0.5f).ToArgb()).Equals(Color.Gray.ToArgb());
            Check.That(Color.White.Multiply(1.0f).ToArgb()).Equals(Color.White.ToArgb());
            Check.That(Color.White.Multiply(2.0f).ToArgb()).Equals(Color.White.ToArgb());
            //80 80 80
            Check.That(Color.Gray.Multiply(float.NegativeInfinity).ToArgb()).Equals(Color.White.ToArgb());
            Check.That(Color.Gray.Multiply(float.PositiveInfinity).ToArgb()).Equals(Color.White.ToArgb());
            Check.That(Color.Gray.Multiply(0.0f).ToArgb()).Equals(Color.Black.ToArgb());
            Check.That(Color.Gray.Multiply(0.25f).ToArgb()).Equals(Color.FromArgb(255, 32, 32, 32).ToArgb());
            Check.That(Color.Gray.Multiply(-0.5f).ToArgb()).Equals(Color.FromArgb(255, 64, 64, 64).ToArgb());
            Check.That(Color.Gray.Multiply(1.0f).ToArgb()).Equals(Color.Gray.ToArgb());
            Check.That(Color.Gray.Multiply(1.5f).ToArgb()).Equals(Color.FromArgb(255, 192, 192, 192).ToArgb());
            Check.That(Color.Gray.Multiply(2.0f).ToArgb()).Equals(Color.White.ToArgb());
            //80 00 00
            Check.That(Color.Maroon.Multiply(0.0f).ToArgb()).Equals(Color.Black.ToArgb());
            Check.That(Color.Maroon.Multiply(0.25f).ToArgb()).Equals(Color.FromArgb(255, 32, 0, 0).ToArgb());
            Check.That(Color.Maroon.Multiply(0.5f).ToArgb()).Equals(Color.FromArgb(255, 64, 0, 0).ToArgb());
            Check.That(Color.Maroon.Multiply(1.0f).ToArgb()).Equals(Color.Maroon.ToArgb());
            Check.That(Color.Maroon.Multiply(-1.5f).ToArgb()).Equals(Color.FromArgb(255, 192, 0, 0).ToArgb());
            Check.That(Color.Maroon.Multiply(2.0f).ToArgb()).Equals(Color.Red.ToArgb());
        }
    }
}

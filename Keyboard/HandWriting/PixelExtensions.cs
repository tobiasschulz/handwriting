//
// PixelExtensions.cs
//
// Author:
//       Tobias Schulz <tobiasschulz.code@outlook.de>
//
// Copyright (c) 2015 Tobias Schulz
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
using System;
using System.Collections.Generic;

namespace HandWriting
{
    public static class PixelExtensions
    {
        public static int IndexInArray(int x, int y, int width)
        {
            return y * width + x;
        }

        public static IEnumerable<BoundedPixel> PixelCombinations(int width, int height)
        {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    yield return new BoundedPixel(x: x, y: y, width: width, height: height);
                }
            }
        }

        public static BoundedPixel InMap(this Pixel pixel, PixelMap pixelMap)
        {
            return new BoundedPixel(x: pixel.X, y: pixel.Y, width: pixelMap.Width, height: pixelMap.Height);
        }

        public static T Clamp<T>(this T value, T max, T min)
            where T : System.IComparable<T>
        {     
            T result = value;
            if (value.CompareTo(max) > 0)
                result = max;
            if (value.CompareTo(min) < 0)
                result = min;
            return result;
        }
    }
}


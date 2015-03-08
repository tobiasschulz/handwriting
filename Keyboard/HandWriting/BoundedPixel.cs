﻿//
// BoundedPixel.cs
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
    public struct BoundedPixel
    {
        public readonly int ParentWidth;
        public readonly int ParentHeight;
        public readonly int IndexInArray;
        public readonly bool IsInValidRange;
        public Pixel RawPixel;

        public int X { get { return RawPixel.X; } }

        public int Y { get { return RawPixel.Y; } }

        public BoundedPixel(int x, int y, int width, int height)
        {
            ParentWidth = width;
            ParentHeight = height;
            IndexInArray = PixelExtensions.IndexInArray(x: x, y: y, width: width);
            IsInValidRange = x > 0 && y > 0 && x < width && y < height;
            RawPixel = new Pixel(x, y);
        }

        public static implicit operator Pixel(BoundedPixel pixel)
        {
            return pixel.RawPixel;
        }

        public BoundedPixel ScaleTo(PixelMap otherMap)
        {
            int normalizedX = (int)Math.Floor((float)X / (float)ParentWidth * (float)otherMap.Width);
            int normalizedY = (int)Math.Floor((float)Y / (float)ParentHeight * (float)otherMap.Height);
            return new BoundedPixel(x: normalizedX, y: normalizedY, width: otherMap.Width, height: otherMap.Height);
        }

        public IEnumerable<BoundedPixel> Neighbors()
        {
            for (int dx = -1; dx <= 1; dx++) {
                for (int dy = -1; dy <= 1; dy++) {
                    int nx = X + dx;
                    int ny = Y + dy;
                    BoundedPixel neighbor = new BoundedPixel(x: nx, y: ny, width: ParentWidth, height: ParentHeight);
                    if (neighbor.IsInValidRange) {
                        yield return neighbor;
                    }
                }
            }
        }
    }
}


//
// PixelMap.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace HandWriting
{
    public class PixelMap
    {
        public int Width { get; private set; }

        public int Height { get; private set; }

        private BitArray pixels;

        public PixelMap(int width, int height)
        {
            Width = width;
            Height = height;
            pixels = new BitArray(width * height);
        }

        public Pixel Size { get { return new Pixel(Width, Height); } }

        public bool this [Pixel pixel] {
            get {
                return pixels[pixel.Y * Width + pixel.X];
            }
            set {
                pixels[pixel.Y * Width + pixel.X] = value;
            }
        }

        public void Add(Pixel pixel)
        {
            this[pixel] = true;
        }

        public void Remove(Pixel pixel)
        {
            this[pixel] = false;
        }

        public int CountPixels(bool value)
        {
            int count = 0;
            foreach (bool bit in pixels) {
                if (bit == value) {
                    count++;
                }
            }
            return count;
        }

        public bool IsEmpty {
            get {
                foreach (bool bit in pixels) {
                    if (bit) {
                        return false;
                    }
                }
                return true;
            }
        }

        public IEnumerable<Pixel> GetPixels(bool value)
        {
            for (int y = 0; y < Height; ++y) {
                for (int x = 0; x < Width; ++x) {
                    Pixel p = new Pixel(x, y);
                    bool b = this[p];
                    if (b == value) {
                        yield return p;
                    }
                }
            }
        }

        public PixelMap Trim()
        {
            if (IsEmpty) {
                return new PixelMap(0, 0);
            }

            int minX = Width;
            int minY = Height;
            int maxX = 0;
            int maxY = 0;

            Pixel[] currentPixels = GetPixels(value: true).ToArray();

            foreach (Pixel pixel in currentPixels) {
                if (pixel.X < minX)
                    minX = pixel.X;
                if (pixel.Y < minY)
                    minY = pixel.Y;
                if (pixel.X > maxX)
                    maxX = pixel.X;
                if (pixel.Y > maxY)
                    maxY = pixel.Y;
            }

            PixelMap trimmed = new PixelMap(maxX - minX + 1, maxY - minY + 1);

            foreach (Pixel pixel in currentPixels) {
                Log.Debug("trimmed.Size=" + trimmed.Size + ", add=" + new Pixel(pixel.X - minX, pixel.Y - minY));
                trimmed[new Pixel(pixel.X - minX, pixel.Y - minY)] = true;
            }

            return trimmed;
        }
    }
}


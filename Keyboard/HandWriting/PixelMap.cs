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
        public static Pixel NORMALIZED_SIZE = new Pixel(10, 10);

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
                return Get(x: pixel.X, y: pixel.Y);
            }
            set {
                Set(x: pixel.X, y: pixel.Y, value: value);
            }
        }

        public bool Get(int x, int y)
        {
            return pixels[PixelExtensions.IndexInArray(x: x, y: y, width: Width)];
        }

        public void Set(int x, int y, bool value)
        {
            pixels[PixelExtensions.IndexInArray(x: x, y: y, width: Width)] = value;
        }

        public void Add(Pixel pixel)
        {
            this[pixel] = true;
        }

        public void Add(IEnumerable<Pixel> pixels)
        {
            foreach (Pixel pixel in pixels) {
                this[pixel] = true;
            }
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

        public IEnumerable<BoundedPixel> GetPixels(bool value)
        {
            for (int y = 0; y < Height; ++y) {
                for (int x = 0; x < Width; ++x) {
                    BoundedPixel p = new BoundedPixel(x, y, width: Width, height: Height);
                    bool b = this[p];
                    if (b == value) {
                        yield return p;
                    }
                }
            }
        }

        public PixelMap Trim(bool test = false)
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

            double aspectRatio = (double)(maxX - minX) / (double)(maxY - minY);

            if (maxX - minX < 0.33 * Width) {
                minX = (int)(minX - 0.20 * Width).Clamp(min: 0, max: Width);
                maxX = (int)(maxX + 0.20 * Width).Clamp(min: 0, max: Width);
            }
            if (maxY - minY < 0.33 * Height) {
                minY = (int)(minY - 0.20 * Height).Clamp(min: 0, max: Height);
                maxY = (int)(maxY + 0.20 * Height).Clamp(min: 0, max: Height);
            }

            if (test) {
                if (aspectRatio >= 0.5 && aspectRatio <= 2) {
                    int treshold = currentPixels.Length / 100 * 10;

                    removeClutter(description: "minX -> maxX",
                        dim1Start: ref minX, dim1End: maxX,
                        dim2Start: minY, dim2End: maxY, direction: +1,
                        treshold: treshold, get: Get);
                    removeClutter(description: "maxX -> minX",
                        dim1Start: ref maxX, dim1End: minX,
                        dim2Start: minY, dim2End: maxY, direction: -1,
                        treshold: treshold, get: Get);
                    removeClutter(description: "minY -> maxY",
                        dim1Start: ref minY, dim1End: maxY,
                        dim2Start: minX, dim2End: maxX, direction: +1,
                        treshold: treshold, get: (x, y) => Get(x: y, y: x));
                    removeClutter(description: "maxY -> minY",
                        dim1Start: ref maxY, dim1End: minY,
                        dim2Start: minX, dim2End: maxX, direction: -1,
                        treshold: treshold, get: (x, y) => Get(x: y, y: x));
                }
            }

            PixelMap trimmed = new PixelMap(width: maxX - minX + 1, height: maxY - minY + 1);

            foreach (Pixel pixel in currentPixels) {
                if (pixel.X >= minX && pixel.X <= maxX && pixel.Y >= minY && pixel.Y <= maxY) {
                    //Log.Debug("trimmed.Size=" + trimmed.Size + ", add=" + new Pixel(x: pixel.X - minX, y: pixel.Y - minY));
                    trimmed[new Pixel(x: pixel.X - minX, y: pixel.Y - minY)] = true;
                }
            }

            return trimmed;
        }

        private void removeClutter(string description, ref int dim1Start, int dim1End, int dim2Start, int dim2End, int direction, int treshold, Func<int, int, bool> get)
        {
            description = "removeClutter[" + description + "]";

            Debug.Assert(dim2Start < dim2End, description + ": dim2Start must always be smaller than dim2End (dim2Start=" + dim2Start + ",dim2End=" + dim2End + ")");

            Func<double,double,bool> isSmaller = (a, b) => direction > 0 ? (a < b) : (a > b);
            for (int x = dim1Start, pcSum = 0; isSmaller(x, dim1End) && isSmaller(x - dim1Start, 0.15 * (dim1End - dim1Start)); x += direction) {
                int pc = 0;
                for (int y = dim2Start; y < dim2End; ++y) {
                    Log.Debug("x=" + x + ", y=" + y + ", dim1=" + dim1Start + "-" + dim1End + ", dim2=" + dim2Start + "-" + dim2End);
                    if (get(x, y) == true) {
                        pc++;
                    }
                }
                pcSum += pc;
                Log.Debug(description + ": threshold row: x=", x, ", pcSum=", pcSum, ", pc=", pc);
                if (pcSum > treshold || pc > Math.Min(5, (dim2End - dim2Start) * 0.05)) {
                    break;
                }
                dim1Start = x;
            }
        }

        public PixelMap Normalize()
        {
            if (Size == NORMALIZED_SIZE) {
                return this;
            }

            PixelMap normalized = new PixelMap(width: NORMALIZED_SIZE.X, height: NORMALIZED_SIZE.Y);

            if (IsEmpty) {
                return normalized;
            }

            BoundedPixel[] currentPixels = GetPixels(value: true).ToArray();

            int[] densities = new int[normalized.Width * normalized.Height];

            foreach (BoundedPixel pixel in currentPixels) {
                densities[pixel.ScaleTo(otherMap: normalized).IndexInArray] += 1;
            }

            int[] neighbors = new int[normalized.Width * normalized.Height];

            Pixel[] normalizedPixelCombinations = PixelExtensions.PixelCombinations(width: normalized.Width, height: normalized.Height).ToArray();
            foreach (BoundedPixel pixel in normalizedPixelCombinations) {
                for (int dx = -1; dx <= 1; dx++) {
                    for (int dy = -1; dy <= 1; dy++) {
                        if (!(dx == 0 && dy == 0)) {
                            BoundedPixel neighbor = new Pixel(x: pixel.X + dx, y: pixel.Y + dy).InMap(normalized);
                            //Log.Debug("pixel: ", pixel, ", neighbor: ", neighbor);
                            if (neighbor.IsInValidRange) {
                                if (densities[neighbor.IndexInArray] > 0) {
                                    neighbors[neighbor.IndexInArray]++;
                                }
                            }
                        }
                    }
                }
            }

            double averageDensity = densities.Where(d => d > 0).Average();

            foreach (BoundedPixel pix in normalizedPixelCombinations) {
                int density = densities[pix.IndexInArray];
                int neighborCount = neighbors[pix.IndexInArray];
                if (density >= averageDensity * 0.7 || (density >= averageDensity * 0.35 && neighborCount > 1)) {
                    normalized[new Pixel(x: pix.X, y: pix.Y)] = true;
                }
            }

            /*
            foreach (Pixel pixel in currentPixels) {
                //Log.Debug("trimmed.Size=" + trimmed.Size + ", add=" + new Pixel(pixel.X - minX, pixel.Y - minY));
                int normalizedX = (int)Math.Floor((float)pixel.X / (float)Width * (float)normalized.Width);
                int normalizedY = (int)Math.Floor((float)pixel.Y / (float)Height * (float)normalized.Height);
                normalized[new Pixel(x: normalizedX, y: normalizedY)] = true;
            }*/

            return normalized;
        }

    }
}


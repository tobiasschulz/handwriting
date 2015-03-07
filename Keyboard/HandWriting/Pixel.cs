//
// Pixel.cs
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
using System.Runtime.InteropServices;

namespace HandWriting
{
    [StructLayout(LayoutKind.Sequential)]
    public class Pixel
    {
        public static Pixel Zero { get { return new Pixel(0, 0); } }

        public readonly int X;
        public readonly int Y;

        public Pixel(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public static Pixel operator -(Pixel b, Pixel a)
        {
            return new Pixel(b.X - a.X, b.Y - a.Y);
        }

        public static Pixel operator +(Pixel b, Pixel a)
        {
            return new Pixel(b.X + a.X, b.Y + a.Y);
        }

        public static Pixel operator *(Pixel a, double d)
        {
            return new Pixel((int)Math.Round(a.X * d), (int)Math.Round(a.Y * d));
        }

        public static Pixel operator /(Pixel a, double d)
        {
            return new Pixel((int)Math.Round(a.X / d), (int)Math.Round(a.Y / d));
        }

        public Pixel Absolute {
            get {
                return new Pixel(Math.Abs(X), Math.Abs(Y));
            }
        }

        public double Length {
            get {
                double aSq = Math.Pow(X, 2);
                double bSq = Math.Pow(Y, 2);
                return Math.Sqrt(aSq + bSq);
            }
        }

        public override string ToString()
        {
            return string.Format("({0},{1})", X, Y);
        }

        public bool IsBetween(Pixel minBounds, Pixel maxBounds)
        {
            return X > minBounds.X && Y > minBounds.Y && X < maxBounds.X && Y < maxBounds.Y;
        }

        public bool Equals(Pixel other)
        {
            if (other == null) {
                return false;
            }
            if (object.ReferenceEquals(this, other)) {
                return true;
            }
            return X == other.X && Y == other.Y;
        }

        public static bool operator ==(Pixel a, Pixel b)
        {
            if (object.ReferenceEquals(a, b)) {
                return true;
            }
            if ((object)a == null || (object)b == null) {
                return false;
            }
            return a.Equals(other: b);
        }

        public static bool operator !=(Pixel a, Pixel b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj != null) {
                Pixel other = obj as Pixel;
                if (other != null) {
                    return Equals((Pixel)obj);
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Y * 97979797 + X;
        }
    }

}


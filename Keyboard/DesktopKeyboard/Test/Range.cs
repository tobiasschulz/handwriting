//
// Range.cs
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

namespace DesktopKeyboard
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Range
    {
        public static Range Zero { get { return new Range(0, 0); } }

        public readonly double Min;
        public readonly double Max;

        public Range(double min, double max)
        {
            this.Min = min;
            this.Max = max;
        }

        public override string ToString()
        {
            return string.Format("({0}...{1})", Min, Max);
        }

        public bool IsInRange(double value)
        {
            return value >= Min && value <= Max;
        }


        public static bool operator ==(Range a, Range b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Range a, Range b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (int)(Min * 97979797.0 + Max * 123);
        }
    }
}


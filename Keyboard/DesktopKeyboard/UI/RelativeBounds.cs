//
// RelativeBounds.cs
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
using System.Drawing;

namespace DesktopKeyboard
{
    public class RelativeBounds
    {
        public Point TopLeft { get { return new Point(x: left, y: top); } }

        public Point BottomRight { get { return new Point(x: reference.Size.Width + right, y: reference.Size.Height + bottom); } }

        public Size Size { get { return new Size(width: reference.Size.Width + right - left, height: reference.Size.Height + bottom - top); } }

        private int top, left, bottom, right;
        private readonly ISize reference;

        public RelativeBounds(ISize reference, int left, int top, int right, int bottom)
        {
            this.reference = reference;
            this.top = top;
            this.left = left;
            this.bottom = bottom;
            this.right = right;
        }
    }
}


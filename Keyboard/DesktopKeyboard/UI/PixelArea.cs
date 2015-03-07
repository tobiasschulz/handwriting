﻿//
// PixelArea.cs
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
using System.Windows.Forms;
using HandWriting;

namespace DesktopKeyboard
{
    public class PixelArea
    {
        protected RelativeBounds bounds;
        protected Form reference;

        public PixelMap Points { get; set; }

        public PixelArea(Form reference, RelativeBounds bounds)
        {
            this.reference = reference;
            this.bounds = bounds;
        }

        public void Load()
        {
            Points = new PixelMap(bounds.Size.Width, bounds.Size.Height);
        }

        public void OnPaint(Graphics g)
        {
            Pen pen = new Pen(Color.Black, 1);
            g.DrawLine(pen, new Point(bounds.TopLeft.X, bounds.TopLeft.Y), new Point(bounds.BottomRight.X, bounds.TopLeft.Y));
            g.DrawLine(pen, new Point(bounds.TopLeft.X, bounds.TopLeft.Y), new Point(bounds.TopLeft.X, bounds.BottomRight.Y));
            g.DrawLine(pen, new Point(bounds.BottomRight.X, bounds.BottomRight.Y), new Point(bounds.BottomRight.X, bounds.TopLeft.Y));
            g.DrawLine(pen, new Point(bounds.BottomRight.X, bounds.BottomRight.Y), new Point(bounds.TopLeft.X, bounds.BottomRight.Y));

            Brush black = Brushes.Black;

            PointF scale = new PointF((float)bounds.Size.Width / (float)Points.Width, (float)bounds.Size.Height / (float)Points.Height);

            foreach (Pixel point in Points.GetPixels(value: true)) {
                int x = (int)((float)point.X * scale.X) + bounds.TopLeft.X;
                int y = (int)((float)point.Y * scale.Y) + bounds.TopLeft.Y;
                g.FillRectangle(black, x, y, scale.X, scale.Y);
            }
        }
    }
}


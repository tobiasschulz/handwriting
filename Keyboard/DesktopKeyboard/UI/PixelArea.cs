//
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
    public class PixelArea : Control
    {
        protected RelativeBounds bounds;
        protected Form reference;
        protected Bitmap Backbuffer;

        public PixelMap Points { get; set; }

        public PixelArea(Form reference, RelativeBounds bounds)
        {
            this.reference = reference;
            this.bounds = bounds;
            Parent = reference;
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
        }

        public void Load()
        {
            Reset();
            Size = bounds.Size;
            Location = bounds.TopLeft;

            CreateBackBuffer();
            reference.ResizeEnd += (s, e) => CreateBackBuffer();
        }

        public void Reset()
        {
            Points = new PixelMap(bounds.Size.Width, bounds.Size.Height);
        }

        private uint previousChangeCounter;

        public virtual void OnUpdate()
        {
            if (Points.ChangeCounter != previousChangeCounter) {
                previousChangeCounter = Points.ChangeCounter;
                Draw();
            }
        }

        private void CreateBackBuffer()
        {
            if (Backbuffer != null)
                Backbuffer.Dispose();

            Backbuffer = new Bitmap(bounds.Size.Width + 2, bounds.Size.Height + 2);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Log.Debug("OnPaint:", this, ", Backbuffer: ", Backbuffer, ", Points.CountPixels: ", Points.CountPixels(true));

            base.OnPaint(e);

            if (Backbuffer != null) {
                Graphics g = e.Graphics;
                g.Clear(Color.Purple);
                g.DrawImageUnscaled(Backbuffer, Point.Empty);
            }

        }

        public void Draw()
        {
            Log.Debug("Draw:", this, ", Backbuffer: ", Backbuffer, ", Points.CountPixels: ", Points.CountPixels(true));
            if (Backbuffer != null) {

                using (var g = Graphics.FromImage(Backbuffer)) {

                    /*Pen pen = new Pen(Color.Black, 1);
                    g.DrawLine(pen, new Point(0, 0), new Point(bounds.Size.Width + 2, 0));
                    g.DrawLine(pen, new Point(0, 0), new Point(0, bounds.Size.Height + 2));
                    g.DrawLine(pen, new Point(bounds.Size.Width + 2, bounds.Size.Height + 2), new Point(bounds.Size.Width + 2, 0));
                    g.DrawLine(pen, new Point(bounds.Size.Width + 2, bounds.Size.Height + 2), new Point(0, bounds.Size.Height + 2));
*/

                    g.Clear(Color.NavajoWhite);

                    Brush black = Brushes.Black;

                    PointF scale = new PointF((float)Size.Width / (float)Points.Width, (float)Size.Height / (float)Points.Height);

                    foreach (Pixel point in Points.GetPixels(value: true)) {
                        int x = (int)((float)point.X * scale.X);
                        int y = (int)((float)point.Y * scale.Y);
                        g.FillRectangle(black, x, y, (int)Math.Ceiling(scale.X), (int)Math.Ceiling(scale.Y));
                    }
                }
            }
        }
    }
}


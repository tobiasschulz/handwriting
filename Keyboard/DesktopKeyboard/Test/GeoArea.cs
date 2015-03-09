//
// FormArea.cs
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
using System.Windows.Forms;
using System.Drawing;
using HandWriting;

namespace DesktopKeyboard
{
    public class GeoArea : Control
    {
        protected RelativeBounds bounds;
        protected Form reference;
        protected Bitmap Backbuffer;

        public GeoForms GeoForms { get; set; }

        public GeoArea(Form reference, RelativeBounds bounds)
        {
            this.reference = reference;
            this.bounds = bounds;
            Parent = reference;

            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);

            Application.AddMessageFilter(new MouseMessageFilter());
            //MouseMessageFilter.MouseMove += new MouseEventHandler(OnMouseMove);
            MouseMessageFilter.LeftButtonUp += new MouseEventHandler(OnLeftButtonUp);
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
            GeoForms = new GeoForms(bounds.Size);
        }

        private uint previousChangeCounter = 99;
        private Point previousMousePosition = Point.Empty;

        public void OnUpdate()
        {
            Point currentMousePosition = Control.MousePosition;
            if (previousMousePosition != currentMousePosition) {
                previousMousePosition = currentMousePosition;
                OnMouseMove(currentMousePosition);
                Invalidate();
            }

            if (GeoForms.ChangeCounter != previousChangeCounter) {
                previousChangeCounter = GeoForms.ChangeCounter;
                Draw();
            }
        }

        private Pixel previousPoint = Pixel.Zero;

        private void OnLeftButtonUp(object sender, MouseEventArgs e)
        {
            Log.Debug("OnLeftButtonUp");
            previousPoint = Pixel.Zero;
        }

        private void OnMouseMove(Point mousePosition)
        {
            // Left button is down.
            if ((Control.MouseButtons & MouseButtons.Left) != 0) {
                Pixel point = PointToClient(mousePosition).ToPixel();//- bounds.TopLeft.ToPixel();
                Pixel _previousPoint = previousPoint;

                if (point.IsBetween(Pixel.Zero, bounds.Size.ToPixel())) {
                    previousPoint = point;

                    // save the point
                    Log.Debug("bounds.TopLeft:" + bounds.TopLeft + " point:" + point);
                    GeoForms.AddPoint(point);
                    Draw();
                    Invalidate();
                }
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
            Log.Debug("Draw:", this, ", Backbuffer: ", Backbuffer, ", GeoForms.Count: ", GeoForms.Count);
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

                    foreach (GeoForm form in GeoForms.Forms) {
                    }
                    GeoForms.Draw(g);

                    /*
                    PointF scale = new PointF((float)Size.Width / (float)Points.Width, (float)Size.Height / (float)Points.Height);

                    foreach (Pixel point in Points.GetPixels(value: true)) {
                        int x = (int)((float)point.X * scale.X);
                        int y = (int)((float)point.Y * scale.Y);
                        g.FillRectangle(black, x, y, (int)Math.Ceiling(scale.X), (int)Math.Ceiling(scale.Y));
                    }*/
                }
            }
        }
    }
}


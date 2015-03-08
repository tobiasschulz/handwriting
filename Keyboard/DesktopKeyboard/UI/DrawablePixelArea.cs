//
// DrawablePixelArea.cs
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
    public class DrawablePixelArea : PixelArea
    {
        private Pixel previousPoint = Pixel.Zero;

        public DrawablePixelArea(Form reference, RelativeBounds bounds)
            : base(reference: reference, bounds: bounds)
        {
            Application.AddMessageFilter(new MouseMessageFilter());
            //MouseMessageFilter.MouseMove += new MouseEventHandler(OnMouseMove);
            MouseMessageFilter.LeftButtonUp += new MouseEventHandler(OnLeftButtonUp);
        }

        private Point previousMousePosition;

        public override void OnUpdate()
        {
            Point currentMousePosition = Control.MousePosition;
            if (previousMousePosition != currentMousePosition) {
                previousMousePosition = currentMousePosition;
                OnMouseMove(currentMousePosition);
                Invalidate();
            }

            base.OnUpdate();
        }

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
                Log.Debug("bounds.TopLeft:" + bounds.TopLeft + " point:" + point);
                Pixel _previousPoint = previousPoint;

                if (point.IsBetween(Pixel.Zero, Points.Size)) {
                    previousPoint = point;

                    if (_previousPoint != Pixel.Zero && (point - _previousPoint).Length < 500) {
                        Pixel diff = point - _previousPoint;
                        int steps = Math.Max(diff.Absolute.X, diff.Absolute.Y);
                        Log.Debug("steps: " + steps);
                        double dx = 0, dy = 0;
                        for (int step = 0; step <= steps + 1; step++) {
                            Pixel interPoint = new Pixel(_previousPoint.X + (int)Math.Round(dx), _previousPoint.Y + (int)Math.Round(dy));
                            if (interPoint.IsBetween(Pixel.Zero, Points.Size)) {
                                Points.Add(interPoint.InMap(Points).Neighbors());
                                Log.Debug("  interPoint: ", interPoint, ", _previousPoint: ", _previousPoint, ", point: ", point);
                            } else {
                                previousPoint = Pixel.Zero;
                                break;
                            }
                            dx += (double)diff.X / (double)steps;
                            dy += (double)diff.Y / (double)steps;
                        }
                        Points.Add(point.InMap(Points).Neighbors());
                        Log.Debug(point.ToString());
                    } else {
                        Points.Add(point.InMap(Points).Neighbors());
                        Log.Debug(point.ToString());
                    }

                    //reference.Refresh();
                } else {
                    previousPoint = Pixel.Zero;
                }
            }
        }

        class MouseMessageFilter : IMessageFilter
        {
            public static event MouseEventHandler MouseMove = delegate { };
            public static event MouseEventHandler LeftButtonUp = delegate { };
            public static event MouseEventHandler LeftButtonDown = delegate { };

            public bool PreFilterMessage(ref Message m)
            {
                if ((MouseMessages)m.Msg == MouseMessages.WM_MOUSEMOVE) {
                    Point mousePosition = Control.MousePosition;
                    MouseMove(null, new MouseEventArgs(
                        MouseButtons.None, 0, mousePosition.X, mousePosition.Y, 0));
                }
                if ((MouseMessages)m.Msg == MouseMessages.WM_LBUTTONUP) {
                    Point mousePosition = Control.MousePosition;
                    LeftButtonUp(null, new MouseEventArgs(
                        MouseButtons.None, 0, mousePosition.X, mousePosition.Y, 0));
                }
                if ((MouseMessages)m.Msg == MouseMessages.WM_LBUTTONDOWN) {
                    Point mousePosition = Control.MousePosition;
                    LeftButtonDown(null, new MouseEventArgs(
                        MouseButtons.None, 0, mousePosition.X, mousePosition.Y, 0));
                }
                return false;
            }

            private enum MouseMessages
            {
                WM_LBUTTONDOWN = 0x0201,
                WM_LBUTTONUP = 0x0202,
                WM_MOUSEMOVE = 0x0200,
                WM_MOUSEWHEEL = 0x020A,
                WM_RBUTTONDOWN = 0x0204,
                WM_RBUTTONUP = 0x0205
            }
        }
    }
}


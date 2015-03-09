//
// MouseMessageFilter.cs
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

namespace DesktopKeyboard
{
    public class MouseMessageFilter : IMessageFilter
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


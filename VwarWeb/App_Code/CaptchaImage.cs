//  Copyright 2011 U.S. Department of Defense

//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at

//      http://www.apache.org/licenses/LICENSE-2.0

//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.



using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace CaptchaImage
{
    /// <summary>
    /// Summary description for CaptchaImage.
    /// </summary>
    public class CaptchaImage
    {
        /// <summary>
        /// Public properties (all read-only). 
        /// </summary>
        public string Text
        {
            get { return this.text; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Bitmap Image
        {
            get { return this.image; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Width
        {
            get { return this.width; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Height
        {
            get { return this.height; }
        }

        /// <summary>
        /// Internal properties. 
        /// </summary>
        private string text;
        /// <summary>
        /// 
        /// </summary>
        private int width;
        /// <summary>
        /// 
        /// </summary>
        private int height;
        /// <summary>
        /// 
        /// </summary>
        private string familyName;
        /// <summary>
        /// 
        /// </summary>
        private Bitmap image;
        /// <summary>
        /// For generating random numbers. 
        /// </summary>
        private Random random = new Random();

        /// <summary>
        /// Initializes a new instance of the CaptchaImage class using the
        /// specified text, width and height.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public CaptchaImage(string s, int width, int height)
        {
            this.text = s;
            this.SetDimensions(width, height);
            this.GenerateImage();
        }

        /// <summary>
        /// Initializes a new instance of the CaptchaImage class using the
        /// specified text, width, height and font family.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="familyName"></param>
        public CaptchaImage(string s, int width, int height, string familyName)
        {
            this.text = s;
            this.SetDimensions(width, height);
            this.SetFamilyName(familyName);
            this.GenerateImage();
        }

        /// <summary>
        /// This member overrides Object.Finalize.
        /// </summary>
        ~CaptchaImage()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases all resources used by this object. 
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Dispose(true);
        }

        /// <summary>
        /// Custom Dispose method to clean up unmanaged resources. 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                // Dispose of the bitmap.
                this.image.Dispose();
        }

        /// <summary>
        /// Sets the image width and height. 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private void SetDimensions(int width, int height)
        {
            // Check the width and height.
            if (width <= 0)
                throw new ArgumentOutOfRangeException("width", width, "Argument out of range, must be greater than zero.");
            if (width <= 0)
                throw new ArgumentOutOfRangeException("height", height, "Argument out of range, must be greater than zero.");
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Sets the font used for the image text. 
        /// </summary>
        /// <param name="familyName"></param>
        private void SetFamilyName(string familyName)
        {
            // If the named font is not installed, default to a system font.
            try
            {
                Font font = new Font(this.familyName, 12F);
                this.familyName = familyName;
                font.Dispose();
            }
            catch (Exception ex)
            {
                this.familyName = System.Drawing.FontFamily.GenericSerif.Name;
            }
        }

        /// <summary>
        /// Creates the bitmap image. 
        /// </summary>
        private void GenerateImage()
        {
            // Create a new 32-bit bitmap image.
            Bitmap bitmap = new Bitmap(this.width, this.height, PixelFormat.Format32bppArgb);

            // Create a graphics object for drawing.
            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, this.width, this.height);

            // Fill in the background.
            HatchBrush hatchBrush = new HatchBrush(HatchStyle.SmallConfetti, Color.LightGray, Color.White);
            g.FillRectangle(hatchBrush, rect);

            // Set up the text font.
            SizeF size;
            float fontSize = rect.Height + 1;
            Font font;
            // Adjust the font size until the text fits within the image.
            do
            {
                fontSize--;
                font = new Font(this.familyName, fontSize, FontStyle.Bold);
                size = g.MeasureString(this.text, font);
            } while (size.Width > rect.Width);

            // Set up the text format.
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            // Create a path using the text and warp it randomly.
            GraphicsPath path = new GraphicsPath();
            path.AddString(this.text, font.FontFamily, (int)font.Style, font.Size, rect, format);
            float v = 4F;
            PointF[] points =
			{
				new PointF(this.random.Next(rect.Width) / v, this.random.Next(rect.Height) / v),
				new PointF(rect.Width - this.random.Next(rect.Width) / v, this.random.Next(rect.Height) / v),
				new PointF(this.random.Next(rect.Width) / v, rect.Height - this.random.Next(rect.Height) / v),
				new PointF(rect.Width - this.random.Next(rect.Width) / v, rect.Height - this.random.Next(rect.Height) / v)
			};
            Matrix matrix = new Matrix();
            matrix.Translate(0F, 0F);
            path.Warp(points, rect, matrix, WarpMode.Perspective, 0F);

            // Draw the text.
            hatchBrush = new HatchBrush(HatchStyle.LargeConfetti, Color.LightGray, Color.DarkGray);
            g.FillPath(hatchBrush, path);

            // Add some random noise.
            int m = Math.Max(rect.Width, rect.Height);
            for (int i = 0; i < (int)(rect.Width * rect.Height / 30F); i++)
            {
                int x = this.random.Next(rect.Width);
                int y = this.random.Next(rect.Height);
                int w = this.random.Next(m / 50);
                int h = this.random.Next(m / 50);
                g.FillEllipse(hatchBrush, x, y, w, h);
            }

            // Clean up.
            font.Dispose();
            hatchBrush.Dispose();
            g.Dispose();

            // Set the image.
            this.image = bitmap;
        }
    }
}

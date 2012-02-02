using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace OpenPharaoh
{
    public static class FileHelper
    {
        public static Bitmap LoadBitmapFrom555(string filename, int width, int height, int offset)
        {
            var b = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format16bppRgb555);
            var bd = b.LockBits(new Rectangle(0, 0, width, height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format16bppRgb555);

            int PixelSize = 2;

            using (var f = new FileStream(filename, FileMode.Open))
            {
                f.Seek(offset, SeekOrigin.Begin);

                unsafe
                {
                    for (int y = 0; y < height; y++)
                    {
                        byte* row = (byte*)bd.Scan0 + (y * bd.Stride);

                        for (int x = 0; x < width; x++)
                        {
                            var b1 = (byte)f.ReadByte();
                            var b2 = (byte)f.ReadByte();

                            row[(x * PixelSize) + 0] = b1;
                            row[(x * PixelSize) + 1] = b2;
                        }
                    }
                }
            }

            b.UnlockBits(bd);
            return b;
        }

        public static Bitmap LoadBitmapFrom555Debug(string filename, int width, int height, int offset)
        {
            var b = new Bitmap(width + 1, height + 1, System.Drawing.Imaging.PixelFormat.Format16bppRgb555);
            var bd = b.LockBits(new Rectangle(0, 0, width, height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format16bppRgb555);

            int PixelSize = 2;

            using (var f = new FileStream(filename, FileMode.Open))
            {
                f.Seek(offset, SeekOrigin.Begin);

                unsafe
                {
                    for (int y = 0; y <= height; y++)
                    {
                        byte* row = (byte*)bd.Scan0 + (y * bd.Stride);

                        var b1 = (byte)0xFF;
                        var b2 = (byte)0xFF;

                        for (int x = 0; x <= width; x++)
                        {
                            if (x != width && y != height)
                            {
                                b1 = (byte)f.ReadByte();
                                b2 = (byte)f.ReadByte();
                            }

                            if (x == width) b1 = b2 = (byte)((y % 2 == 0) ? 0xFF : 0x00);
                            if (y == height) b1 = b2 = (byte)((x % 2 == 0) ? 0xFF : 0x00);

                            row[(x * PixelSize) + 0] = b1;
                            row[(x * PixelSize) + 1] = b2;
                        }
                    }
                }
            }

            b.UnlockBits(bd);
            return b;
        }
    }
}

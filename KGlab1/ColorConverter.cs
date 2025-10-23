using System;

namespace KGlab1
{
    public static class ColorConverter
    {
        public static void RGBtoCMYK(byte r, byte g, byte b, out double c, out double m, out double y, out double k)
        {
            double R = r / 255.0;
            double G = g / 255.0;
            double B = b / 255.0;

            k = 1 - Math.Max(R, Math.Max(G, B));
            if (Math.Abs(k - 1.0) < 0.00001)
            {
                c = m = y = 0;
            }
            else
            {
                c = (1 - R - k) / (1 - k);
                m = (1 - G - k) / (1 - k);
                y = (1 - B - k) / (1 - k);
            }
        }

        public static void CMYKtoRGB(double c, double m, double y, double k, out byte r, out byte g, out byte b)
        {
            r = (byte)(255 * (1 - c) * (1 - k));
            g = (byte)(255 * (1 - m) * (1 - k));
            b = (byte)(255 * (1 - y) * (1 - k));
        }

        public static void RGBtoHLS(byte r, byte g, byte b, out double h, out double l, out double s)
        {
            double R = r / 255.0;
            double G = g / 255.0;
            double B = b / 255.0;

            double max = Math.Max(R, Math.Max(G, B));
            double min = Math.Min(R, Math.Min(G, B));
            l = (max + min) / 2.0;

            if (Math.Abs(max - min) < 0.00001)
            {
                h = s = 0;
            }
            else
            {
                double d = max - min;
                s = l > 0.5 ? d / (2.0 - max - min) : d / (max + min);

                if (max == R)
                    h = (G - B) / d + (G < B ? 6 : 0);
                else if (max == G)
                    h = (B - R) / d + 2;
                else
                    h = (R - G) / d + 4;

                h *= 60;
            }
            if (h < 0 || h > 360 || l < 0 || l > 1 || s < 0 || s > 1)
                throw new ArgumentOutOfRangeException("HLS values are out of valid range. RGB cannot be represented correctly in HLS.");
        }

        public static void HLStoRGB(double h, double l, double s, out byte r, out byte g, out byte b)
        {
            if (h < 0 || h > 360 || l < 0 || l > 1 || s < 0 || s > 1)
                throw new ArgumentOutOfRangeException("HLS values are out of valid range (H: 0–360, L/S: 0–1)");


            if (l <= 0)
            {
                r = g = b = 0;
                return;
            }

            if (s <= 0)
            {
                byte gray = (byte)Math.Round(l * 255);
                r = g = b = gray;
                return;
            }

            double q = l < 0.5 ? l * (1 + s) : l + s - l * s;
            double p = 2 * l - q;
            double hk = h / 360.0;

            double[] t = { hk + 1.0 / 3.0, hk, hk - 1.0 / 3.0 };
            double[] rgb = new double[3];

            for (int i = 0; i < 3; i++)
            {
                if (t[i] < 0) t[i] += 1;
                if (t[i] > 1) t[i] -= 1;

                if (t[i] < 1.0 / 6.0)
                    rgb[i] = p + (q - p) * 6 * t[i];
                else if (t[i] < 0.5)
                    rgb[i] = q;
                else if (t[i] < 2.0 / 3.0)
                    rgb[i] = p + (q - p) * (2.0 / 3.0 - t[i]) * 6;
                else
                    rgb[i] = p;
            }

            for (int i = 0; i < 3; i++)
            {
                if (rgb[i] < 1e-5) rgb[i] = 0;
                if (rgb[i] > 1.0) rgb[i] = 1.0;
            }

            r = (byte)Math.Round(rgb[0] * 255);
            g = (byte)Math.Round(rgb[1] * 255);
            b = (byte)Math.Round(rgb[2] * 255);
        }

    }
}

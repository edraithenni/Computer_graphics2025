using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace KGlab1
{
    public partial class MainWindow : Window
    {
        private bool isUpdating = false;
        private bool isLoaded = false;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded; 
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            isLoaded = true;
            RGB_Changed(null, null);
        }

        private void UpdatePreview(byte r, byte g, byte b)
        {
            ColorPreview.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(r, g, b));
        }

        private void RGB_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!isLoaded || isUpdating) return;
            isUpdating = true;

            byte r = (byte)R_Slider.Value;
            byte g = (byte)G_Slider.Value;
            byte b = (byte)B_Slider.Value;

            R_Text.Text = r.ToString();
            G_Text.Text = g.ToString();
            B_Text.Text = b.ToString();

            ColorConverter.RGBtoCMYK(r, g, b, out double c, out double m, out double y, out double k);
            UpdateCMYK(c, m, y, k);

            ColorConverter.RGBtoHLS(r, g, b, out double h, out double l, out double s);
            UpdateHLS(h, l, s);

            UpdatePreview(r, g, b);
            isUpdating = false;
        }

        private void CMYK_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!isLoaded || isUpdating) return;
            isUpdating = true;

            double c = C_Slider.Value / 100;
            double m = M_Slider.Value / 100;
            double y = Y_Slider.Value / 100;
            double k = K_Slider.Value / 100;

            C_Text.Text = (c * 100).ToString("F1");
            M_Text.Text = (m * 100).ToString("F1");
            Y_Text.Text = (y * 100).ToString("F1");
            K_Text.Text = (k * 100).ToString("F1");

            ColorConverter.CMYKtoRGB(c, m, y, k, out byte r, out byte g, out byte b);
            UpdateRGB(r, g, b);

            ColorConverter.RGBtoHLS(r, g, b, out double h, out double l, out double s);
            UpdateHLS(h, l, s);

            UpdatePreview(r, g, b);
            isUpdating = false;
        }

        private void HLS_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!isLoaded || isUpdating) return;
            isUpdating = true;

            double h = H_Slider.Value;
            double l = L_Slider.Value / 100.0; 
            double s = S_Slider.Value / 100.0;

            H_Text.Text = h.ToString("F1");
            L_Text.Text = L_Slider.Value.ToString("F2");
            S_Text.Text = S_Slider.Value.ToString("F2");

            ColorConverter.HLStoRGB(h, l, s, out byte r, out byte g, out byte b);

            UpdateRGB(r, g, b);

            ColorConverter.RGBtoCMYK(r, g, b, out double c, out double m, out double y, out double k);
            UpdateCMYK(c, m, y, k);

            UpdatePreview(r, g, b);
            isUpdating = false;
        }



        private void UpdateRGB(byte r, byte g, byte b)
        {
            R_Slider.Value = r;
            G_Slider.Value = g;
            B_Slider.Value = b;

            R_Text.Text = r.ToString();
            G_Text.Text = g.ToString();
            B_Text.Text = b.ToString();
        }

        private void UpdateCMYK(double c, double m, double y, double k)
        {
            C_Slider.Value = c * 100;
            M_Slider.Value = m * 100;
            Y_Slider.Value = y * 100;
            K_Slider.Value = k * 100;

            C_Text.Text = (c * 100).ToString("F1");
            M_Text.Text = (m * 100).ToString("F1");
            Y_Text.Text = (y * 100).ToString("F1");
            K_Text.Text = (k * 100).ToString("F1");
        }

        private void UpdateHLS(double h, double l, double s)
        {
            H_Slider.Value = h;
            L_Slider.Value = l * 100;
            S_Slider.Value = s * 100;

            H_Text.Text = h.ToString("F1");
            L_Text.Text = (l * 100).ToString("F2");
            S_Text.Text = (s * 100).ToString("F2");
        }




        private void PickColor_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new System.Windows.Forms.ColorDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var color = dlg.Color;
                UpdateRGB(color.R, color.G, color.B);
                RGB_Changed(null, null);
            }
        }

        private void RGB_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!isLoaded || isUpdating) return;
            isUpdating = true;

            byte r = ParseByte(R_Text, 0, 255);
            byte g = ParseByte(G_Text, 0, 255);
            byte b = ParseByte(B_Text, 0, 255);

            R_Slider.Value = r;
            G_Slider.Value = g;
            B_Slider.Value = b;

            ColorConverter.RGBtoCMYK(r, g, b, out double c, out double m, out double y, out double k);
            UpdateCMYK(c, m, y, k);

            //ColorConverter.RGBtoHLS(r, g, b, out double h, out double l, out double s);

           // UpdateHLS(h, l * 100, s * 100);

            UpdatePreview(r, g, b);

            try
            {
                ColorConverter.RGBtoHLS(r, g, b, out double h, out double l, out double s);
                UpdateHLS(h, l, s);
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show(
                    "These rgb values cannot be converted to HLS.\n" +
                    "HLS cannot be set for this color",
                    "Convertation error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );

                UpdateHLS(0, 0, 0);
            }
            isUpdating = false;
        }

        private void CMYK_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!isLoaded || isUpdating) return;
            isUpdating = true;

            double c = ParseDouble(C_Text, 0, 100) / 100;
            double m = ParseDouble(M_Text, 0, 100) / 100;
            double y = ParseDouble(Y_Text, 0, 100) / 100;
            double k = ParseDouble(K_Text, 0, 100) / 100;

            C_Slider.Value = c * 100;
            M_Slider.Value = m * 100;
            Y_Slider.Value = y * 100;
            K_Slider.Value = k * 100;

            ColorConverter.CMYKtoRGB(c, m, y, k, out byte r, out byte g, out byte b);
            UpdateRGB(r, g, b);

            ColorConverter.RGBtoHLS(r, g, b, out double h, out double l, out double s);
            UpdateHLS(h, l, s);

            UpdatePreview(r, g, b);
            isUpdating = false;
        }

        private void HLS_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!isLoaded || isUpdating) return;
            isUpdating = true;

            double h = ParseDouble(H_Text, 0, 360);
            double l = ParseDouble(L_Text, 0, 100) / 100.0; 
            double s = ParseDouble(S_Text, 0, 100) / 100.0; 

            H_Slider.Value = h;
            L_Slider.Value = l * 100; 
            S_Slider.Value = s * 100;

            ColorConverter.HLStoRGB(h, l, s, out byte r, out byte g, out byte b);
            UpdateRGB(r, g, b);

            ColorConverter.RGBtoCMYK(r, g, b, out double c, out double m, out double y, out double k);
            UpdateCMYK(c, m, y, k);

            UpdatePreview(r, g, b);
            isUpdating = false;
        }



        private double ParseDouble(TextBox tb, double min, double max)
        {
            if (double.TryParse(tb.Text, out double val))
            {
                if (val < min) val = min;
                if (val > max) val = max;
                return val;
            }
            return min;
        }

        private byte ParseByte(TextBox tb, byte min, byte max)
        {
            if (byte.TryParse(tb.Text, out byte val))
            {
                if (val < min) val = min;
                if (val > max) val = max;
                return val;
            }
            return min;
        }

    }
}

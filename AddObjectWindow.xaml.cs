using hogs_gameManager_wpf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace hogs_gameEditor_wpf
{
    /// <summary>
    /// Interaction logic for AddObjectWindow.xaml
    /// </summary>
    public partial class AddObjectWindow : Window
    {
        string mapName;
        int index;
        public AddObjectWindow(string mapName,int index)
        {
            this.mapName = mapName;
            this.index = index;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.nameComboBox.Items.Add("Pig");
            this.nameComboBox.Items.Add("Explosive Drum");
            this.nameComboBox.Items.Add("Gas Drum");
            this.nameComboBox.Items.Add("Weapon Crate");
            this.nameComboBox.Items.Add("Health Crate");
            this.nameComboBox.Items.Add("Promotion Point");
            this.nameComboBox.Items.Add("Tank");
            this.nameComboBox.Items.Add("Water Tank");
            this.nameComboBox.Items.Add("Carry");
            this.nameComboBox.Items.Add("Water Carry");
            this.nameComboBox.Items.Add("Artillery");
            this.nameComboBox.Items.Add("PillBox");
            this.nameComboBox.Items.Add("small Tent");
            this.nameComboBox.Items.Add("Medical Tent (green)");
            this.nameComboBox.Items.Add("Medical Tent (tan)");
            this.nameComboBox.Items.Add("shelter");

            this.mapImage.Source = new BitmapImage(new Uri("file://D:/Games/IGG-HogsofWar/Maps/pngs/" + mapName + ".png"));

            Ellipse eli = new Ellipse();
            eli.Width = 14;
            eli.Height = 14;
            eli.Fill = Brushes.Transparent;
            eli.Stroke = Brushes.Red;
            eli.StrokeThickness = 2;
            eli.MouseMove += Eli_MouseMove;

            this.mapCanvas.Children.Add(eli);
            Canvas.SetLeft(eli, 116);
            Canvas.SetTop(eli, 116);

        }

        private void Eli_MouseMove(object sender, MouseEventArgs e)
        {
            Ellipse eli = (Ellipse)sender;
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                double x = e.GetPosition(mapCanvas).X - 7;
                double y = e.GetPosition(mapCanvas).Y - 7;

                if( x <= 240 && 8 <= x) { Canvas.SetLeft(eli, x); }
                if( y <= 240 && 8 <= y) { Canvas.SetTop(eli, y); }

                this.label_Copy0.Content = x*128 + " | " + y*128;
            }
            
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {

            FrameworkElement fe = (FrameworkElement)this.mapCanvas.Children[0];
            double top1 = Canvas.GetTop(fe) * 128;
            double left1 = Canvas.GetLeft(fe) * 128;

            MapObjectV3 mo = new MapObjectV3
            {
                name = "".ToCharArray(),
                unused0 = "NULL    ".ToCharArray(),
                position = new short[] { Convert.ToInt16(top1), 10, Convert.ToInt16(left1) },
                index = Convert.ToUInt16(index),
                angles = new short[] { 0, Convert.ToInt16(this.rotationSlider.Value), 0 },
                type = (ushort)this.typeUShortUpDown.Value,
                bounds = new short[] { 5, 5, 5 },
                bounds_type = 1,
                energy = (short)this.energyShortUpDown.Value,
                appearance = (byte)this.apperByteUpDown.Value,
                team = (byte)this.teamByteUpDown.Value,
                objective = 0,
                objective_actor_id = 0,
                objective_extra = new byte[] { 0, 0 },
                unused1 = 0,
                unused2 = new ushort[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                fallback_position = new short[] { 0, 0, 0 },
                extra = (short)this.extraShortUpDown.Value,
                attached_actor_num = (short)this.att_actorNumShortUpDown.Value,
                unused3 = 0
            }; 
        }
    }
}

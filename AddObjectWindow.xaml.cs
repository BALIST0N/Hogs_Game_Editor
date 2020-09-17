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
        Dictionary<string, string> itemsNameconvert;
        Dictionary<string, short> ranks;
        Dictionary<string, byte> weaponList;

        public AddObjectWindow(string mapName,int index)
        {
            this.mapName = mapName;
            this.index = index;
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            itemsNameconvert = new Dictionary<string, string>();
            ranks = new Dictionary<string, short>();
            weaponList = new Dictionary<string, byte>();

            weaponParams1.ItemsSource = weaponList.Keys;

            ranks.Add("Grunt", 0);
            ranks.Add("Gunner", 1);
            ranks.Add("Bombardier", 2);
            ranks.Add("Pyrotechnic", 3);
            ranks.Add("Sapper", 5);
            ranks.Add("Engineer", 6);
            ranks.Add("Saboteur", 7);
            ranks.Add("Scout", 8);
            ranks.Add("Sniper", 9);
            ranks.Add("Spy", 10);
            ranks.Add("Orderly", 11);
            ranks.Add("Medic", 12);
            ranks.Add("Surgeon", 13);
            ranks.Add("Commando", 4);
            ranks.Add("Hero", 14);
            ranks.Add("Ace", 15);
            ranks.Add("Legend", 16);
            ranks.Add("Grenadier MP", 17);
            ranks.Add("Gunner MP", 18);
            ranks.Add("Sapper MP", 19);
            ranks.Add("Scout MP", 20);

            this.typeComboBox.ItemsSource = ranks.Keys;

            this.teamComboBox.Items.Add(1);
            this.teamComboBox.Items.Add(2);
            this.teamComboBox.Items.Add(4);
            this.teamComboBox.Items.Add(8);
            this.teamComboBox.Items.Add(16);
            this.teamComboBox.Items.Add(32);

            this.apperCombobox.Items.Add("non-Player");
            this.apperCombobox.Items.Add("Player");

            itemsNameconvert.Add("Pig","GR_ME");
            itemsNameconvert.Add("Explosive Drum", "DRUM1");
            itemsNameconvert.Add("Gas Drum", "DRUM2");
            itemsNameconvert.Add("Weapon Crate", "CRATE1");
            itemsNameconvert.Add("Health Crate", "CRATE2");
            itemsNameconvert.Add("Promotion Point", "PROPOINT");
            itemsNameconvert.Add("Tank", "TANK");
            itemsNameconvert.Add("Water Tank", "AM_TANK");
            itemsNameconvert.Add("Carry", "CARRY");
            itemsNameconvert.Add("Water Carry", "AM_CARRY");
            itemsNameconvert.Add("Artillery", "BIG_GUN");
            itemsNameconvert.Add("PillBox", "PILLBOX");
            itemsNameconvert.Add("Small Tent", "TENT_S");
            itemsNameconvert.Add("Medical Tent (green)", "M_TENT1");
            itemsNameconvert.Add("Medical Tent (tan)", "M_TENT2");
            itemsNameconvert.Add("Shelter", "SHELTER");

            this.nameComboBox.ItemsSource = itemsNameconvert.Keys;

            weaponList.Add("Fist", 01);
            weaponList.Add("Knife", 02);
            weaponList.Add("Bajonett", 03);
            weaponList.Add("Saber", 04);
            weaponList.Add("Cattle Prod", 05);
            weaponList.Add("Pistol", 06);
            weaponList.Add("Rifle", 07);
            weaponList.Add("Rifle Burst", 08);
            weaponList.Add("MG", 09);
            weaponList.Add("Heavy MG", 10);
            weaponList.Add("Sniper Rifle", 11);
            weaponList.Add("Shotgun", 12);
            weaponList.Add("Flamethrower", 13);
            weaponList.Add("Rocket Launcher", 14);
            weaponList.Add("Guided Missile", 15);
            weaponList.Add("Medicine Dart", 16);
            weaponList.Add("Tranquiliser", 17);
            weaponList.Add("Grenade", 18);
            weaponList.Add("Clustergrenade", 19);
            weaponList.Add("HX-Grenade", 20);
            weaponList.Add("Roller Grenade", 21);
            weaponList.Add("Confusion Gas", 22);
            weaponList.Add("Freeze Gas", 23);
            weaponList.Add("Madness Gas", 24);
            weaponList.Add("Poison Gas", 25);
            weaponList.Add("Mortar", 26);
            weaponList.Add("Bazooka", 27);
            weaponList.Add("Airburst", 28);
            weaponList.Add("Super Airburst", 29);
            weaponList.Add("Medicine Ball", 30);
            weaponList.Add("Homing Missile", 31);
            weaponList.Add("Mine", 32);
            weaponList.Add("Anti-P Mine", 33);
            weaponList.Add("TNT", 34);
            weaponList.Add("Long Range Shell (Artillery)", 35);
            weaponList.Add("Mine Shell (Artillery)", 36);
            weaponList.Add("Poison Shell (Artillery)", 37);
            weaponList.Add("Fire Rain Shell (Artillery)", 38);
            weaponList.Add("1000 LBS Shell (Artillery)", 39);
            weaponList.Add("Shock Shell (Artillery)", 40);
            weaponList.Add("Heavy M-Gun (MG)", 41);
            weaponList.Add("Flamethrower (MG)", 42);
            weaponList.Add("Airburst (Tank)", 43);
            weaponList.Add("Bazooka (Tank)", 44);
            weaponList.Add("Mortar (Tank)" , 45); 
            weaponList.Add("Jetpack", 51);
            weaponList.Add("Suicide", 52);
            weaponList.Add("Healing Hands", 53);
            weaponList.Add("Self Heal", 54);
            weaponList.Add("Pick Pocket", 55);
            weaponList.Add("Shockwave", 56);
            weaponList.Add("Spec-Ops", 57);
            weaponList.Add("Airstrike", 58);
            weaponList.Add("Fire Rain Airstrike", 59);
            weaponList.Add("Map View (Ability/Freeze Timer)", 63);
            weaponList.Add("Binoculars (Ability/Cosmetic)", 64);
            weaponList.Add("Skip Turn (Ability)", 65);
            weaponList.Add("Surrender (Ability)", 66);
            weaponList.Add("HX-TNT", 67);
            weaponList.Add("Hide", 68);
            weaponList.Add("Super Shotgun", 69);
            weaponList.Add("Shrapnel Grenade", 70);
            weaponList.Add("Grenade Launcher", 71);

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

                this.label_Copy0.Content = (x*128 - 16384) + " | " + -(y*128 - 16384);
            }
            
        }

        private void nameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(this.nameComboBox.SelectedIndex != -1)
            {
                if( (string)this.nameComboBox.SelectedItem == "Pig")
                {
                    this.typeUShortUpDown.Visibility = Visibility.Hidden;
                    this.typeComboBox.Visibility = Visibility.Visible;
                }
                else
                {
                    this.typeUShortUpDown.Visibility = Visibility.Visible;
                    this.typeComboBox.Visibility = Visibility.Hidden;
                }

                if ((string)this.nameComboBox.SelectedItem == "Health Crate")
                {
                    this.label_amount.Visibility = Visibility.Visible;
                    this.weaponParams2.Visibility = Visibility.Visible;
                }
                else
                {
                    this.label_amount.Visibility = Visibility.Hidden;
                    this.weaponParams2.Visibility = Visibility.Hidden;
                }

                if ((string)this.nameComboBox.SelectedItem == "Weapon Crate")
                {
                    this.label_amount.Visibility = Visibility.Visible;
                    this.label_weap.Visibility = Visibility.Visible;
                    this.weaponParams1.Visibility = Visibility.Visible;
                    this.weaponParams2.Visibility = Visibility.Visible;
                }
                else
                {
                    this.label_weap.Visibility = Visibility.Hidden;
                    this.weaponParams1.Visibility = Visibility.Hidden;
                }
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement fe = (FrameworkElement)this.mapCanvas.Children[0];
            double top1 = (Canvas.GetTop(fe)+7) * 128 - 16384;
            double left1 = (Canvas.GetLeft(fe)+7) * 128 - 16384;

            MapObjectV3 mo = new MapObjectV3();
            mo.name = GetSelectedName();
            mo.unused0 = "NULL\0\0\0\0\0\0\0\0\0\0\0\0".ToCharArray();
            mo.position = new short[] { Convert.ToInt16(left1), 10, Convert.ToInt16(-top1) };
            mo.index = Convert.ToUInt16(index+1);
            mo.angles = new short[] { 0, Convert.ToInt16(this.rotationSlider.Value), 0 };
            mo.type = (ushort)this.typeUShortUpDown.Value;
            mo.bounds = new short[] { 5, 5, 5 };
            mo.bounds_type = 1;
            mo.energy = (short)this.energyShortUpDown.Value;
            mo.appearance = GetAppearance();
            mo.team =  Convert.ToByte( this.teamComboBox.SelectedItem);
            mo.objective = 0;
            mo.objective_actor_id = 0;
            mo.objective_extra = GetObjectiveParams();
            mo.unused1 = 0;
            mo.unused2 = new ushort[] { 0, 0, 0, 0, 0, 0, 0, 0 };
            mo.fallback_position = new short[] { 0, 0, 0 };
            mo.extra = (short)this.extraShortUpDown.Value;
            mo.attached_actor_num = (short)this.att_actorNumShortUpDown.Value;
            mo.unused3 = 0;

            MainWindow main = (MainWindow)Application.Current.MainWindow;

            main.CurrentMap.Add(mo);
            main.MapObjectsListView.Items.Add(newItem: new MapObjectsListViewItem { Name = new String(mo.name), Id = Convert.ToString(mo.index), Team = Convert.ToString(mo.team) });  //this is just adding a row on the listbox
            main.LoadMapObjects();
            main.mapObjectEdited = true;
            this.Close();

        }

        private char[] GetSelectedName()
        {          
            List<char> res = new List<char>();
            res.AddRange( itemsNameconvert[this.nameComboBox.Text].ToCharArray() );
            for(int i = res.Count;i<16;i++)
            {
                res.Add('\0');
            }
            return res.ToArray();
        }

        private byte GetAppearance()
        {
            if(this.apperCombobox.SelectedIndex == 0) { return 63; }
            else { return 127; }
        }

        private byte[] GetObjectiveParams()
        {
            byte[] res = new byte[] { 0, 0 };

            if(this.nameComboBox.SelectedIndex == 3)
            {
                res[0] = weaponList[this.weaponParams1.SelectedItem.ToString()];
                res[1] = (byte)weaponParams2.Value;
            }

            if (this.nameComboBox.SelectedIndex == 4)
            {
                res[0] = 255;
                res[1] = (byte)weaponParams2.Value;
            }

            return new byte[] { 0, 0 };
        }

    }
}

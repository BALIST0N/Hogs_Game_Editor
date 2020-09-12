using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace hogs_gameManager_wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Dictionary<string, string> MapList;
        List<MapObjectV3> CurrentMap;
        public MainWindow()
        {
            InitializeComponent();

            #region MapList Fillings
            this.MapList = new Dictionary<string, string>
            {
                { "You Hillock", "ARCHI" },
                { "Doomed", "ARTGUN" },
                { "15: Fortified Swine", "BAY" },
                { "Dam Busters", "BHILL" },
                { "Friendly Fire", "BOOM" },
                { "17: Geneva Convention", "BRIDGE" },
                { "Moon Buttes", "BUTE" },
                { "00: Boot Camp", "CAMP" },
                { "Cratermass", "CMASS" },
                { "Graveyard Shift", "CREEPY2" },
                { "Death Bowl", "DBOWL" },
                { "04 Beta", "DEMO" },
                { "Ice Flow Beta", "DEMO2" },
                { "18: I Spy", "DESVAL" },
                { "04: Morning Glory", "DEVI" },
                { "Death Valley Beta", "DVAL" },
                { "Death Valley", "DVAL2" },
                { "15 Beta", "EASY" },
                { "20: Achilles Heal", "EMPLACE" },
                { "01: The War Foundation", "ESTU" },
                { "14: Battle Stations", "EYRIE" },
                { "25: Well, Well, Well!", "FINAL" },
                { "13: Glacier Guns", "FJORDS" },
                { "24: Hamburger Hill", "FOOT" },
                { "10: Bangers 'N' Mash", "GUNS" },
                { "Pigin' Hell", "HELL2" },
                { "Skulduggery", "HELL3" },
                { "Completely unused", "HILLBASE" },
                { "Chill Hill", "ICE" },
                { "Ice Flow", "ICEFLOW" },
                { "Island Hopper", "ISLAND" },
                { "22: Assassination", "KEEP" },
                { "The Lake", "LAKE" },
                { "Bridge The Gap", "LECPROD" },
                { "11: Saving Private Rind", "LIBERATE" },
                { "Pigs in Space", "LUNAR1" },
                { "09: The Village People", "MASHED" },
                { "Hedge Maze", "MAZE" },
                { "16: Over The Top", "MEDIX" },
                { "Frost Fight", "MLAKE" },
                { "12: Just Deserts", "OASIS" },
                { "One Way System", "ONEWAY" },
                { "Play Pen", "PLAY1" },
                { "Duvet Fun", "PLAY2" },
                { "Ridge Back", "RIDGE" },
                { "02: Routine Patrol", "ROAD" },
                { "05: Island Invasion", "RUMBLE" },
                { "Square Off", "SEPIA1" },
                { "19: Chemical Compound", "SNAKE" },
                { "08: The Spying Game", "SNIPER" },
                { "21: High And Dry", "SUPLINE" },
                { "23: Hero Warship", "TESTER" },
                { "03: Trench Warfare", "TRENCH" },
                { "07: Communication Breakdown", "TWIN" },
                { "06: Under Siege", "ZULUS" }
            };

            this.MapList = MapList.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            #endregion

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.mapListComboBox.ItemsSource = MapList.Keys;
        }

        private void MapListComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if( this.mapListComboBox.SelectedIndex != -1 )
            {
                /* 
                if(e.RemovedItems.Count > 0)
                {
                    if ( Xceed.Wpf.Toolkit.MessageBox.Show("would You like to save your data on this map ?", "Attention", MessageBoxButton.YesNoCancel) == MessageBoxResult.Yes )
                    {
                        //Save into the file                        
                        public void SaveMap(List<MapObject> map)
                        {
                            foreach( MapObject mo in CurrentMap)
                            {
                            }
                        }
                    }
                }
                */

                //clear to avoid Exceptions
                this.MapObjectPropertiesControl.SelectedObject = null;
                this.MapObjectsListView.Items.Clear();
                CurrentMap = new List<MapObjectV3>();
                this.CanvasImageMap.Children.Clear();

                byte[] mapdata = File.ReadAllBytes("D:/Games/IGG-HogsofWar/Maps/" + MapList.ElementAt(mapListComboBox.SelectedIndex).Value + ".POG"); //Read the File
                int blocks = mapdata.Length / 94; //Count number of map objects

                for (int i = 1; i < blocks; i++)
                {
                    int endblock = i * 94 + 2;
                    int startblock = endblock - 94; //a map object is 94 bytes, so every 94 bytes, cut and create a mapobject

                    if (endblock < mapdata.Length)//if this is the end of file
                    {
                        //MapObject mo = new MapObject(mapdata[startblock..endblock]);
                        MapObjectV3 mo = new MapObjectV3(mapdata[startblock..endblock]);
                        CurrentMap.Add(mo);

                        this.MapObjectsListView.Items.Add(newItem: new MapObjectsListViewItem { Name = new String(mo.name), Id = Convert.ToString(mo.index), Group = Convert.ToString(mo.team) });  //this is just adding a row on the listbox
                    }
                }
                this.MapImageControl.Source = new BitmapImage(new Uri("file://D:/Games/IGG-HogsofWar/Maps/pngs/"+ MapList.ElementAt(mapListComboBox.SelectedIndex).Value + ".png")); //loading the center map
                //this.MapImageControl.Source = new BitmapImage(new Uri("file://D:/Games/IGG-HogsofWar/Maps/pngs/temp.png"));

                
                //generate buttons with icons in the minimap
                foreach (MapObjectV3 mo in CurrentMap)
                {
                    string test = new String(mo.name).TrimEnd('\0');    //remove "\0" chars
                    switch(test)    //check the mapobject name and draw it differently accoring to his name
                    {
                        case "AC_ME":
                        case "CO_ME":
                        case "GR_ME":
                        case "HV_ME":
                        case "LE_ME":
                        case "ME_ME":
                        case "SA_ME":
                        case "SB_ME":
                        case "SN_ME":
                        case "SP_ME":
                            if(mo.appearance == 1) 
                            { 
                                this.CanvasImageMap.Children.Add(GenerateObjectMapButton(mo, Brushes.Lime) ); 
                            }
                            else
                            { 
                                this.CanvasImageMap.Children.Add(GenerateObjectMapButton(mo, Brushes.Crimson) );  
                            }
                            break;

                        case "DRUM":
                            break;
                        case "DRUM2":
                            break;

                        case "CRATE1":
                        case "CRATE4":
                            this.CanvasImageMap.Children.Add(GenerateObjectMapButton(mo, Brushes.DarkGoldenrod)); 
                            
                            break;

                        case "CRATE2":
                            this.CanvasImageMap.Children.Add(GenerateObjectMapButton(mo, Brushes.DeepPink));
                            
                            break;
                        
                        case "PROPOINT":
                            this.CanvasImageMap.Children.Add(GenerateObjectMapButton(mo, Brushes.Yellow,Brushes.Gold));
                            break;

                        case "AM_TANK":
                        case "TANK":
                        case "CARRY	":
                        case "AMLAUNCH":
                            this.CanvasImageMap.Children.Add(GenerateObjectMapButton(mo, Brushes.Gray, Brushes.Blue));
                            break;

                        case "BIG_GUN":
                            this.CanvasImageMap.Children.Add(GenerateObjectMapButton(mo, Brushes.Black, Brushes.DarkBlue));
                            break;

                        case "PILLBOX":
                            this.CanvasImageMap.Children.Add(GenerateObjectMapButton(mo, Brushes.Wheat, Brushes.White));
                            break;

                        case "M_TENT1":
                        case "M_TENT2":
                        case "TENT_S":
                            this.CanvasImageMap.Children.Add(GenerateObjectMapButton(mo, Brushes.Green,Brushes.Pink));
                            break;

                        case "SHELTER":
                            this.CanvasImageMap.Children.Add(GenerateObjectMapButton(mo, Brushes.Gray, Brushes.Orange));
                            break;

                        default:
                            //this.CanvasImageMap.Children.Add(GenerateObjectMapButton(mo, Brushes.White));
                            break;
                    }
                }

            }
        }

        private void MapObjectsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)    //click on a different map object
        {
            if(this.MapObjectsListView.SelectedIndex != -1)
            {
                this.MapObjectPropertiesControl.SelectedObject = CurrentMap[this.MapObjectsListView.SelectedIndex];
                this.MapObjectPropertiesControl.SelectedObjectName = new string( CurrentMap[this.MapObjectsListView.SelectedIndex].name);  //"new string" cuz char[]
                this.MapObjectPropertiesControl.SelectedObjectTypeName = "Object n°" + this.MapObjectsListView.SelectedIndex.ToString();
                this.MapObjectPropertiesControl.Update();
                this.MapObjectPropertiesControl.ExpandAllProperties();
            }
        }

        private System.Windows.Shapes.Rectangle GenerateObjectMapButton(MapObjectV3 mo)
        {
            //MessageBox.Show(CurrentMap.IndexOf(mo).ToString() + " '\n\r" + mo.position[0] + " " + mo.position[1] + " '\n\r" + Math.Round(mo.position[0] / 72.81, 2) + " " + Math.Round(mo.position[1] / 72.81, 2));

            double x = -mo.position[0] / 2; //map size is 32768 px, the panel is 450, 32768/450 = 72.81 , map scale : )
            double y = mo.position[1] / 2;

            System.Windows.Shapes.Rectangle b = new System.Windows.Shapes.Rectangle
            {
                Name = "n" + CurrentMap.IndexOf(mo).ToString(),
                Width = 9,
                Height = 9,
                StrokeThickness = 1.5,
                Stroke = Brushes.Black,
                VerticalAlignment = VerticalAlignment.Center,
            };
            b.MouseDown += B_Click;
            Canvas.SetLeft(b, x+225);
            Canvas.SetTop(b, y+225);

            return b;
        }
        private System.Windows.Shapes.Rectangle GenerateObjectMapButton(MapObjectV3 mo,Brush backColor)
        {
            System.Windows.Shapes.Rectangle b = GenerateObjectMapButton(mo);
            b.Fill = backColor;

            return b;
        }
        private System.Windows.Shapes.Rectangle GenerateObjectMapButton(MapObjectV3 mo, Brush backColor,Brush bordercolor)
        {
            System.Windows.Shapes.Rectangle b = GenerateObjectMapButton(mo);
            b.Fill = backColor;
            b.Stroke = bordercolor;

            return b;
        }

        private Button GenerateBasicButton(MapObject mo)
        {
            double x = mo.XOffset / 291.2;
            double y = mo.YOffset / 145.6;

            Button b = new Button();
            //b.Name = "n" + CurrentMap.IndexOf(mo).ToString();
            b.Width = 9;
            b.Height = 9;
            b.BorderThickness = new Thickness(1.5);
            b.BorderBrush = Brushes.Black;
            b.Margin = new Thickness(y, x, 0, 0);
            b.Click += B_Click;
            return b;
        }

        private void B_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Shapes.Rectangle b = (System.Windows.Shapes.Rectangle)sender;
            this.MapObjectsListView.SelectedIndex = Convert.ToInt32( b.Name.Replace("n", String.Empty) );
            this.MapObjectsListView.ScrollIntoView(this.MapObjectsListView.Items[this.MapObjectsListView.SelectedIndex]);

            //int TERRAIN_PIXEL_WIDTH = TERRAIN_TILE_PIXEL_WIDTH * TERRAIN_ROW_TILES; //512 * 256 = 131072 ?.?.??.
        }
    }

    class MapObjectsListViewItem
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Group { get; set; }
    }
}

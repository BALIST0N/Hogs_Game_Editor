using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using hogs_gameEditor_wpf;

namespace hogs_gameManager_wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string, string> MapList;
        public List<MapObjectV3> CurrentMap;
        string CurrentMapName;
        public bool mapObjectEdited = false;
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
            this.MapObjectsListView.KeyUp += MapObjectsListView_KeyUp;
        }

        private void MapListComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if( this.mapListComboBox.SelectedIndex != -1 )
            {
                if(e.RemovedItems.Count > 0 && mapObjectEdited)
                {
                    if ( Xceed.Wpf.Toolkit.MessageBox.Show("would You like to save your data on this map ?", "Attention", MessageBoxButton.YesNoCancel) == MessageBoxResult.Yes )
                    {
                        SaveFile();
                    }
                }
                
                //clear to avoid Exceptions
                this.MapObjectPropertiesControl.SelectedObject = null;
                this.MapObjectsListView.Items.Clear();
                this.CanvasImageMap.Children.Clear();
                CurrentMap = new List<MapObjectV3>();
                this.mapObjectEdited = false;

                this.CurrentMapName = MapList.ElementAt(mapListComboBox.SelectedIndex).Value;

                //Read the File
                using (FileStream fs = File.Open("D:/Games/IGG-HogsofWar/Maps/" + CurrentMapName + ".POG", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    byte[] mapdata = new byte[fs.Length];
                    fs.Read( mapdata, 0, Convert.ToInt32(fs.Length) );
                    ushort blocks = BitConverter.ToUInt16(mapdata, 0); //get number of map objects

                    for (int i = 1; i <= blocks; i++)
                    {
                        int endblock = i * 94 + 2;  
                        int startblock = endblock - 94; //a map object is 94 bytes, so every 94 bytes, cut and create a mapobject

                        if (endblock < mapdata.Length)  //if this is not the end of file
                        {
                            //MapObject mo = new MapObject(mapdata[startblock..endblock]);
                            MapObjectV3 mo = new MapObjectV3(mapdata[startblock..endblock]);
                            CurrentMap.Add(mo);

                            this.MapObjectsListView.Items.Add(newItem: new MapObjectsListViewItem { Name = new String(mo.name), Id = Convert.ToString(mo.index), Team = Convert.ToString(mo.team) });  //this is just adding a row on the listbox
                        }
                    }
                }               
                this.MapImageControl.Source = new BitmapImage(new Uri("file://D:/Games/IGG-HogsofWar/Maps/pngs/"+ CurrentMapName + ".png")); //loading the center map

                //MadModdingTool();

                //generate buttons with icons in the minimap
                LoadMapObjects();

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

        private void MapObjectsListView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && MapObjectsListView.SelectedIndex != -1)
            {
                MapObjectsListViewItem molv =  (MapObjectsListViewItem)MapObjectsListView.SelectedItem;
                MessageBoxResult res = MessageBox.Show("are you sure you want to delete Object n°" + molv.Id + " (" + molv.Name + ") ","A T T E N T I O N ",MessageBoxButton.YesNo);
                if(res == MessageBoxResult.Yes)
                {
                    CurrentMap.Remove( CurrentMap.Find(x => x.index == Convert.ToInt16( molv.Id) ) );
                    this.MapObjectsListView.Items.Clear();
                    foreach(MapObjectV3 mo in CurrentMap)
                    {
                        this.MapObjectsListView.Items.Add(newItem: new MapObjectsListViewItem { Name = new String(mo.name), Id = Convert.ToString(mo.index), Team = Convert.ToString(mo.team) }); 
                    }
                    
                    this.CanvasImageMap.Children.Clear();
                    LoadMapObjects();
                    
                    mapObjectEdited = true;
                }
            }
        }

        public void LoadMapObjects()
        {
            //generate buttons with icons in the minimap
            foreach (MapObjectV3 mo in CurrentMap)
            {
                string test = new String(mo.name).TrimEnd('\0');    //remove "\0" chars
                switch (test)    //check the mapobject name and draw it differently accoring to his name
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
                        if (mo.team == 1) { GenerateObjectMapButton(mo, Brushes.Lime); }
                        else { GenerateObjectMapButton(mo, Brushes.Crimson); }
                        break;

                    case "DRUM":
                        GenerateObjectMapButton(mo, Brushes.DarkOrange, Brushes.Crimson);
                        break;
                    case "DRUM2":
                        GenerateObjectMapButton(mo, Brushes.GreenYellow, Brushes.LawnGreen);
                        break;

                    case "CRATE1":
                    case "CRATE4":
                        GenerateObjectMapButton(mo, Brushes.DarkGoldenrod);

                        break;

                    case "CRATE2":
                        GenerateObjectMapButton(mo, Brushes.DeepPink, Brushes.Indigo);

                        break;

                    case "PROPOINT":
                        GenerateObjectMapButton(mo, Brushes.Yellow, Brushes.Gold);
                        break;

                    case "AM_TANK":
                    case "CARRY":
                    case "TANK":
                    case "AMLAUNCH":
                        GenerateObjectMapButton(mo, Brushes.Gray, Brushes.Blue);
                        break;

                    case "BIG_GUN":
                        GenerateObjectMapButton(mo, Brushes.Black, Brushes.DarkBlue);
                        break;

                    case "PILLBOX":
                        GenerateObjectMapButton(mo, Brushes.Wheat, Brushes.White);
                        break;

                    case "M_TENT1":
                    case "M_TENT2":
                    case "TENT_S":
                        GenerateObjectMapButton(mo, Brushes.Green, Brushes.Pink);
                        break;

                    case "SHELTER":
                        GenerateObjectMapButton(mo, Brushes.Gray, Brushes.Orange);
                        break;

                    default:
                        //GenerateObjectMapButton(mo, Brushes.White);
                        break;
                }
            }
        }

        private Rectangle GenerateObjectMapButton(MapObjectV3 mo)
        {
            //MessageBox.Show(CurrentMap.IndexOf(mo).ToString() + " '\n\r" + mo.position[0] + " " + mo.position[1] + " '\n\r" + Math.Round(mo.position[0] / 72.81, 2) + " " + Math.Round(mo.position[1] / 72.81, 2));

            Rectangle b = new Rectangle
            {
                Name = "n" + CurrentMap.IndexOf(mo).ToString(),
                Width = 9,
                Height = 9,
                StrokeThickness = 1.5,
                Stroke = Brushes.Black,
                VerticalAlignment = VerticalAlignment.Center,
            };
            b.MouseDown += B_Click;
            return b;
        }
        private void GenerateObjectMapButton(MapObjectV3 mo,Brush backColor)
        {
            Rectangle b = GenerateObjectMapButton(mo);
            b.Fill = backColor;
            SpawnObjectMapRectangle(b, mo);

        }
        private void GenerateObjectMapButton(MapObjectV3 mo, Brush backColor,Brush bordercolor)
        {
            Rectangle b = GenerateObjectMapButton(mo);
            b.Fill = backColor;
            b.Stroke = bordercolor;
            SpawnObjectMapRectangle(b, mo);
        }

        private void SpawnObjectMapRectangle(Rectangle R, MapObjectV3 mo)
        {
            double x = mo.position[0]/64;
            double y = -mo.position[2]/64;

            this.CanvasImageMap.Children.Add(R);
            Canvas.SetLeft(R, x+251);
            Canvas.SetTop(R, y+251);

        }

        private void SaveFile()
        {
            List<byte> mapdataList = new List<byte>();
            mapdataList.AddRange(BitConverter.GetBytes(Convert.ToUInt16(CurrentMap.Count)));
            foreach (MapObjectV3 mo in CurrentMap) { mapdataList.AddRange(mo.ConvertToByteArray()); }
            mapdataList.AddRange(new byte[] { 0, 0 });

            using (FileStream fs = File.OpenWrite("D:/Games/IGG-HogsofWar/Maps/" + CurrentMapName + "_edited.POG"))
            {
                fs.Write(mapdataList.ToArray(), 0, mapdataList.Count);
            }
        }

        private void SaveFile(List<MapObjectV3> map)//in case of ...?
        {
            List<byte> mapdataList = new List<byte>();
            mapdataList.AddRange(BitConverter.GetBytes( Convert.ToUInt16(map.Count) ));
            foreach (MapObjectV3 mo in map) { mapdataList.AddRange(mo.ConvertToByteArray()); }
            mapdataList.AddRange(new byte[] { 0, 0 });

            using ( FileStream fs = File.OpenWrite("D:/Games/IGG-HogsofWar/Maps/" + CurrentMapName + "_edited.POG") )
            {
                fs.Write(mapdataList.ToArray(), 0, mapdataList.Count );
            }
        }

        private void B_Click(object sender, RoutedEventArgs e)
        {
            Rectangle b = (Rectangle)sender;
            this.MapObjectsListView.SelectedIndex = Convert.ToInt32( b.Name.Replace("n", String.Empty) );
            this.MapObjectsListView.ScrollIntoView(this.MapObjectsListView.Items[this.MapObjectsListView.SelectedIndex]);
        }

        private void MapObjectPropertiesControl_PropertyValueChanged(object sender, Xceed.Wpf.Toolkit.PropertyGrid.PropertyValueChangedEventArgs e)
        {
            this.CanvasImageMap.Children.Clear();
            LoadMapObjects();
            this.mapObjectEdited = true;
        }

        private void AddNewObjectButton_Click(object sender, RoutedEventArgs e)
        {
            if(this.mapListComboBox.SelectedIndex != -1)
            {
                AddObjectWindow a = new AddObjectWindow(CurrentMapName, CurrentMap.Count);
                a.Show();
            }
        }

        private void MadModdingTool()
        {

            int added = 0;

            List<ModelObject> MADFILE = ModelObject.LoadMADFile(CurrentMapName);
            List<ModelObject> MADFILEofMods = ModelObject.LoadMADFile("MODDING");

            foreach (ModelObject modelobj in MADFILEofMods)
            {
                if (MADFILE.Any( x => x.Name.SequenceEqual(modelobj.Name) ) == false) 
                {
                    MADFILE.Add(modelobj);
                    added++;
                }
            }

            MADFILE.OrderBy(x => x.Name);
            MADFILE = ModelObject.recalculateOffsets(MADFILE);
            ModelObject.SaveMadFile(MADFILE, CurrentMapName);
            MessageBox.Show("the tool have added " + added + " new MODEL(s) on the map " + CurrentMapName);


            /*
            string[] filesNames = Directory.GetFiles("D:/Games/IGG-HogsofWar/Maps/", "*.MAD");

            int added = 0;
            foreach(string madfileName in filesNames)
            {
                madfileName.Replace("D:/Games/IGG-HogsofWar/Maps/", "");

                List<ModelObject> MADFILE = ModelObject.LoadMADFile(madfileName);
                List<ModelObject> MADFILEofMods = ModelObject.LoadMADFile("MODDING");

                foreach (ModelObject modelobj in MADFILEofMods)
                {
                    if (MADFILE.Exists(x => x.Name == modelobj.Name) == false)
                    {
                        MADFILE.Add(modelobj);
                        added++;
                    }
                }

                MADFILE = ModelObject.recalculateOffsets(MADFILE);
                ModelObject.SaveMadFile(MADFILE, madfileName);  
            }
            MessageBox.Show("the tool have added " + added + " new object in all the maps");
            */
        }
    }

    class MapObjectsListViewItem
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Team { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectBlya
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>


    #region Logic
    enum PlaneType
    {
        Airbus_A220 = 0,
        Airbus_A310 = 1,
        Airbus_A320,
        Airbus_A330,
        Airbus_A340,
        Airbus_A350,
        Airbus_A380,
        Boeing_717,
        Boeing_737,
        Boeing_747,
        Boeing_757,
        Boeing_767,
        Boeing_777,
        Boeing_787,
        ATR_42_72,
        BAe_Avro_RJ,
        Bombardier_Dash_8,
        Bombardier_CRJ,
        Embraer_ERJ,
        Embraer_170_190,
        Saab,
        SUPERJET_100,
        Typolev_Ty_204,
        Ilushin_Il_96,
        Ilushin_Il_114,
        Antonov_An_38,
        Antonov_An_140,
        Antonov_An_148,
        Comac_ARJ21,
        Comac_C919,
        CRAIC_CR929,
        Harbin_Y_12,
        Xian_MA60,
        //invalidType //если указано выше 32 или ниже 0
    }

    [Serializable]
    class Flight
    {
        [JsonProperty("Base_id")]
        public int base_id { get; set; } //id в базе данных

        [JsonProperty("flight_id")]
        public string Id { get; set; } //id рейса

        [JsonProperty("type")]
        public PlaneType Type { get; set; } //тип самолета

        [JsonProperty("date")]
        public DateTime Date { get; set; } //дата совершения рейса

        [JsonProperty("status")]
        public int Status { get; set; } //оценка качества самолета

        [JsonProperty("onboard_passangers")]
        public int Passengers { get; set; } //кол-во пассажиров на самолете

        [JsonProperty("lethals")]
        public int Deaths { get; set; } //смертей во время перелета

        public override string ToString() => '#' + Id + ", Type = " + Type +
            ", Day = " + Date.Day + ", Status = " + Status +
            ", Passengers = " + Passengers + ", RIP = " + Deaths;
    }

    class ParkInfo
    {
        [JsonProperty("type")]
        public PlaneType Type { get; set; }

        [JsonProperty("seats")] //кол-во вмещаемых людей в 1 самолете
        public int Seats { get; set; }

        [JsonProperty("ammount")] //всего этой модели
        public int Count { get; set; }

        public override string ToString() => $"Type = {Type} Seats = {Seats} Count = {Count}";
    }

    class Filter
    {
        public Filter(LinkedList<Flight> _arr, LinkedList<ParkInfo> _list)
        {
            array = _arr;
            list = _list;
        }

        /// <summary>
        /// Sorts by days, when false returns least deadliest days
        /// </summary>
        /// <param name="Descending">When false => least deadliest days </param>
        /// <returns></returns>
        public Dictionary<int, int> DeadliestDays(bool Descending = true)
        {

            Dictionary<int, int> top_days = new Dictionary<int, int>(); //от самых смертельных дней

            foreach (var item in array.Distinct().OrderByDescending(x => x.Deaths))
                if (!top_days.ContainsKey(item.Date.Day))
                    top_days.Add(item.Date.Day, array.Where(x => x.Date.Day == item.Date.Day).Sum(x => x.Deaths));

            if (!Descending) top_days = top_days.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            return top_days;
        }


        /// <summary>
        /// Sorts by days, when false returns worst models
        /// </summary>
        /// <param name="Descending">When false => worst models </param>
        /// <returns></returns>
        public Dictionary<PlaneType, int> BestModels(bool Descending = true)
        {
            Dictionary<PlaneType, int> quality = new Dictionary<PlaneType, int>(); //от самых хороших моделей до, самых плохих

            foreach (var item in array.Distinct().OrderByDescending(x => x.Status))
                if (!quality.ContainsKey(item.Type))
                {
                    int sum = array.Where(x => x.Type == item.Type).Sum(x => x.Status);
                    int ammount = array.Where(x => x.Type == item.Type).Count();
                    quality.Add(item.Type, (sum / ammount)); //по среднем значению
                }

            if (!Descending) quality = quality.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            return quality;
        }

        /// <summary>
        /// Sorts by days, when false returns most breakable days
        /// </summary>
        /// <param name="Descending">When false => most breakable days </param>
        /// <returns></returns>
        public Dictionary<int, int> LeastBreakableDays(bool Descending = true)
        {
            Dictionary<int, int> breakable = new Dictionary<int, int>(); //дни где меньше всего поломок
            foreach (var item in array.Distinct().OrderByDescending(x => x.Status))
                if (!breakable.ContainsKey(item.Date.Day))
                    breakable.Add(item.Date.Day, array.Where(x => x.Date.Day == item.Date.Day).Sum(x => x.Status));

            if (!Descending) breakable = breakable.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            return breakable;
        }

        /// <summary>
        /// Predict how much we need to reserve planes for flight
        /// </summary>
        /// <param name="type">Type of desired plane</param>
        /// <param name="day">Day when flight will occur</param>
        /// <param name="needSeats">How many passengers bouhght ticket</param>
        /// <returns>if -1 means we don't have planes in the park for flight</returns>
        public int Ammount(PlaneType type, int day, int needSeats)
        {
            Dictionary<int, Probe> pairs = new Dictionary<int, Probe>(); //day
            int index = -1;
            foreach (var item in array.Distinct().OrderByDescending(x => x.Date.Day))
            {
                index++;
                int ammount = array.Where(x => x.Type == item.Type && x.Date.Day == item.Date.Day).Count();
                int normal = array.Where(x => x.Type == item.Type && x.Date.Day == item.Date.Day && x.Status > 6).Count(); //кол-во не сломанных

                if (!pairs.ContainsKey(index))
                    pairs.Add(index, new Probe() { Type = item.Type, Day = item.Date.Day, Chance = (double)normal / ammount }); //вероятность, что попадется нормальный самолет            
            }

            int sum = 0;
            double ans = 0;
            var flag = list.FirstOrDefault(x => x.Type == type);
            if (flag == null)
                return -1;
            for (int i = 0; i < flag.Count; i++)
            {
                if (sum >= needSeats)
                    break;

                sum += flag.Seats;
                ans++;
            }

            var matchedDays = pairs.Where(x => x.Value.Day == day && x.Value.Type == type);

            if (!matchedDays.Any())
                return -1; //если меньше 0, то мы ничего не нашли           
            else
            {
                ans /= (matchedDays.First().Value.Chance != 0) ? matchedDays.First().Value.Chance : 0.05;
                return Convert.ToInt32(Math.Ceiling(ans));
            }
        }

        public LinkedList<ParkInfo> list;
        public LinkedList<Flight> array;
    }

    class Probe
    {
        public int Day { get; set; }
        public double Chance { get; set; }
        public PlaneType Type { get; set; }

        public override string ToString() => $"{Type.ToString()}, {Day.ToString()}, {Chance.ToString()}";
    }
    #endregion

    class CRUD
    {
        public CRUD(LinkedList<Flight> _array, string _DB = "event.json") { array = _array; DB = _DB; }
        public CRUD(string _DB = "event.json") { array = new LinkedList<Flight>(); DB = _DB; }

        /// <summary>
        /// Create item in json file
        /// </summary>
        /// <param name="fl">Type Flight</param>
        /// <returns></returns>
        public LinkedList<Flight> Create(Flight fl)
        {
            fl.base_id = array.Count + 1;

            if (File.Exists(DB))
            {
                array = Read();
                array.AddLast(fl);
                File.WriteAllText(DB, string.Empty);
                File.WriteAllText(DB, JsonConvert.SerializeObject(array));
            }
            else using (StreamWriter sw = File.CreateText(DB))
                {
                    array.AddLast(fl);
                    sw.Write(JsonConvert.SerializeObject(array));
                    sw.Close();
                }

            return array;
        }

        /// <summary>
        /// Read json file
        /// </summary>
        /// <returns>LinkedList<Flight>array</returns>
        public LinkedList<Flight> Read()
        {
            if (File.Exists(DB))
                return JsonConvert.DeserializeObject<LinkedList<Flight>>(File.ReadAllText(DB));

            else return array;
        }

        /// <summary>
        /// Update value which goes by index.
        /// </summary>
        /// <param name="index">index of value</param>
        /// <param name="selector">id, type, date, status, passangers, rip</param>
        /// <returns>LinkedList<Flight>array</returns>
        public LinkedList<Flight> Update(int index, string selector)
        {
            if (File.Exists(DB))
            {
                if (array.Count == 0) return array;

                array = Read();
                var item = array.Where(x => x.base_id.Equals(index)).First();
                switch (selector)
                {
                    case "id":
                        Console.Write("Change flight id: ");
                        array.Find(item).Value.Id = Console.ReadLine();
                        break;
                    case "type":
                        {
                            var read = Console.ReadLine();
                            if (Enum.TryParse(read, out PlaneType type))
                                if (Enum.IsDefined(typeof(PlaneType), type) | type.ToString().Contains(","))
                                {
                                    array.Find(item).Value.Type = type;
                                }

                                else
                                    Console.WriteLine("{0} is not an underlying value of the types enumeration.", read);
                            else
                                Console.WriteLine("{0} is not a member of the types enumeration.", read);
                        }
                        break;
                    case "date":
                        Console.Write("Change date (exmpl: 2008,06,25): ");
                        var values = Console.ReadLine().Split(',');
                        array.Find(item).Value.Date = new DateTime(Convert.ToInt32(values[0]), Convert.ToInt32(values[1]), Convert.ToInt32(values[2]));
                        break;
                    case "status":
                        {
                            again:
                            Console.Write("Change status of the fluight: ");
                            var temp = Convert.ToInt32(Console.ReadLine());
                            if (temp < 0 || temp > 10)
                            {
                                Console.WriteLine("Error value can be only in range 0-10! Try again!");
                                goto again;
                            }
                            else array.Find(item).Value.Status = temp;
                        }
                        break;
                    case "passangers":
                        dela:
                        Console.Write("Enter new ammount of passangers: ");
                        var sel = Convert.ToInt32(Console.ReadLine());
                        if (sel < 0)
                        {
                            Console.WriteLine("Ammount can`t be negative! Try again!");
                            goto dela;
                        }
                        else array.Find(item).Value.Passengers = sel;
                        break;
                    case "rip":
                        Console.Write("Enter new ammount of casualties: ");
                        var sele = Convert.ToInt32(Console.ReadLine());
                        if (sele < 0)
                        {
                            Console.WriteLine("Casualties can`t be negative! Try again!");
                            goto dela;
                        }
                        else array.Find(item).Value.Deaths = sele;
                        break;
                    default:
                        break;
                }
                File.WriteAllText(DB, string.Empty); //empty file
                File.WriteAllText(DB, JsonConvert.SerializeObject(array));
            }
            return array;
        }

        /// <summary>
        /// Delets item in json by uniq index 
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns></returns>
        public LinkedList<Flight> Delete(int index)
        {
            if (File.Exists(DB))
            {
                //if (array.Count == 0) return array;

                array = Read();
                var item = array.Where(x => x.base_id.Equals(index)).First();
                array.Remove(item); // remove object from array
                File.WriteAllText(DB, string.Empty); //empty file
                File.WriteAllText(DB, JsonConvert.SerializeObject(array));
            }
            return array;
        }


        public LinkedList<Flight> array;
        public string DB;
    }

    public partial class MainWindow : Window
    {
        ImageSource full = new BitmapImage(new Uri("pack://application:,,,/ProjectBlya;component/Full.png"));
        ImageSource half = new BitmapImage(new Uri("pack://application:,,,/ProjectBlya;component/Half.png"));
        ImageSource empty = new BitmapImage(new Uri("pack://application:,,,/ProjectBlya;component/Empty.png"));
        public MainWindow()
        {
            InitializeComponent();
            
            LinkedList<Flight> flights = new LinkedList<Flight>(new CRUD().Read());
            LinkedList<ParkInfo> list = new LinkedList<ParkInfo>(JsonConvert.DeserializeObject<LinkedList<ParkInfo>>(File.ReadAllText("park.json")));

            flights.AddLast(new Flight() { Id = "NP-24", Type = (PlaneType)22, base_id = 0, Date = new DateTime(2009, 12, 22), Deaths = 2, Passengers = 156, Status = 8 });
            
            LinkedList<Card> cards = new LinkedList<Card>();
            foreach (var item in flights)
            {
                Card temp1 = new Card();
                temp1.flightId.Text = item.Id;
                temp1.typeName.Text = item.Type.ToString();
                temp1.Date.Text = item.Date.ToString("dd.MM.yyyy");
                temp1.rating.Text = ((double)item.Status / 2).ToString();
                temp1.desription.Text = $"This flight contained {item.Passengers} on board.\nDuring whole trip was {item.Deaths} casulties";
                cards.AddLast(temp1);
                var key = temp1.rating.Text;
                switch (key)
                {
                    case "5":
                        {
                            temp1.StarOne.Source = full;
                            temp1.StarTwo.Source = full;
                            temp1.StarThree.Source = full;
                            temp1.StarFour.Source = full;
                            temp1.StarFive.Source = full;
                        }
                        break;
                    case "4,5":
                        {
                            temp1.StarOne.Source = full;
                            temp1.StarTwo.Source = full;
                            temp1.StarThree.Source = full;
                            temp1.StarFour.Source = full;
                            temp1.StarFive.Source = half;
                        }
                        break;
                    case "4":
                        {
                            temp1.StarOne.Source = full;
                            temp1.StarTwo.Source = full;
                            temp1.StarThree.Source = full;
                            temp1.StarFour.Source = full;
                            temp1.StarFive.Source = empty;
                        }
                        break;
                    case "3,5":
                        {
                            temp1.StarOne.Source = full;
                            temp1.StarTwo.Source = full;
                            temp1.StarThree.Source = full;
                            temp1.StarFour.Source = half;
                            temp1.StarFive.Source = empty;
                        }
                        break;
                    case "3":
                        {
                            temp1.StarOne.Source = full;
                            temp1.StarTwo.Source = full;
                            temp1.StarThree.Source = full;
                            temp1.StarFour.Source = empty;
                            temp1.StarFive.Source = empty;
                        }
                        break;
                    case "2,5":
                        {
                            temp1.StarOne.Source = full;
                            temp1.StarTwo.Source = full;
                            temp1.StarThree.Source = half;
                            temp1.StarFour.Source = empty;
                            temp1.StarFive.Source = empty;
                        }
                        break;
                    case "2":
                        {
                            temp1.StarOne.Source = full;
                            temp1.StarTwo.Source = full;
                            temp1.StarThree.Source = empty;
                            temp1.StarFour.Source = empty;
                            temp1.StarFive.Source = empty;
                        }
                        break;
                    case "1,5":
                        {
                            temp1.StarOne.Source = full;
                            temp1.StarTwo.Source = half;
                            temp1.StarThree.Source = empty;
                            temp1.StarFour.Source = empty;
                            temp1.StarFive.Source = empty;
                        }
                        break;
                    case "1":
                        {
                            temp1.StarOne.Source = full;
                            temp1.StarTwo.Source = empty;
                            temp1.StarThree.Source = empty;
                            temp1.StarFour.Source = empty;
                            temp1.StarFive.Source = empty;
                        }
                        break;
                    case "0,5":
                        {
                            temp1.StarOne.Source = half;
                            temp1.StarTwo.Source = empty;
                            temp1.StarThree.Source = empty;
                            temp1.StarFour.Source = empty;
                            temp1.StarFive.Source = empty;
                        }
                        break;
                    case "0":
                        {
                            temp1.StarOne.Source = empty;
                            temp1.StarTwo.Source = empty;
                            temp1.StarThree.Source = empty;
                            temp1.StarFour.Source = empty;
                            temp1.StarFive.Source = empty;
                        }
                        break;
                    default:
                        break;
                }

                ContentGrid.Children.Add(temp1);
            }


         

           

            
        }

        private void CRUD_Click(object sender, RoutedEventArgs e)
        {
            CRUD button = new CRUD();
            Flight fl = new Flight();

            //cards = button.Create(fl);
        }
    }
}

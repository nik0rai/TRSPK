using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp7
{
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

            Dictionary<int, int> top_days = new(); //от самых смертельных дней

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
            Dictionary<PlaneType, int> quality = new(); //от самых хороших моделей до, самых плохих

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
            Dictionary<int, int> breakable = new(); //дни где меньше всего поломок
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
            Dictionary<int, Probe> pairs = new(); //day
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
        public CRUD (string _DB = "event.json") { array = new(); DB = _DB; }

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

    class Program
    {
        static void Help()
        {
            Console.WriteLine("0: Top deadliest days." +
                              "\n1: Top safiest days." +
                              "\n2: Top best models." +
                              "\n3: Top worst models." +
                              "\n4: Top breakable days." +
                              "\n5: Top not breakable days." +
                              "\n6: How much company should reserve planes for specified flight." +
                              "\n7: Exit." +
                              "\n---------------------------------------------------------------\nCRUD:" +
                              "\n101: Read from databases." +
                              "\n102: Add flight to database." +
                              "\n103: Delete flight from database." +
                              "\n104: Update flight in database.");
        }

        static void Main()
        {
            bool isRunning = true;
            LinkedList<Flight> flights = new(new CRUD().Read());
            LinkedList<ParkInfo> list = JsonConvert.DeserializeObject<LinkedList<ParkInfo>>(File.ReadAllText("park.json"));            
            Filter filter = new(flights, list);

            Help();
            do
            {
                Console.Write(">> "); int selected = Convert.ToInt32(Console.ReadLine());
                switch (selected)
                {
                    case 0:
                        {
                            if (flights.Count != 0)
                            {
                                Console.WriteLine("\nTop deadliest days:\n---------------------------");
                                var a = filter.DeadliestDays();
                                foreach (var item in a)
                                    Console.WriteLine($"{item.Key} Deaths: {item.Value}");
                                Console.WriteLine("---------------------------");
                            }
                            else Console.WriteLine("Empty!");
                        }
                        break;
                    case 1:
                        {
                            if (flights.Count != 0)
                            {
                                Console.WriteLine("\nTop safiest days:\n---------------------------");
                                var aa = filter.DeadliestDays(false);
                                foreach (var item in aa)
                                    Console.WriteLine($"{item.Key} Deaths: {item.Value}");
                                Console.WriteLine("---------------------------");
                            }
                            else Console.WriteLine("Empty!");
                        }
                        break;
                    case 2:
                        {
                            if (flights.Count != 0)
                            {
                                Console.WriteLine("\nTop best models:\n---------------------------");
                                var bb = filter.BestModels();
                                foreach (var item in bb)
                                    Console.WriteLine(item.Key);
                                Console.WriteLine("---------------------------");
                            }
                            else Console.WriteLine("Empty!");
                        }
                        break;
                    case 3:
                        {
                            if (filter.list.Count != 0)
                            {
                                Console.WriteLine("\nTop worst models:\n---------------------------");
                                var b = filter.BestModels(false);
                                Console.WriteLine(b.Count);
                                foreach (var item in b)
                                    Console.WriteLine(item.Key);
                                Console.WriteLine("---------------------------");
                            }
                            else Console.WriteLine("Empty!");
                        }
                        break;
                    case 4:
                        {
                            if (flights.Count != 0)
                            {
                                Console.WriteLine("\nMost breakable days:\n---------------------------");
                                var cc = filter.LeastBreakableDays(false);
                                foreach (var item in cc)
                                    Console.WriteLine(item.Key);
                                Console.WriteLine("---------------------------");
                            }
                            else Console.WriteLine("Empty!");
                        }
                        break;
                    case 5:
                        {
                            if (flights.Count != 0)
                            {
                                Console.WriteLine("\nLeast breakable days:\n---------------------------");
                                var c = filter.LeastBreakableDays();
                                foreach (var item in c)
                                    Console.WriteLine(item.Key);
                                Console.WriteLine("---------------------------");
                            }
                            else Console.WriteLine("Empty!");
                        }
                        break;
                    case 6:
                        {
                            if (flights.Count != 0)
                            {
                                Console.WriteLine("\nAll types of planes:\n---------------------------");
                                foreach (var item in Enum.GetValues(typeof(PlaneType)))
                                    Console.WriteLine(item);
                                Console.WriteLine("---------------------------");

                                Console.Write("Enter desiered model: "); var read = Console.ReadLine();
                                Console.Write("Enter day of flight: "); int day = Convert.ToInt32(Console.ReadLine());
                                Console.Write("Enter how much passengers will be on flight: "); int amm = Convert.ToInt32(Console.ReadLine());
                                if (Enum.TryParse(read, out PlaneType type))
                                    if (Enum.IsDefined(typeof(PlaneType), type) | type.ToString().Contains(","))
                                    {
                                        var ans = filter.Ammount(type, day, amm);
                                        if (ans == -1)
                                            Console.WriteLine("We cannot find any models to reserve from the park. Maybe we don`t have this model.");
                                        else
                                            Console.WriteLine($"You should reserve: {ans}");
                                    }

                                    else
                                        Console.WriteLine("{0} is not an underlying value of the types enumeration.", read);
                                else
                                    Console.WriteLine("{0} is not a member of the types enumeration.", read);
                            }
                            else Console.WriteLine("Empty!");
                        }
                        break;
                    case 7:
                        {
                            retey:
                            Console.Write("Are you shure you want to exit? (y/n): ");
                            char desire = Convert.ToChar(Console.ReadLine());
                            switch (desire)
                            {
                                case 'y':
                                    Console.WriteLine("Goodbye!");
                                    isRunning = false;
                                    break;
                                case 'n':
                                    break;
                                default:
                                    Console.WriteLine("Wrong command try again!"); goto retey;
                                    break;
                            }
                        }
                        break;
                    default:
                        Console.WriteLine("Wrong command!");
                        break;
                    case 101: //read db
                        {
                            flights = new CRUD().Read();
                            list = JsonConvert.DeserializeObject<LinkedList<ParkInfo>>(File.ReadAllText("park.json"));
                            filter = new(flights, list);

                            if (flights.Count == 0 || list.Count == 0) { Console.WriteLine("Empty!"); break; }

                            Console.WriteLine("\nFlights info:\n--------------------\n");
                            foreach (var item in flights)
                                Console.WriteLine(item);
                            Console.WriteLine("--------------------");

                            Console.WriteLine("\nPark info:\n--------------------\n");
                            foreach (var item2 in list)
                                Console.WriteLine(item2);
                            Console.WriteLine("--------------------");
                        }
                        break;
                    case 102: //create in db
                        {
                            CRUD crud = new();

                            Console.Write("Enter flight id (example: NP-123): "); string id = Console.ReadLine();

                            Console.WriteLine("\nAll types of planes:\n---------------------------");
                            foreach (var item in Enum.GetValues(typeof(PlaneType)))
                                Console.WriteLine(item);
                            Console.WriteLine("---------------------------");

                            Console.Write("Enter model of plane: ");
                            var read = Console.ReadLine();
                            if (!Enum.TryParse(read, out PlaneType type))
                                Console.WriteLine("{0} is not a member of the types enumeration.", read);
                            else
                            {
                                if (Enum.IsDefined(typeof(PlaneType), type) | type.ToString().Contains(","))
                                {
                                    //
                                }
                                else
                                    Console.WriteLine("{0} is not an underlying value of the types enumeration.", read);
                            }


                            Console.Write("Enter date (example: 2008,02,22): ");
                            var s = Console.ReadLine().Split(',');
                            DateTime date = new(Convert.ToInt32(s[0]), Convert.ToInt32(s[1]), Convert.ToInt32(s[2]));

                            Console.Write("Give rating (example: from 0 to 10): ");
                            int status = Convert.ToInt32(Console.ReadLine());

                            Console.Write("Enter ammount of passangers boarded: ");
                            int passengers = Convert.ToInt32(Console.ReadLine());

                            Console.Write("Enter ammount of passangers casualties: ");
                            int rip = Convert.ToInt32(Console.ReadLine());

                            Flight flight = new() { Id = id, Type = type, Date = date, Passengers = passengers, Status = status, Deaths = rip, base_id = flights.Count };

                            flights = crud.Create(flight);
                            filter = new(flights, list);
                        }
                        break;
                    case 103: //delete from db
                        {
                            CRUD crude = new();
                            again:
                            Console.Write($"Enter which flight do you want to delete: (example: 1 - {flights.Count}): ");
                            int val = Convert.ToInt32(Console.ReadLine());
                            if (val < flights.Count || val > 0)
                                flights = crude.Delete(val);
                            else { Console.WriteLine($"Wrong position! Enter propper value (example: 1 - {flights.Count}): "); goto again; }

                          
                            filter = new(flights, list);

                            foreach (var item in filter.array)
                            {
                                Console.WriteLine(item);
                            }
                        }
                        break;
                    case 104: //update in db
                        {
                            CRUD hm = new();
                            Console.WriteLine("Enter id: ");
                            int key = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("What do you want to update:" + 
                                "\nid: Flight id" + 
                                "\ntype: Model of plane" + 
                                "\nstatus: Status of plane" + 
                                "\npassangers: Update ammount of passangers on the board" + 
                                "\nrip: Update ammount of casualties");
                            gogo:
                            string help = Console.ReadLine();
                            if (string.IsNullOrEmpty(help) || string.IsNullOrWhiteSpace(help))
                            {
                                Console.WriteLine("Value cannot be empty or white space!");
                                goto gogo;
                            }
                            else { flights = hm.Update(key, help); filter = new(flights, list); }
                        }
                        break;
                    case 1337:
                        Console.Clear();
                        Help();
                        break;
                }
            } while (isRunning);
        }
    }
}

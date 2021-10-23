using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TrainCsharp
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
        public int Passangers { get; set; } //кол-во пассажиров на самолете

        [JsonProperty("lethals")]
        public int Deaths { get; set; } //смертей во время перелета   

        public override string ToString() => '#' + Id + ", Type = " + Type +
            ", Day = " + Date.Day + ", Status = " + Status +
            ", Passengers = " + Passangers + ", RIP = " + Deaths;       
    }

    class ParkInfo
    {
        [JsonProperty("type")]
        public PlaneType Type { get; set; }

        [JsonProperty("seats")] //кол-во вмещаемых людей в 1 самолете
        public int Seats { get; set; }

        [JsonProperty("ammount")] //всего этой модели
        public int Count { get; set; }
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
        public Dictionary<int, int> DeadliestDays(bool Descending = true) {

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
        public Dictionary<int,int> LeastBreakableDays(bool Descending = true)
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

        private readonly LinkedList<ParkInfo> list;
        private readonly LinkedList<Flight> array;
    }

    class Probe
    {
        public int Day { get; set; }
        public double Chance { get; set; }
        public PlaneType Type { get; set; }

        public override string ToString() => $"{Type.ToString()}, {Day.ToString()}, {Chance.ToString()}";
    }
    #endregion

    #region CRUD
    class CRUD<T>
    {
        public CRUD(string db = "events.json") => DB = db;
        public LinkedList<T> Read()
        {
            LinkedList<T> Next = new();
            if (File.Exists(DB))
            {
                string reader = File.ReadAllText(DB);
                Next = JsonConvert.DeserializeObject<LinkedList<T>>(reader);
                
            }
            return Next;
        }

        public void Add(Flight Next)
        {
            JsonSerializer serializer = new();

            if (!File.Exists(DB))
            {
                var file = File.Create(DB);
                file.Close();
            }
            using StreamReader streamReader = new(DB);

            string iter = streamReader.ReadLine();
            bool first = (iter != null) ? iter[0].Equals('[') : false;

            string secondIter = streamReader.ReadToEnd();
            bool last = (secondIter.Length != 0) ? secondIter[^1].Equals(']') : false;

            streamReader.Close();

            int i = 0;
            using (StreamReader sr = new(DB))
            {
                String line;
                while ((line = sr.ReadLine()) != null)
                    i++;

            }

            string[] tempStr = File.ReadAllLines(DB);

            if (i == 0)
                Next.base_id = i + 1;
            else
                Next.base_id = i - 1;

            if (!first && !last)
            {
                StreamWriter text = new StreamWriter(DB, true);
                text.WriteLine('[');
                using JsonWriter writer = new JsonTextWriter(text);
                serializer.Serialize(writer, Next);
                text.WriteLine(',');
                text.Write(']');
                text.Close();
            }

            if (last)
            {
                string[] readText = File.ReadAllLines(DB);
                string[] writeText = new string[readText.Length - 1];
                Array.Copy(readText, 0, writeText, 0, readText.Length - 1);
                File.WriteAllLines(DB, writeText);

                StreamWriter text = new(DB, true);
                using JsonWriter writer = new JsonTextWriter(text);
                serializer.Serialize(writer, Next);
                text.WriteLine(',');
                text.Write(']');
                text.Close();
            }
        }

        public void Delete(int num)
        {                  
            string str = "";

            if (!File.Exists(DB + "help.json"))
            {
                var file = File.Create(DB + "help.json");
                file.Close();
            }

            string[] readText = File.ReadAllLines(DB);
            string[] writeText = new string[num];
            Array.Copy(readText, 0, writeText, 0, num);
            File.WriteAllLines(DB + "help.json", writeText);

            for (int i = 0; i <= num; i++)
            {
                using (StreamReader sr = new(@DB))
                {
                    sr.ReadLine(); //читаем первую строку
                    str = sr.ReadToEnd(); //записываем все остальные строки
                }
                using (StreamWriter sw = new(@DB))
                {
                    sw.WriteLine(str); //перезаписываем содержимое фала
                }
            }

            using (StreamReader sr = new(DB))
            {
                str = sr.ReadToEnd();
            }
            using (StreamWriter swrite = new(DB + "help.json", true))
            {
                swrite.WriteLine(str);
            }

            using (StreamReader sr = new(DB + "help.json"))
            {
                str = sr.ReadToEnd();
            }
            using (StreamWriter sw = new(DB))
            {
                sw.WriteLine(str);
            }
        }

        public void Update(int k, string help)
        {
            if (!File.Exists(DB))
            {
                Console.WriteLine("У вас нет событий"); 
            }

            Console.WriteLine("Какое событие вы хотите редактировать?");
            int num = Convert.ToInt32(Console.ReadLine());
            string str = "";

            if (!File.Exists(DB + "help2.json"))
            {
                var file = File.Create(DB + "help2.json");
                file.Close();
            }

            if (!File.Exists(DB + "edit.json"))
            {
                var file = File.Create(DB + "edit.json");
                file.Close();
            }

            string[] readText = File.ReadAllLines(DB);                      //
            string[] writeText = new string[num];                           //  Сохранение текста до num-строки
            Array.Copy(readText, 0, writeText, 0, num);                     //
            File.WriteAllLines(DB + "help2.json", writeText);               //

            for (int i = 0; i < num; i++)
            {
                using (StreamReader sr = new StreamReader(DB))
                {
                    sr.ReadLine();
                    str = sr.ReadToEnd();
                }
                using (StreamWriter sw = new StreamWriter(DB))
                {
                    sw.WriteLine(str);
                }
            }

            //Переносим редактируемую строку в другой файл
            using (StreamReader sreader = new StreamReader(DB))
            {
                str = sreader.ReadLine();
            }
            using (StreamWriter swriter = new StreamWriter(DB + "edit.json"))
            {
                swriter.Write(str);
            }

            using (StreamReader sreader = new StreamReader(DB + "edit.json"))
            {
                str = sreader.ReadLine()[0..^1]; //Создать подстроку от 0 до length-1(^1 ==> length - 1)
                //Console.WriteLine(str);
            }
            using (StreamWriter swriter = new StreamWriter(DB + "edit.json"))
            {
                swriter.Write(str);
            }


            /*string[] readText1 = File.ReadAllLines(@"jsonedit.txt");
            string[] writeText1 = new string[readText.Length - 1];
            Array.Copy(readText1, 0, writeText1, 0, readText1.Length - 1);
            File.WriteAllLines(@"jsonedit.txt", writeText1);*/


            string json = File.ReadAllText(DB + "edit.json");
            dynamic jsonObj = JsonConvert.DeserializeObject(json);

            switch (k)
            {
                case 1:                  
                    jsonObj["flight_id"] = help;
                    string output = JsonConvert.SerializeObject(jsonObj);
                    File.WriteAllText(DB + "edit.json", output);
                    break;
                case 2:                  
                    jsonObj["type"] = help;
                    string output1 = JsonConvert.SerializeObject(jsonObj);
                    File.WriteAllText(DB + "edit.json", output1);
                    break;
                case 3:                   
                    string help1 = Console.ReadLine();
                    string help2 = Console.ReadLine();
                    jsonObj["date"] = help + "-" + help1 + "-" + help2 + "T00:00:00";
                    string output2 = JsonConvert.SerializeObject(jsonObj);
                    File.WriteAllText(DB + "edit.json", output2);
                    break;
                case 4:                   
                    jsonObj["status"] = help;
                    string output3 = JsonConvert.SerializeObject(jsonObj);
                    File.WriteAllText(DB + "edit.json", output3);
                    break;
                case 5:                
                    jsonObj["onboard_passangers"] = help;
                    string output4 = JsonConvert.SerializeObject(jsonObj);
                    File.WriteAllText(DB + "edit.json", output4);
                    break;
                case 6:                  
                    jsonObj["lethals"] = help;
                    string output5 = JsonConvert.SerializeObject(jsonObj);
                    File.WriteAllText(DB + "edit.json", output5);
                    break;
                default:                   
                    break;
            }

            using (StreamReader sr = new StreamReader(DB + "edit.json"))
            {
                str = sr.ReadLine();
            }
            using (StreamWriter sw = new StreamWriter(DB + "help2.json", true))
            {
                sw.WriteLine(str + ',');
            }


            using (StreamReader sr = new StreamReader(DB))
            {
                sr.ReadLine(); //читаем первую строку
                str = sr.ReadToEnd(); //записываем все остальные строки
            }
            using (StreamWriter sw = new StreamWriter(DB + "help2.json", true))
            {
                sw.WriteLine(str); //перезаписываем содержимое фала
            }

            using (StreamReader sr = new StreamReader(DB + "help2.json"))
            {
                str = sr.ReadToEnd();
            }
            using (StreamWriter sw = new StreamWriter(DB))
            {
                sw.WriteLine(str);
            }

        }

        private readonly string DB;
    }
    #endregion

    class Program
    {
        static void Help()
        {
            Console.WriteLine("0: Top deadliest days." +
                              "\n1: Top safiest days." +
                              "\n2: Top best models."  +
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
            LinkedList<Flight> flights = new(new CRUD<Flight>().Read());
            LinkedList<ParkInfo> list = new(new CRUD<ParkInfo>("park.json").Read());

            bool isRunning = true;
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
                            if (flights.Count != 0)
                            {
                                Console.WriteLine("\nTop worst models:\n---------------------------");
                                var b = filter.BestModels(false);
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
                                    Console.WriteLine(item);
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
                                    Console.WriteLine(item);
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
                                Console.WriteLine("Enter day of flight: "); int day = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine("Enter how much passengers will be on flight: "); int amm = Convert.ToInt32(Console.ReadLine());
                                PlaneType type;
                                if (Enum.TryParse(read, out type))
                                    if (Enum.IsDefined(typeof(PlaneType), type) | type.ToString().Contains(","))
                                    {
                                        var ans = filter.Ammount(type, day, amm);
                                        if (ans == -1)
                                            Console.WriteLine("We cannot find any models to reserve from the park. Maybe we don`t have this model.");
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
                    default: Console.WriteLine("Wrong command!");
                        break;
                    case 101: //read db
                        {
                            flights = new CRUD<Flight>().Read();
                            list = new CRUD<ParkInfo>("park.json").Read();
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
                            CRUD<Flight> crud = new();

                            Console.Write("Enter flight id (example: NP-123): ");
                            string id = Console.ReadLine();


                            Console.Write("Enter model of plane (example: от 0 до 32): ");
                            var read = Console.ReadLine();
                            PlaneType type;
                            if (Enum.TryParse(read, out type))
                            {
                                if (Enum.IsDefined(typeof(PlaneType), type) | type.ToString().Contains(","))
                                {
                                    continue;
                                }
                                else
                                    Console.WriteLine("{0} is not an underlying value of the types enumeration.", read);
                            }
                            else Console.WriteLine("{0} is not a member of the types enumeration.", read);


                            Console.Write("Enter date (example: year -> moth -> day): ");
                            DateTime date = new(Convert.ToInt32(Console.ReadLine()), Convert.ToInt32(Console.ReadLine()), Convert.ToInt32(Console.ReadLine()));

                            Console.WriteLine("Give rating (example: from 0 to 10): ");
                            int status = Convert.ToInt32(Console.ReadLine());

                            Console.WriteLine("Enter ammount of passangers boarded: ");
                            int passengers = Convert.ToInt32(Console.ReadLine());

                            Console.WriteLine("Enter ammount of passangers casualties: ");
                            int rip = Convert.ToInt32(Console.ReadLine());

                            Flight flight = new() { Id = id, Type = type, Date = date, Passangers = passengers, Status = status, Deaths = rip, base_id = flights.Count };
                            crud.Add(flight);
                            flights = crud.Read();
                        }
                        break;
                    case 103: //delete from db
                        {
                            CRUD<Flight> crude = new();
                            again:
                            Console.WriteLine("Enter which flight do you want to delete: ");
                            int val = Convert.ToInt32(Console.ReadLine());
                            if (val < flights.Count - 1 && val > 0)
                                crude.Delete(val);
                            else { Console.WriteLine($"Wrong position! Enter propper value (example: 0 - {flights.Count - 1}): "); goto again; }
                            flights = crude.Read();
                        }
                        break;
                    case 104: //update in db
                        {
                            CRUD<Flight> hm = new();
                            gogo:
                            Console.WriteLine("Which parameter do you want to change?\n" +
                                              "1.Flight id\n" +
                                              "2.Model of plane\n" +
                                              "3.Date\n" +
                                              "4.Status\n" +
                                              "5.Ammount of passangers\n" +
                                              "6.Ammount of casualties");
                            int k = Convert.ToInt32(Console.ReadLine());
                            string help = "";

                            switch (k)
                            {
                                case 1:
                                    Console.WriteLine("Change flight id: ");
                                    help = Console.ReadLine();
                                    break;
                                case 2:
                                    Console.WriteLine("Change model of plane (номер типа: от 0 до 32): ");
                                    help = Console.ReadLine();
                                    break;
                                case 3:
                                    Console.WriteLine("Change date (год -> месяц -> число: ");
                                    help = Console.ReadLine();
                                    break;
                                case 4:
                                    Console.WriteLine("Change status (по шкале от 0 до 10): ");
                                    help = Console.ReadLine();
                                    break;
                                case 5:
                                    Console.WriteLine("Change ammount of passnagers: ");
                                    help = Console.ReadLine();
                                    break;
                                case 6:
                                    Console.WriteLine("Change ammount of casualties: ");
                                    help = Console.ReadLine();
                                    break;
                                default:
                                    Console.WriteLine("Error command! Try again: "); goto gogo;
                                    break;
                            }

                            if (string.IsNullOrEmpty(help) || string.IsNullOrWhiteSpace(help))
                            {
                                Console.WriteLine("Value cannot be empty or whitespace!");
                                goto gogo;
                            }
                            else { hm.Update(k, help); flights = hm.Read(); }
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
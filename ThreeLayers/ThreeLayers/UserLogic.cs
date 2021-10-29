/// <summary>
/// Слой представления
/// </summary>
using System;

namespace ThreeLayers
{
    class UserLogic
    {
        private BuisnessLogic BuisnessLogic;

        public UserLogic(BuisnessLogic buisnessLogic) => BuisnessLogic = buisnessLogic;

        private static void Help()
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

        public void Run()
        {
            Help();
            bool isRunning = true;           
            do
            {
                Console.Write(">> "); int selected = Convert.ToInt32(Console.ReadLine());
                switch (selected)
                {
                    case 0:
                        {
                            if (BuisnessLogic.GetItems().Count != 0)
                            {
                                Console.WriteLine("\nTop deadliest days:\n---------------------------");
                                var a = BuisnessLogic.DeadliestDays();
                                foreach (var item in a)
                                    Console.WriteLine($"{item.Key} Deaths: {item.Value}");
                                Console.WriteLine("---------------------------");
                            }
                            else Console.WriteLine("Empty!");
                        }
                        break;
                    case 1:
                        {
                            if (BuisnessLogic.GetItems().Count != 0)
                            {
                                Console.WriteLine("\nTop safiest days:\n---------------------------");
                                var aa = BuisnessLogic.DeadliestDays(false);
                                foreach (var item in aa)
                                    Console.WriteLine($"{item.Key} Deaths: {item.Value}");
                                Console.WriteLine("---------------------------");
                            }
                            else Console.WriteLine("Empty!");
                        }
                        break;
                    case 2:
                        {
                            if (BuisnessLogic.GetItems().Count != 0)
                            {
                                Console.WriteLine("\nTop best models:\n---------------------------");
                                var bb = BuisnessLogic.BestModels();
                                foreach (var item in bb)
                                    Console.WriteLine(item.Key);
                                Console.WriteLine("---------------------------");
                            }
                            else Console.WriteLine("Empty!");
                        }
                        break;
                    case 3:
                        {
                            if (BuisnessLogic.GetItems().Count != 0)
                            {
                                Console.WriteLine("\nTop worst models:\n---------------------------");
                                var b = BuisnessLogic.BestModels(false);
                                
                                foreach (var item in b)
                                    Console.WriteLine(item.Key);
                                Console.WriteLine("---------------------------");
                            }
                            else Console.WriteLine("Empty!");
                        }
                        break;
                    case 4:
                        {
                            if (BuisnessLogic.GetItems().Count != 0)
                            {
                                Console.WriteLine("\nMost breakable days:\n---------------------------");
                                var cc = BuisnessLogic.LeastBreakableDays(false);
                                foreach (var item in cc)
                                    Console.WriteLine(item.Key);
                                Console.WriteLine("---------------------------");
                            }
                            else Console.WriteLine("Empty!");
                        }
                        break;
                    case 5:
                        {
                            if (BuisnessLogic.GetItems().Count != 0)
                            {
                                Console.WriteLine("\nLeast breakable days:\n---------------------------");
                                var c = BuisnessLogic.LeastBreakableDays();
                                foreach (var item in c)
                                    Console.WriteLine(item.Key);
                                Console.WriteLine("---------------------------");
                            }
                            else Console.WriteLine("Empty!");
                        }
                        break;
                    case 6:
                        {
                            if (BuisnessLogic.GetItems().Count != 0)
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
                                        var ans = BuisnessLogic.Ammount(type, day, amm);
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
                            if (BuisnessLogic.GetItems().Count == 0) { Console.WriteLine("Empty!"); break; }

                            Console.WriteLine("\nFlights info:\n--------------------\n");
                            foreach (var item in BuisnessLogic.GetItems())
                                Console.WriteLine(item);
                            Console.WriteLine("--------------------");
                        }
                        break;
                    case 102: //create in db
                        {                            
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
                            var s = Console.ReadLine().Split(',', ' ', StringSplitOptions.RemoveEmptyEntries);
                            DateTime date = new(Convert.ToInt32(s[0]), Convert.ToInt32(s[1]), Convert.ToInt32(s[2]));

                            Console.Write("Give rating (example: from 0 to 10): ");
                            int status = Convert.ToInt32(Console.ReadLine());

                            Console.Write("Enter ammount of passangers boarded: ");
                            int passengers = Convert.ToInt32(Console.ReadLine());

                            Console.Write("Enter ammount of passangers casualties: ");
                            int rip = Convert.ToInt32(Console.ReadLine());

                            Item flight = new() { Id = id, Type = type, Date = date, Passengers = passengers, Status = status, Deaths = rip};

                            BuisnessLogic.CreateItem(flight);                            
                        }
                        break;
                    case 103: //delete from db
                        {                           
                            again:
                            Console.Write($"Enter which flight do you want to delete: (example: {BuisnessLogic.GetItems().First.Value.base_id} - {BuisnessLogic.GetItems().Count}): ");
                            int val = Convert.ToInt32(Console.ReadLine());
                            if (val < BuisnessLogic.GetItems().Count || val >= BuisnessLogic.GetItems().First.Value.base_id)
                                BuisnessLogic.DeleteItem(val);
                            else { Console.Write($"Enter which flight do you want to delete: (example: {BuisnessLogic.GetItems().First.Value.base_id} - {BuisnessLogic.GetItems().Count}): "); goto again; }                         
                        }
                        break;
                    case 104: //update in db
                        {                            
                            Console.WriteLine("Enter id: ");
                            int key = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("What do you want to update:" +
                                "\nid: Flight id" +
                                "\ntype: Model of plane" +
                                "\nstatus: Status of plane" +
                                "\npassangers: Update ammount of passangers on the board" +
                                "\nrip: Update ammount of casualties");
                            gogo:
                            string data;
                            string help = Console.ReadLine(); //selector
                            switch (help)
                            {
                                case "id":
                                    Console.Write("Change flight id: ");
                                    data = Console.ReadLine();
                                    try
                                    {
                                        BuisnessLogic.UpdateItem(key, help, data);
                                    }
                                    catch (Exception)
                                    {
                                        Console.WriteLine("Error!");
                                    }
                                    break;
                                case "type":
                                    data = Console.ReadLine();
                                    try
                                    {
                                        BuisnessLogic.UpdateItem(key, help, data);
                                    }
                                    catch (Exception)
                                    {
                                        Console.WriteLine("Error!");
                                    }
                                    break;
                                case "date":
                                    Console.Write("Change date (exmpl: 2008, 06, 25): ");
                                    data = Console.ReadLine();
                                    try
                                    {
                                        BuisnessLogic.UpdateItem(key, help, data);
                                    }
                                    catch (Exception)
                                    {
                                        Console.WriteLine("Error!");
                                    }
                                    break;
                                case "status":
                                    Console.Write("Change status of the flight: ");
                                    data = Console.ReadLine();
                                    try
                                        {
                                            BuisnessLogic.UpdateItem(key, help, data);
                                        }
                                        catch (Exception)
                                        {
                                            Console.WriteLine("Error!");
                                        }                                  
                                    break;
                                case "passangers":                                
                                    Console.Write("Enter new ammount of passangers: ");
                                    data = Console.ReadLine();
                                    try
                                    {
                                        BuisnessLogic.UpdateItem(key, help, data);
                                    }
                                    catch (Exception)
                                    {
                                        Console.WriteLine("Error!");
                                    }
                                    break;
                                case "rip":
                                    Console.Write("Enter new ammount of casualties: ");
                                    data = Console.ReadLine();
                                    try
                                    {
                                        BuisnessLogic.UpdateItem(key, help, data);
                                    }
                                    catch (Exception)
                                    {
                                        Console.WriteLine("Error!");
                                    }
                                    break;
                                default:
                                Console.WriteLine("Value cannot be empty or white space!");
                                goto gogo;
                                    break;
                            }                                                                                
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

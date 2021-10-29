/// <summary>
/// Слой бизнес-логики
/// </summary>
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ThreeLayers
{
    class BuisnessLogic
    {
        private DataLogic DataLogic;

        public BuisnessLogic(DataLogic dataLogic) => DataLogic = dataLogic;

        public LinkedList<Item> GetItems() => DataLogic.Read();

        public void DeleteItem(int id) => DataLogic.Delete(id);

        public void CreateItem(Item item) => DataLogic.Create(item);

        public void UpdateItem(int index, string selector, string data) => DataLogic.Update(index, selector, data);

        public Item GetItem(int id) => DataLogic.GetItem(id);

        /// <summary>
        /// Sorts by days, when false returns least deadliest days
        /// </summary>
        /// <param name="Descending">When false => least deadliest days </param>
        /// <returns></returns>
        public Dictionary<int, int> DeadliestDays(bool Descending = true)
        {

            Dictionary<int, int> top_days = new(); //от самых смертельных дней
            var array = DataLogic.Read();

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
            var array = DataLogic.Read();

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
            var array = DataLogic.Read();

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
            var array = DataLogic.Read();
            if (array == null) return -1;

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


                var list = JsonConvert.DeserializeObject<LinkedList<ParkInfo>>(File.ReadAllText("park.json")); //для него не надо по заданию делать db
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

    }
}
/// <summary>
/// Слой доступа и работы с базой данных
/// </summary>
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace ThreeLayers
{
    class DataLogic
    {
        private readonly string file;
        private int indexer = -1;

        public DataLogic(string _file)
        {
            file = _file;
            indexer = (Read() != null) ? Read().Count : 0;
        }

        public LinkedList<Item> Read()
        {
            LinkedList<Item> items = new();
            try { items = JsonConvert.DeserializeObject<LinkedList<Item>>(File.ReadAllText(file)); }
            catch (Exception)
            {
                var _file = File.Create(file);
                _file.Close();
            }
            return items;
        }

        public Item GetItem(int id)
        {
            if (File.Exists(file))
            {
                StreamReader reader = new(file);
                string text = reader.ReadToEnd();
                reader.Close();

                var pos = Regex.Match(text, "{\"Base_id\":" + id).Index;
                string temp = string.Empty;
                while (text[pos] != '}')
                {
                    temp += text[pos];
                    pos++;
                }
                temp += '}';

                return JsonConvert.DeserializeObject<Item>(temp);
            }
            else throw new ArgumentNullException(nameof(file));
        }

        public void Create(Item item)
        {          
            indexer++;
            item.base_id = (uint)indexer;
            if (File.Exists(file))
            {
                StreamReader reader = new(file);
                string text = reader.ReadToEnd();
                reader.Close();
                text = Regex.Replace(text, "]", ',' + JsonConvert.SerializeObject(item) + ']');
                StreamWriter writer = new(file);
                writer.Write(text);
                writer.Close();
            }
            else
            {
                StreamWriter sw = File.CreateText(file);
                sw.Write('[' + JsonConvert.SerializeObject(item) + ']');
                sw.Close();
            }

        }

        public void Update(int index, string selector, string data)
        {
            if (File.Exists(file))
            {
                var item = GetItem(index);
                var temp = item;
                switch (selector)
                {
                    case "id":
                        item.Id = data;
                        break;
                    case "type":
                        {
                            if (Enum.TryParse(data, out PlaneType type))
                                if (Enum.IsDefined(typeof(PlaneType), type) | type.ToString().Contains(",")) item.Type = type;
                                else throw new ArgumentOutOfRangeException(nameof(data));
                            else throw new TypeLoadException(nameof(data));
                        }
                        break;
                    case "date":
                        var values = data.Split(',', ' ', StringSplitOptions.RemoveEmptyEntries);
                        item.Date = new DateTime(Convert.ToInt32(values[0]), Convert.ToInt32(values[1]), Convert.ToInt32(values[2]));
                        break;
                    case "status":
                        var sts = Convert.ToInt32(data);
                        if (sts < 11 || sts > -1) item.Status = sts; //0..10
                        else throw new ArgumentOutOfRangeException(nameof(data));
                        break;
                    case "passangers":
                        var sel = Convert.ToInt32(data);
                        if (sel < 0) throw new ArgumentOutOfRangeException(nameof(data));
                        else item.Passengers = sel;
                        break;
                    case "rip":
                        var sele = Convert.ToInt32(data);
                        if (sele < 0) throw new ArgumentOutOfRangeException(nameof(data));
                        else item.Deaths = sele;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(selector));
                        break;
                }

                StreamReader reader = new(file);
                string text = reader.ReadToEnd();
                reader.Close();

                text = Regex.Replace(text, JsonConvert.SerializeObject(temp), JsonConvert.SerializeObject(item));

                StreamWriter writer = new(file);
                writer.Write(text);
                writer.Close();
            }
        }

        public void Delete(int index)
        {
            if (File.Exists(file))
            {
                StreamReader reader = new(file);
                string text = reader.ReadToEnd();
                reader.Close();

                var pos = Regex.Match(text, "{\"Base_id\":" + index).Index;
                string temp = string.Empty;
                while (text[pos] != '}')
                {
                    temp += text[pos];
                    pos++;
                }
                temp += '}';

                text = Regex.Replace(text, temp, string.Empty);
                StreamWriter writer = new(file);
                writer.Write(text);
                writer.Close();
            }
        }      
    }
}
 class Program
    {   
        //Вводить только в таком порядке => {a1, a2, a3, a4}, т.е по часовой стрелки начиная с самой верхней левой точки => (1,1,   2,1,    2,0,    1,0)
        //а1(1;1)      а2(2;1)
        //  *———————————*
        //  |           |
        //  |           |
        //  |           |
        //  |           |
        //  *———————————*
        //а4(1;0)      а3(2;0) 
        /// <summary>
        /// Вводить только по часовой стрелки начиная с самой верхней левой точки.
        /// </summary>
        /// <param name="P">Периметр</param>
        /// <param name="S">Площадь</param>
        /// <param name="vs">Координаты</param>
        /// <returns>Если площадь была не найдена, возращает false иначе true.</returns>
        public static bool Geometry(out double P, out double S, params double[] vs) 
        {
            var a = Math.Sqrt(((vs[0] - vs[2]) * (vs[0] - vs[2])) + ((vs[1] - vs[3]) * (vs[1] - vs[3])));
            var b = Math.Sqrt(((vs[2] - vs[4]) * (vs[2] - vs[4])) + ((vs[3] - vs[5]) * (vs[3] - vs[5])));
            var c = Math.Sqrt(((vs[4] - vs[6]) * (vs[4] - vs[6])) + ((vs[5] - vs[7]) * (vs[5] - vs[7])));
            var d = Math.Sqrt(((vs[6] - vs[0]) * (vs[6] - vs[0])) + ((vs[7] - vs[1]) * (vs[7] - vs[1])));


            double a1 = (vs[0] - vs[2]) * (vs[1] + vs[3]);
            double a2 = (vs[2] - vs[4]) * (vs[3] + vs[5]);
            double a3 = (vs[4] - vs[6]) * (vs[5] + vs[7]);
            double a4 = (vs[6] - vs[0]) * (vs[7] + vs[1]);

            S = 0.5 * Math.Abs(a1 + a2 + a3 + a4);
            P = a + b + c + d;

            return S != 0;
        }

        static void Main(string[] args)
        {         
            object a = Geometry(out var p, out var s, 1,1,   2,1,    2,0,    1,0);

            object b = Geometry(out var p1, out var s1, 0, 0, 0, 2, 0, 0, 1, 0);       

            Console.WriteLine("Ploshad: {1} {0}", a, s);
            Console.WriteLine("Perimetr: " + p);            
            Console.WriteLine("Ploshad: {1} {0}", b, s1);
            Console.WriteLine("Perimetr: {0}", p1);
        }       
    }

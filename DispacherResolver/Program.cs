using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispacherResolver
{
    public enum State { NoWork, Ready, Working, ES, Stopped }
    class Ciclo
    {
        public int ciclos,
                   ciclosRestantes;
        public Ciclo(int c)
        {
            ciclos = c;
            ciclosRestantes = c;
        }
        public static Ciclo[] getBase(int[] cs)
        {
            Ciclo[] r = new Ciclo[cs.Length];
            int i = 0;
            foreach (int c in cs)
            {
                r[i] = new Ciclo(c);
                ++i;
            }
            return r;
        }
    }
    class Proceso
    {
        private int arrCyc = 0;
        public int tiempoLlegada;
        public Ciclo[] ciclos;
        public string nombre;
        public State estado;
        public Proceso(string name, int tl, params int[] cs)
        {
            tiempoLlegada = tl;
            ciclos = Ciclo.getBase(cs);
            //duracion = dur;
            nombre = name;
        }
        public string RequestColor(int c)
        {
            //if (acabado)
            //    return GetSColor(State.NoWork);
            /*if (c < tiempoLlegada)
                estado = State.NoWork;
            else
            {// if (c >= tiempoLlegada && estado == State.NoWork && cycLeft > 0)
                if (ciclos.Any(x => x.ciclosRestantes > 0))
                    estado = State.Ready;
                else
                    estado = State.NoWork;
                //estado = State.NoWork;
            }*/
            //if (estado == State.ES && curCycLeft > 0)
            //    --curCycLeft;
            //else
            //    estado = State.Ready;
            //if (estado == State.Ready && !Program.pro.Any(x => x.estado == State.Working))
            //    estado = State.Working;
            if (c >= tiempoLlegada)
            {
                /*if (c - comenzo >= ciclos[arrCyc])
                {
                    if (arrCyc + 1 < ciclos.Length)
                    {
                        ++arrCyc;
                        if (arrCyc % 2 == 0)
                            estado = State.Working;
                        else
                        {
                            estado = State.ES;
                            curCycLeft = ciclos[arrCyc];
                        }
                        --cycLeft;
                    }
                    else
                        estado = State.Stopped;
                }*/
                if (ciclos[arrCyc].ciclosRestantes > 0)
                {
                    if (Program.pro.Any(x => x.estado == State.Working && x != this))
                        estado = State.Ready;
                    if (arrCyc % 2 == 0)
                    {
                        if (!Program.pro.Any(x => x.estado == State.Working && x != this))
                            estado = State.Working;
                    }
                    else
                        estado = State.ES;
                    --ciclos[arrCyc].ciclosRestantes;
                }
                else
                {
                    if (arrCyc + 1 < ciclos.Length)
                        ++arrCyc;
                    else
                        estado = State.Stopped;
                }
            }
            if (estado == State.Stopped)
            {
                estado = State.NoWork;
            }
            return GetSColor(estado);
        }
        static string GetSColor(State st)
        {
            switch (st)
            {
                case State.NoWork:
                    return "<*white*>";
                case State.Ready:
                    return "<*blue*>";
                case State.Working:
                    return "<*green*>";
                case State.ES:
                    return "<*yellow*>";
                case State.Stopped:
                    return "<*magenta*>";
                default:
                    return "<*red*>";
            }
        }
    }
    class Program
    {
        private const int procs = 5,
                          cycles = 35;
        //i = 0,
        //j = 0;
        private static string[] proc = new string[procs];
        public static Proceso[] pro = new Proceso[procs]
        {
            new Proceso("A", 0, 5, 2, 3),
            new Proceso("B", 2, 3),
            new Proceso("C", 4, 7),
            new Proceso("D", 6, 4),
            new Proceso("E", 8, 8)
        };
        private static Random r = new Random();
        static void Main(string[] args)
        {
            for (int i = 0; i < cycles; ++i)
                for (int j = 0; j < procs; ++j)
                    proc[j] += GetColor(i, pro[j]);

            for (int k = 0; k < procs; ++k)
            {
                proc[k] = pro[k].nombre + " " + proc[k];
                WriteLineColor(proc[k]);
            }

            Console.ReadKey();
        }
        static string GetColor(int c, Proceso p)
        {
            //string s = GetSColor((State)r.Next(0, 5));
            return string.Format("{0}x{1}", p.RequestColor(c), "<*/*>");
        }
        static void WriteColor(string str)
        {
            var fgStack = new Stack<ConsoleColor>();
            var bgStack = new Stack<ConsoleColor>();
            var parts = str.Split(new[] { "<*" }, StringSplitOptions.None);
            foreach (var part in parts)
            {
                var tokens = part.Split(new[] { "*>" }, StringSplitOptions.None);
                if (tokens.Length == 1)
                {
                    Console.Write(tokens[0]);
                }
                else
                {
                    if (!String.IsNullOrEmpty(tokens[0]))
                    {
                        ConsoleColor color;
                        if (tokens[0][0] == '!')
                        {
                            if (Enum.TryParse(tokens[0].Substring(1), true, out color))
                            {
                                bgStack.Push(Console.BackgroundColor);
                                Console.BackgroundColor = color;
                            }
                        }
                        else if (tokens[0][0] == '/')
                        {
                            if (tokens[0].Length > 1 && tokens[0][1] == '!')
                            {
                                if (bgStack.Count > 0)
                                    Console.BackgroundColor = bgStack.Pop();
                            }
                            else
                            {
                                if (fgStack.Count > 0)
                                    Console.ForegroundColor = fgStack.Pop();
                            }
                        }
                        else
                        {
                            if (Enum.TryParse(tokens[0], true, out color))
                            {
                                fgStack.Push(Console.ForegroundColor);
                                Console.ForegroundColor = color;
                            }
                        }
                    }
                    for (int j = 1; j < tokens.Length; j++)
                        Console.Write(tokens[j]);
                }
            }
        }

        static void WriteLineColor(string str)
        {
            WriteColor(str);
            Console.WriteLine();
        }

        static void WriteColor(string str, params object[] arg)
        {
            WriteColor(String.Format(str, arg));
        }

        static void WriteLineColor(string str, params object[] arg)
        {
            WriteColor(String.Format(str, arg));
            Console.WriteLine();
        }
    }
}

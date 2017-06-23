using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Lab5___GeneticAlg
{

    class Comparator : IComparer<List<int>>
    {
        public int Compare(List<int> x, List<int> y)
        {
            if (x.Sum() > y.Sum())
                return -1;
            if (x.Sum() < y.Sum())
                return 1;
            else return 0;
        }
    }

    class ProcSchedule
    {

        Comparator Compar = new Comparator();
        public List<List<int>> Schedule;
        //public List<List<int>> ProcList;
        public int SizeSchedule { get; set; }
        public int ProcCount { get; set; }
        public int Start { get; set; }
        public int End { get; set; }

        public int PopulationSize { get; set; }
        public int MaxIdentyGeneration { get; set; }
        public int ChanceCross { get; set; }
        public int ChanceMutation { get; set; }
        public bool EliteMod { get; set; }

        public List<List<int>> Population = new List<List<int>>();

        static Random random = new Random((int)DateTime.Now.Ticks);
        //ProcCount = Convert.ToInt16(Console.ReadLine());


        public ProcSchedule(int proc = 0 , int size_tasks = 0 , int start = 0 , int end = 0, int max_iden = 0, int cross = -1, int mut = -1, int pop_size = 0, bool eliteFlag = false)
        {
            if (proc == 0)
            {
                Console.Write($"Кол-во процессоров P(N): ");
                ProcCount = Convert.ToInt16(Console.ReadLine());
            }
            else
            {
                Console.WriteLine($"Кол-во процессоров P(N): {proc}");
                ProcCount = proc;
            }
            if (size_tasks == 0)
            {
                Console.Write($"Кол-во задач Q(M)): ");
                SizeSchedule = Convert.ToInt16(Console.ReadLine());
            }
            else
            {
                Console.WriteLine($"Кол-во задач Q(M)):{size_tasks} ");
                SizeSchedule = size_tasks;
            }

            Schedule = new List<List<int>>(SizeSchedule);
           
            EliteMod = eliteFlag;
            if (start == 0)
            {
                Console.Write($"Нижняя граница диапазона: ");
                Start = Convert.ToInt16(Console.ReadLine());
            }
            else
            {
                Console.WriteLine($"Нижняя граница диапазона: ");
                Start = start;
            }
            if (end == 0)
            {
                Console.Write($"Верхняя граница диапазона: ");
                End =  Convert.ToInt16(Console.ReadLine());
            }
            else
            {
                Console.WriteLine($"Верхняя граница диапазона: {end}");
                End = end;
            }
            if (max_iden == 0)
            {
                Console.Write($"Кол-во идентичных поколений: ");
                MaxIdentyGeneration = Convert.ToInt16(Console.ReadLine());
            }
            else
            {
                Console.WriteLine($"Кол-во идентичных поколений: {max_iden}");
                MaxIdentyGeneration = max_iden;
            }

            if (cross == -1)
            {
                Console.Write($"Вероятность оператора кроссвера[0-100]% P(cross): ");
                ChanceCross = Convert.ToInt16(Console.ReadLine());
            }
            else
            {
                Console.WriteLine($"Вероятность оператора кроссвера[0-100]% P(cross):{ChanceCross} ");
                ChanceCross = cross;
            }

            if (mut == -1)
            {
                Console.Write($"Вероятность оператора мутации[0-100]% P(mut): ");
                ChanceMutation = Convert.ToInt16(Console.ReadLine()); 
            }
            else
            {
                Console.WriteLine($"Вероятность оператора мутации[0-100]% P(mut):{mut} ");
                ChanceMutation = mut;
            }
            if (pop_size == 0)
            {
                Console.Write($"Размер популяции: ");
                PopulationSize = Convert.ToInt16(Console.ReadLine()); 
            }
            else
            {
                Console.WriteLine($"Размер популяции:{pop_size} ");
                PopulationSize = pop_size;
            }


            Console.WriteLine("Очередь задач: ");
            for (int k = 0; k < ProcCount; k++)
            {
                Schedule.Add(new List<int>());
                for (int i = 0; i < SizeSchedule; i++)
                {
                    // Schedule.Add(Console.Read());
                    Schedule[k].Add(random.Next(Start, End));
                }
            }

            //for (int i = 0; i < Schedule.Count; i++)
            //{
            //    Console.Write($"{i} CPU: ");
            //    foreach (var item in Schedule[i])
            //    {
            //        Console.Write(item + " ");
            //    }
            //    Console.WriteLine();
            //}

            if (EliteMod == true)
            {

                var TranspList = new List<List<int>>();
                for (int i = 0; i < Schedule[0].Count; i++)
                {
                    TranspList.Add(new List<int>());
                    for (int j = 0; j < Schedule.Count; j++)
                        TranspList[i].Add(0);
                }

                for (int j = 0; j < Schedule.Count; j++)
                {
                    for (var i = 0; i < Schedule[j].Count; i++)
                    {
                        TranspList[i][j] = Schedule[j][i];
                    }
                }

                TranspList.Sort(Compar);
               // TranspList.Reverse();

                for (int j = 0; j < Schedule.Count; j++)
                {

                    for (var i = 0; i < Schedule[j].Count; i++)
                    {
                        Schedule[j][i] = TranspList[i][j];
                    }
                }

                Console.WriteLine("Упорядочим задачи по убыванию:");
                for (int i = 0; i < Schedule.Count; i++)
                {
                    Console.Write($"{i} CPU: ");
                    foreach (var item in Schedule[i])
                    {
                        Console.Write(item + " ");
                    }
                    Console.WriteLine();
                }



                var ElitePerson = new List<int>();
                ElitePerson = FreeLoad(TranspList); 

                for (int i = 0; i < TranspList.Count; i++)
                {
                    ElitePerson[i]=((ElitePerson[i] * 2 + 1) * (256 / ProcCount / 2));
                }
                Population.Add(ElitePerson);

                ShowTasks();
                //for (int j = 0; j < Schedule.Count; j++)
                //{

                //    for (var i = 0; i < Schedule[j].Count; i++)
                //    {
                //        Schedule[j][i] = TranspList[i][j];
                //    }
                //}

            }




            for (int i = 1; i < PopulationSize; i++)
            {
                var person = new List<int>();
                for (int j = 0; j < SizeSchedule; j++)
                {
                    person.Add(random.Next(0, 256));
                }
                Population.Add(person);
            }

            //ShowPopulation();
            

            //ProcList = new List<List<int>>(ProcCount);
            //for (var j = 0; j < ProcCount; j++)
            //    ProcList.Add(new List<int>());

            //for (int i = 0; i < SizeSchedule; i++)
            //{
            //    var index = random.Next(0, ProcCount);
            //   // Console.WriteLine("Добавим в {0} процессор", index);
            //    ProcList[index].Add(Schedule[i]);
            //}

            //ProcList = new List<List<int>>();
            //ProcList.Add(new List<int> { 15});
            //ProcList.Add(new List<int> { 14, 18, 13, 15, 15 });
            //ProcList.Add(new List<int> { 17, 10, 13, 11 });
            //ProcList.Add(new List<int> { 11, 17 });

        }

        public List<int> FreeLoad(List<List<int>> Lst)
        {
            List<int> SumLoad = new List<int>(); //Списко загруженности каждого

            List<List<int>> OutList = new List<List<int>>();
            List <int> OutListIndex = new List<int>();
            for (int k = 0; k < Lst.Count; k++)
            {
                var tempList = new List<int>();
                for (var l = 0; l < Lst[0].Count; l++)
                    tempList.Add(0);
                OutList.Add(tempList);
            }

            for (var i = 0; i < Lst[0].Count; i++)
                SumLoad.Add(0);

            for (var i = 0; i < Lst.Count; i++)
            {
                int min = 0;
                for (var j = 1; j < Lst[i].Count; j++)
                {
                    if (Lst[i][j] + SumLoad[j] < Lst[i][min] + SumLoad[min])
                        min = j;
                }
                SumLoad[min] += Lst[i][min];

               OutList[i][min] = Lst[i][min];
               OutListIndex.Add(min);
               //Console.WriteLine("Шаг {0}:", i);
               //ShowList(OutList);

            }

            //Console.WriteLine("Загруженность процессоров элитной особи:");
            //for (var i = 0; i < SumLoad.Count; i++)
            //{
            //    Console.WriteLine("P[{0}] = {1}", i, SumLoad[i]);
            //}
            //ShowList(OutList);
            return OutListIndex;
        }

        public int SumList(List<int> list)
        {
            int sum = 0;
            for (int i = 0; i < list.Count; i++)
            {
                sum += list[i];
            }
            return sum;
        }

        public void ShowList(List<List<int>> Lst)
        {

            for (int i = 0; i < Lst.Count; i++)
            {

                Console.Write("Процессор " + i + " : ");
                foreach (var item in Lst[i])
                {
                    Console.Write(item + " ");

                }
                Console.Write("(" + SumList(Lst[i]) + ")\n");

            }

            //Console.WriteLine("Транспонированный вывод: ");

            //for (int j = 0; j < Lst[0].Count; j++)
            //{
            //    Console.Write("[{0}] - ",j);
            //    for (int i = 0; i < Lst.Count; i++)
            //    {
            //        Console.Write(Lst[i][j] + " ");
            //    }
            //    Console.Write("(" + SumCol(Lst,j) + ")\n");
            //}

        }

        public void ShowPopulation()
        {
            Console.WriteLine("----------------------------------");
            for(int i=0; i<Population.Count; i++)
            {
                Console.Write($"Особь [{i}]  ");
                for (int j = 0; j < Population[i].Count; j++)
                {
                    Console.Write($"{Population[i][j]} ");
                }
                Console.WriteLine();
            }
            Console.WriteLine($"\nTmax = {FitnessFunc(Population[WorstPerson()]).Max()} [{WorstPerson()}] ");
            Console.WriteLine("----------------------------------");
        }

        public void CrossOperator(int parent1, int parent2)
        {
            int CrossPoint = random.Next(1, SizeSchedule-1) ;
           // Console.WriteLine($"Точка раздела {CrossPoint}");

            List<int> NewPerson1 = new List<int>();
            List<int> NewPerson2 = new List<int>();

            for (int i = 0; i < CrossPoint; i++)
            {
                NewPerson1.Add(Population[parent1][i]);
                NewPerson2.Add(Population[parent2][i]);
            }

            for (int i=CrossPoint; i<SizeSchedule; i++)
            {
                NewPerson1.Add(Population[parent2][i]);
                NewPerson2.Add(Population[parent1][i]);
            }

            //Console.Write("Новая особь 1: ");
            //for (int i=0; i<NewPerson1.Count; i++)
            //{
            //    Console.Write($"{NewPerson1[i]} ");
            //}
            //Console.Write("\nНовая особь 2: ");
            //for (int i = 0; i < NewPerson2.Count; i++)
            //{
            //    Console.Write($"{NewPerson2[i]} ");
            //}
            //Console.WriteLine("");

            Population.Add(NewPerson1);
            Population.Add(NewPerson2);

            if (FitnessFunc(NewPerson1).Max() < FitnessFunc(Population[parent1]).Max() && FitnessFunc(NewPerson1).Max() < FitnessFunc(Population[parent2]).Max())
            {
               // Console.WriteLine("_______Новая особь 1 лучше родителей ");
                //Console.WriteLine($"Tmax новой особи  {FitnessFunc(NewPerson1).Max()} || Tmax[p1]={FitnessFunc(Population[parent1]).Max()} Tmax[p2]={FitnessFunc(Population[parent2]).Max()}");
               // ShowTasks();
            }
           
            if (FitnessFunc(NewPerson2).Max() < FitnessFunc(Population[parent1]).Max() && FitnessFunc(NewPerson2).Max() < FitnessFunc(Population[parent2]).Max())
            {
               // Console.WriteLine("________Новая особь  лучше родителей  ");
              //  Console.WriteLine($" Tmax новой особи = {FitnessFunc(NewPerson2).Max()} || Tmax[p1]={FitnessFunc(Population[parent1]).Max()} Tmax[p2]={FitnessFunc(Population[parent2]).Max()}");
               // ShowTasks();
            }
         

    
        }

        public List<int> FitnessFunc(List<int> Person)
        {
            
            List<int> Eval = new List<int>();
            List<List<int>> Matrix = new List<List<int>>();

            for (int i = 0; i < ProcCount; i++)
                Matrix.Add(new List<int>());

            for (int i = 0; i < Person.Count; i++)
            {
                int proc = ((Person[i] / (256 / ProcCount)));
                //proc = Convert.ToInt16(Math.Truncate(Convert.ToDecimal(proc)));
                if (proc > 0) proc -= 1;
                Matrix[proc].Add(Schedule[proc][i]);
            }

            for (int i=0; i<Matrix.Count; i++)
            {
                Eval.Add(SumList(Matrix[i]));
            }

            //ShowList(Matrix);
            return Eval;
        }

        public void ReductionOperator(int EliteIndex)
        {
            
                int WorstPersonIndex = WorstPerson(EliteIndex);
               // Console.WriteLine($"Удалим самую слабую особь с Тmax = {FitnessFunc(Population[WorstPersonIndex]).Max()}");
                Population.Remove(Population[WorstPersonIndex]);
        }

        static string RevStr(string str)
        {
            string rez="";
            for (int i = str.Length - 1; i >= 0; i--)
                rez += str[i];
            return rez;
        }

        public static string FuncTo2(int value)
        {
            string rez = "";
            while(value > 1)
            {
                rez += (value % 2).ToString();
                value /= 2;
            }
            rez += value.ToString();
            while (rez.Length < 8)
            {
                rez += "0";
            }
            
            return RevStr(rez);
        }

        public int WorstPerson(int EliteIndex = -1)
        {
            int Max = 0;
         
                for (int i = 0; i < Population.Count; i++)
                {
                    if (FitnessFunc(Population[i]).Max() > FitnessFunc(Population[Max]).Max() && i!=EliteIndex)
                        Max = i;
                }
            
            return Max;
        }


        public int BestPerson()
        {
            int Min = 0;

            for (int i = 0; i < Population.Count; i++)
            {
                if (FitnessFunc(Population[i]).Max() < FitnessFunc(Population[Min]).Max())
                    Min = i;
            }

            return Min;
        }

        public void MutaionOperator(List<int> person)
        {
            //ShowTasks();
            int GenNumber = random.Next(0, person.Count);
            int Tmax = FitnessFunc(person).Max();
            //Console.WriteLine($"Мутируем ген #{GenNumber} = {person[GenNumber]} || {FuncTo2(person[GenNumber])} ");

            int Cube = random.Next(0, 8);
           // Console.WriteLine($"Кидаем кубик, меняем {Cube} бит");

            var str = FuncTo2(person[GenNumber]);
            var chars = str.ToCharArray();

            if (chars[Cube] == '1')
                chars[Cube] = '0';
            else
                chars[Cube] = '1';

            str = new string(chars);

            int _int = Convert.ToInt32(str, 2);
            var str2 = Convert.ToString(_int, 2);

            //Console.WriteLine($"{str} |{_int} => Процессор~{_int /(256/ProcCount)}");

            person[GenNumber] = _int / (256 / ProcCount);
            //ShowTasks();
            if (FitnessFunc(person).Max() < Tmax)
            {
                //Console.WriteLine($"____Мутация привела к улучшнеию___ Tmax {Tmax} =>{ FitnessFunc(person).Max() }");
                //ShowTasks();
               // Console.WriteLine();
            }
        }

        public void ShowTasks()
        {
            List<List<int>> BestMatrix = new List<List<int>>();
            int max = 0; int best=10000;
            int best_index = 0;

            for (int j=0; j<Population.Count; j++)
            {
                max = 0;
                //Console.WriteLine($"\nРасписание {j}");

                List<List<int>> Matrix = new List<List<int>>();

                for (int i = 0; i < ProcCount; i++)
                    Matrix.Add(new List<int>());

                for (int i = 0; i < Population[j].Count; i++)
                {
                    int proc = Population[j][i] / (256 / ProcCount);
                    if (proc > 0) proc -= 1;
                    Matrix[proc].Add(Schedule[proc][i]);
                }


                for (int i=0; i<Matrix.Count; i++)
                {
                    if (SumList(Matrix[i]) > max)
                        max = SumList(Matrix[i]);
                }
                if (max < best)
                {
                    BestMatrix = Matrix;
                    best = max;
                    best_index = j;
                }
               // ShowList(Matrix);

            }

            Console.WriteLine($"\nЛучшее Тmax={best} в расписании #{best_index}");
        }

        public int Genetic()
        {
            int IdentyGenCounter = 0;
            int GenCounter = 0;
            int Person1 = 0, Person2 = 0;
            int Cube = 0;
            int PrevWorstValue = 0;
            int EliteIndex = -1;

            if (EliteMod)
            {
                Console.WriteLine("\n\n######## ГА + ЭЛИТАРНАЯ ОСОБЬ ########\n");
                EliteIndex = 0;

            }

            if(EliteMod && GenCounter==0)
            {
               
               //howList(Population);
            }

            while (IdentyGenCounter < MaxIdentyGeneration)
            {
                PrevWorstValue = FitnessFunc(Population[WorstPerson()]).Max();
                //Console.WriteLine($"\nПоколение {GenCounter}");
               // ShowPopulation();
                GenCounter++;
                Cube = random.Next(0, 101);
                //Console.WriteLine($"Кидаем кубик {Cube} \\ {ChanceCross}");

                if (Cube <= ChanceCross)
                {
                    Person1 = random.Next(0, Population.Count);

                    do
                    {
                        Person2 = random.Next(0, Population.Count );
                    }
                    while (Person2 == Person1);
                   // Console.WriteLine($"Применяем оператор кроссовера к {Person1} и {Person2}");
                    CrossOperator(Person1, Person2);

                    Cube = random.Next(0, 101);
                    //Console.WriteLine($"Кидаем кубик {Cube} \\ {ChanceMutation}");
                    if (Cube <= ChanceMutation)
                    {
                        MutaionOperator(Population.Last());
                        MutaionOperator(Population[Population.Count - 2]);
                    }
                    ReductionOperator(EliteIndex);
                    ReductionOperator(EliteIndex);

                    if (PrevWorstValue == FitnessFunc(Population[WorstPerson()]).Max())
                    {
                        IdentyGenCounter++;
                    }
                    else
                    {
                        IdentyGenCounter = 0;
                    }

                    if (EliteMod)
                    {
                        var OldEliteIndex = EliteIndex;
                        EliteIndex = BestPerson();
                        if (OldEliteIndex != EliteIndex)
                        {
                            Console.WriteLine($"____Смена элитарной особи #{OldEliteIndex} => { EliteIndex}");
                        }
                    }
                        
                }
                
            }
            ShowTasks();
            return GenCounter;
        }
    }

    public class MyWriter : TextWriter
    {
        private TextWriter originalOut = Console.Out;

        public override void Write(string value)
        {
            //originalOut.Write(value);
            File.AppendAllText("Test.txt",value);
        }

        public override void WriteLine(string value)
        {
            //originalOut.WriteLine(value);
            File.AppendAllText("Normal.txt", value);
        }

        public override Encoding Encoding
        {
            get
            {
                return Encoding.ASCII;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //var originalOut = Console.Out;
            //Console.SetOut(new MyWriter());

            //ProcSchedule P = new ProcSchedule();
            ProcSchedule P = new ProcSchedule(6,79,10,25,65,99,99,65,false);
            var E =          new ProcSchedule(6, 79, 10, 25, 65, 99, 99, 65, true);
            E.Population = P.Population;
            E.Schedule = P.Schedule;

            Console.WriteLine("\nИсходные расписания");
            P.ShowTasks();
            P.Genetic();
           
            var x1 = P.Genetic();
            P.ShowTasks();
            var x2 = E.Genetic();
            E.ShowTasks();
            //Console.SetOut(originalOut);
            Console.WriteLine("FINISH");
            Console.WriteLine($"Стандартный ГА {x1} поколений\n ГА + Элитизм {x2} поколений");
           // Console.ReadKey(true);
           // Console.ReadKey(true);


            //Console.SetOut(new MyWriter());
            //for (int i = 0; i < 1000; i++)
            //{
            //    var index = i;
            //    P = new ProcSchedule(8, 40, 2, 30, 5, 90, 90, 10, true);
            //    P.Genetic();
            //}

            //P.ShowTasks();






            //Console.SetOut(new MyWriter());
            //int LengthTest = 1000;
            //double AvgRes = 0;

            //var S = new ProcSchedule(8, 20, 1, 100, 10, 90, 95, 20, false);
            //Stopwatch stopWatch = new Stopwatch();
            //stopWatch.Start();
            //for (int i = 0; i < LengthTest; i++)
            //{
            //    AvgRes += S.Genetic();
            //}
            //stopWatch.Stop();
            //TimeSpan ts = stopWatch.Elapsed;

            //string elapsedTime = String.Format("{0:0000}m:{1:0000}s {2:0000}ms",
            //    ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            //File.AppendAllText("Normal.txt", "\nNormal " + (AvgRes / 1000).ToString() + " \nt =" + elapsedTime);


            //AvgRes = 0;
            //var E = new ProcSchedule(8, 20, 1, 100, 5, 90, 95, 20, true);
            //stopWatch = new Stopwatch();
            //stopWatch.Start();
            //for (int i = 0; i < LengthTest; i++)
            //{
            //    AvgRes += E.Genetic();
            //}
            //stopWatch.Stop();
            //ts = stopWatch.Elapsed;

            //elapsedTime = String.Format("{0:0000}m:{1:0000}s {2:0000}ms",
            //    ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            //File.AppendAllText("Normal.txt", "\nElite " + (AvgRes / 1000).ToString() + " \nt =" + elapsedTime);

            //Console.SetOut(originalOut);
            //Console.WriteLine("FINISH");
            //Console.ReadKey();
            //Console.ReadKey();

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Globalization;



namespace HomeWork6
{
    internal class Worker
    {
        //Все поля нам не нужны, так как автосвойства имеют свои ячейки памяти.


        //private int id;
        //private DateTime insertDate;
        //private string fullName;
        //private int age;
        //private int height;
        //private DateOnly birthDate;
        //private string birthAddress;


        public int ID { get; set; }
        public DateTime InsertDate { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public int Height { get; set; }
        public DateOnly BirthDate { get; set; }
        public string BirthAddress { get; set; }


        public void FillFields()
        {
            Console.WriteLine("Please input full name");
            FullName = Console.ReadLine();

            Console.WriteLine("Please input age");
            Age = int.Parse(Console.ReadLine());

            Console.WriteLine("Please input height");
            Height = int.Parse(Console.ReadLine());

            IFormatProvider culture = new CultureInfo("en-US", true);
            Console.WriteLine("Please input date of bith (mm/dd/yyyy)");
            BirthDate = DateOnly.Parse(Console.ReadLine());

            Console.WriteLine("Please input address of bith");
            BirthAddress = Console.ReadLine();

            InsertDate = DateTime.Now;
        }


    }

    internal static class WorkerExt
    {
        public static void PrintData(this Worker employee)
        {
            foreach (FieldInfo filed in typeof(Worker).GetFields())
                Console.WriteLine("{0} = {1}", filed.Name, filed.GetValue(employee));
            Console.WriteLine();
        }
    }

    internal class Repository
    {
        private Worker[] workers;
        private int lastId;

        public Repository()
        { 
            GetAllWorkers();
        }

        //public Worker this[int index] //Indexer
        //{
        //    get { return workers[index]; }
        //    set { workers[index] = value; }
        //}

        public async Task<Worker[]> GetAllWorkers()
        {
            string fileData = await FileAdapter.Read();
            workers =  Serializer.DeserializeData(fileData);
            lastId = workers.Length;

            return workers;
        }

        public Worker GetWorkerById(int id)
        {
            foreach (Worker worker in workers)
            {
                if (worker.ID == id)
                {
                    return worker;
                }
            }
            Console.WriteLine("Worker with such ID doesn't exist");
            return null;
        }

        public async void DeleteWorkerById(int id)
        {
            Worker workerRequested = GetWorkerById(id);
            if (workerRequested != null)
            {
                Worker[] workersNew = new Worker[workers.Length - 1];
                for (int i = 0, j = 0; i < workersNew.Length; i++)
                {
                    if (workers[i] != workerRequested)
                    {
                        workersNew[j++] = workers[i];
                    }
                }
                workers = workersNew;

                await FileAdapter.Write(Serializer.SerializeData(workers));
            }
        }

        public async void AddWorker(Worker worker)
        {
            worker.ID = ++lastId;
            Worker[] workersNew = new Worker[workers.Length + 1];

            for (int i = 0; i < workers.Length; i++)
            {
                workersNew[i] = workers[i];
            }
            workersNew[workersNew.Length - 1] = worker;
            workers = workersNew;

            await FileAdapter.Write(Serializer.SerializeData(workers));
        }

        public Worker[] GetWorkersBetweenTwoDates(DateTime dateFrom, DateTime dateTo)
        {
            List<Worker> result = new List<Worker>();
            foreach (Worker worker in workers)
            { 
                if(worker.InsertDate > dateFrom && worker.InsertDate < dateTo)
                    result.Add(worker);
            }

            return result.ToArray();
        }

        public void Print()
        {
            foreach (Worker worker in workers)
            {
                worker.PrintData();
            }
        }
    }

    internal static class Serializer
    {
        public static string SerializeData(Worker[] workers) //Text encode
        {
            StringBuilder outputText = new StringBuilder();// для скоросости и экономии памяти при конкатизации

            foreach (Worker worker in workers)
            {
                foreach (FieldInfo field in typeof(Worker).GetFields())
                {
                    if (field.Name == "ID")
                        outputText.Append($"{field.GetValue(worker)}");
                    else
                        outputText.Append($"#{field.GetValue(worker)}");
                }
                outputText.Append("\n");
            }

            return outputText.ToString();
        }

        public static Worker[] DeserializeData(string rawText) // Text decode
        {
            string[] workersArr = rawText.Split('\n');
            Worker[] workersList = new Worker[workersArr.Length - 1]; // the reason of -1, is the string "" that appears after split

            for (int i = 0; i < workersArr.Length - 1; i++)
            {
                Worker worker = new Worker();
                string[] workersFields = workersArr[i].Split('#');
                //IFormatProvider culture = new CultureInfo("en-US", true);
                //employee.InsertDate = DateTime.ParseExact(employeeFields[1], "dd.MM.yyyy HH:mm", culture);

                worker.ID = int.Parse(workersFields[0]);
                worker.InsertDate = DateTime.Parse(workersFields[1]);
                worker.FullName = workersFields[2];
                worker.Age = int.Parse(workersFields[3]);
                worker.Height = int.Parse(workersFields[4]);
                worker.BirthDate = DateOnly.Parse(workersFields[5]);
                worker.BirthAddress = workersFields[6];

                workersList[i] = worker;
            }

            return workersList;
        }
    }
}

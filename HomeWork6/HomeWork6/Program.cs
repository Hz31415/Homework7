using HomeWork6;

Console.WriteLine("Welcome to the program");
FileAdapter.Create();
Repository repository = new Repository();

bool _continue = true;

while (_continue)
{
    Console.WriteLine("Please choose the option (input the number)\n\n");
    Console.WriteLine("1. GetAllWorkers");
    Console.WriteLine("2. GetWorkerById");
    Console.WriteLine("3. DeleteWorkerById");
    Console.WriteLine("4. AddWorker");
    Console.WriteLine("5. GetWorkersBetweenTwoDates");
    Console.WriteLine("6. Exit\n\n");

    Console.Write("Your choice here:");
    string choice = Console.ReadLine();
    Console.WriteLine("\n\n");


    switch (choice)
    {
        case "1":
            await repository.GetAllWorkers();
            repository.Print();
            break;

        case "2":
            Console.Write("Input ID: ");
            repository.GetWorkerById(int.Parse(Console.ReadLine()));
            break;

        case "3":
            Console.Write("Input ID: ");
            repository.DeleteWorkerById(int.Parse(Console.ReadLine()));
            break;

        case "4":

            Worker worker = new Worker();
            worker.FillFields();
            repository.AddWorker(worker);
            break;

        case "5":
            Console.WriteLine("Please input date from (mm/dd/yyyy): ");
            DateTime dateFrom = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("Please input date to (mm/dd/yyyy): ");
            DateTime dateTo = DateTime.Parse(Console.ReadLine());
            Worker[] workers = repository.GetWorkersBetweenTwoDates(dateFrom, dateTo);

            foreach (Worker _worker in workers)
            { 
                _worker.PrintData();
            }
            break;

        case "6":
            _continue = false;
            break;

        default:
            Console.WriteLine("Input parameter is wrong. Try again.");
            break;
    }
}





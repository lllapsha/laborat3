namespace laborat3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var catalog = new Catalog();

            bool isTrue = true;

            do
            {
                Console.WriteLine("1. Показать все треки");
                Console.WriteLine("2. Добавить трек");
                Console.WriteLine("3. Удалить трек");
                Console.WriteLine("4. Поиск трека");
                Console.WriteLine("5. Сохранить треки");
                Console.WriteLine("8. Загрузить треки");
                Console.WriteLine("q. Выйти");

                Console.WriteLine();
                var key = Console.ReadKey().KeyChar;

                switch (key)
                {
                    case '1':
                        {
                            Console.WriteLine();
                            foreach (Track tr in catalog.AllTracks)
                            {
                                PrintTrack(tr);
                            }
                            break;
                        }
                    case '2':
                        {
                            Console.WriteLine();
                            var track = ReadTrack();
                            catalog.AddTrack(track);
                            break;
                        }
                    case '3':
                        {
                            Console.WriteLine();
                            var track = ReadTrack();
                            catalog.RemoveTrack(track);
                            break;
                        }
                    case '4':
                        {
                            Console.WriteLine();
                            var track = ReadTrack();
                            catalog.SearchTrack(track);
                            PrintTrack(track);
                            break;
                        }
                    case '5':
                        {
                            Console.WriteLine();
                            SaveMenu(catalog);
                            break;
                        }
                    case '6':
                        {
                            Console.WriteLine();
                            LoadMenu(catalog);
                            break;
                        }
                    case 'q':
                        {
                            isTrue = false;
                            break;
                        }
                }
                Console.WriteLine();
            }
            while (isTrue);

        }
        public static void SaveMenu(Catalog catalog)
        {
            Console.WriteLine("1. Сохранить в JSON");
            Console.WriteLine("2. Сохранить в XML");
            Console.WriteLine("3. Сохранить в SQLite");

            Console.WriteLine();
            var key = Console.ReadKey().KeyChar;

            switch (key) {
                case '1':
                    {
                        Console.WriteLine();
                        catalog.SaveToJson();
                        break;
                    }
                case '2':
                    {
                        Console.WriteLine();
                        catalog.SaveToXml();
                        break;
                    }
                case '3':
                    {
                        Console.WriteLine();
                        catalog.SaveToSQLite();
                        break;
                    }
            }
        }

        public static void LoadMenu(Catalog catalog)
        {
            Console.WriteLine("1. Загрузить из JSON");
            Console.WriteLine("2. Загрузить из XML");
            Console.WriteLine("3. Загрузить из SQLite");

            Console.WriteLine();
            var key = Console.ReadKey().KeyChar;

            switch (key)
            {
                case '1':
                    {
                        Console.WriteLine();
                        catalog.LoadFromJson();
                        break;
                    }
                case '2':
                    {
                        Console.WriteLine();
                        catalog.LoadFromXml();
                        break;
                    }
                case '3':
                    {
                        Console.WriteLine();
                        catalog.LoadFromSQLite();
                        break;
                    }
            }
        }


        public static void PrintTrack(Track track)
        {
            Console.WriteLine(track.Title + " - " + track.Author);
        }

        public static Track ReadTrack()
        {
            Console.Write("Введите название композиции: ");
            string songName = Console.ReadLine();

            Console.Write("Введите автора композиции: ");
            string authorName = Console.ReadLine();

            return new Track(songName, authorName);

        }
    }
}

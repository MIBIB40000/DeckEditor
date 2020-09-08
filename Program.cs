using System;
using System.Runtime.CompilerServices;
using Newtonsoft;

namespace DeckEditor
{

    class Program
	{
		static void Main(string[] args)
		{
			Deck test = new Deck();
			Console.WriteLine("СОЗДАНИЕ СЛУЧАЙНОЙ КОЛОДЫ \n");
			test.GetRandomDeck();
			test.Print();

			Console.WriteLine("\nДЕМОНСТРАЦИЯ СОРТИРОВКИ");
			test.SortDeck();
			test.Print();

			Console.WriteLine("\nДЕМОНСТРАЦИЯ УДАЛЕНИЯ ДУБЛИКАТОВ");
			test.DistinctDoubles();
			test.Print();

			Console.WriteLine("\nДЕМОНСТРАЦИЯ ПЕРЕМЕШИВАНИЯ");
			test.ShuffleDeck();
			test.Print();

			Deck usertest = new Deck();
			Console.WriteLine("\nСОЗДАНИЕ ПОЛЬЗОВАТЕЛЬСКОЙ КОЛОДЫ");
			usertest.CreateCustomDeck();

			Console.WriteLine("\nСОРТИРОВКА");
			usertest.SortDeck();
			//usertest.DistinctDoubles(); понадобится при отключении проверки на дубликаты в методе CreateCustomDeck()
			usertest.Print();

			Console.WriteLine("\nСЕРИАЛИЗАЦИЯ И ДЕСЕРИАЛИЗАЦИЯ");
			usertest.Save();
			Deck detest = Deck.Extract();
			detest.Print();

			Console.ReadKey();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;

namespace DeckEditor
{
	/// <summary>
	/// Класс колоды,
	/// включает Лист объектов карт,
	/// вспомогательные значения и методы для работы с колодой. 
	/// </summary>
    public class Deck
	{
		public string DeckName { get; set; }
		public bool IsSorted { get; set; }		
		
		/// <summary>
		/// Метод добавления карты в лист
		/// </summary>
		/// <param name="card"></param>		
		public void AddCard(Card card)
		{
			if (_deck == null)												      
				_deck = new List<Card>();									      
																			      
			_deck.Add(card);												      
		}																	      
		
		/// <summary>
		/// Создание полной колоды
		/// </summary>		
		public void GetFullDeck()											      
		{																	      
			foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))		      
			{																      
				foreach (CardValue value in Enum.GetValues(typeof(CardValue)))    
				{																  
					Card card = new Card(value, suit);							  
					AddCard(card);												  
				}																  
			}
			DeckName = "Full deck";																  
			IsSorted = true;													  
		}																		  
		
		/// <summary>
		/// Создание псевдослучайной колоды
		/// для демонстрации работы некоторых методов
		/// </summary>		
		public void GetRandomDeck() 											  
		{																		  
			var random = new Random();
																				  
			var values = Enum.GetValues(typeof(CardValue)) as int[];			  
			var suits = Enum.GetValues(typeof(CardSuit)) as int[];				  
																				  
			for (int i = 0; i < _maxDeckSize; i++)								  
			{																	  
				var value = (CardValue) values[random.Next(values.Length)];		  
				var suit = (CardSuit) suits[random.Next(suits.Length)];			  
																				  
				AddCard(new Card(value,suit));									  
			}
			DeckName = "Random deck";
			IsSorted = false;
		}
		
		/// <summary>
		/// Вывод всех необходимых данных о колоде в консоль
		/// </summary>
		public void Print() 							  
		{																		  
			Console.WriteLine($"название колоды - {DeckName} \nКоличество карт - {_deck.Count}");					  				  

			if (IsSorted)														  
			{																	  
				Console.WriteLine("Колода отсортирована");
			}
			else
			{
				Console.WriteLine("Колода НЕ отсортирована");
			}

			Console.WriteLine("Состав колоды: \n");

			foreach(var card in _deck)											  
			{																	  
				card.Print();
			}

		} 
		
		/// <summary>
		/// Перемешивание колоды
		/// </summary>
		public void ShuffleDeck() 
		{
            var length = _deck.Count;
            var random = new Random();

            while (length > 1)
            {
				int k = random.Next(length);
				length--;

                var card = _deck[k];
                _deck[k] = _deck[length];
                _deck[length] = card;
            }

			IsSorted = false;
		}																		 
		
		/// <summary>
		/// Сортировка колоды
		/// по числовым значениям "веса"
		/// </summary>
		public void SortDeck() 
		{
			_deck = _deck
				.OrderBy(x => (int)x.Suit + (int)x.Value)
				.ToList();

			IsSorted = true;
		}
		
		/// <summary>
		/// Удаление дубликатов,
		/// Используется метод расширения .DistinctBy для LINQ.
		/// </summary>
		public void DistinctDoubles()  // Решил вынести этот метод отдельно из создания пользовательской колоды
									   // чтобы юзер мог вставлять дубликаты для специфичных случаев
		{
			_deck = _deck
				.DistinctBy(p => new { p.Value, p.Suit })
				.ToList();

			IsSorted = true;
		}
		
		/// <summary>
		/// Создание собственной колоды
		/// </summary>
		public void CreateCustomDeck()
		{
			Console.WriteLine("Введите название колоды");	 
																				  
			while (true)												  
            {
				string deckName = Console.ReadLine();

				if (string.IsNullOrEmpty(deckName))
					Console.WriteLine("Введите корректное название");
				else
					break;
			}                                                                     

			Console.WriteLine("Введите размер колоды в диапазоне [1..100]");

			while (true)
			{
				_maxDeckSize = ReadInt();//метод вынесен ниже

				if (_maxDeckSize > 0 && _maxDeckSize <= 100)
                {
					break;
                }
				else
                {
					Console.WriteLine("Размер колоды может быть только в диапазоне [1..100]");
                }
			}

			Console.WriteLine("Добавляйте карты по шаблону: КАРТА МАСТЬ ");
			int count = 0;

			var textInfo = new CultureInfo("en-US", false).TextInfo;
			while (count < _maxDeckSize) 
			{
				Console.WriteLine("Введите карту");

				string input = Console.ReadLine();

				var stringArr = textInfo
					.ToTitleCase(input)
					.Split(' '); 

				if (Enum.TryParse(stringArr[0], out CardValue MyValue))
				{
					if (Enum.TryParse(stringArr[1], out CardSuit MySuit))
					{
						//условие проверки на дубликаты при заполнении колоды по требованию задания
						//если убрать это условие, то пользователь сможет добавлять дубликаты

						if (_deck.Where(x => x.Value == MyValue && x.Suit == MySuit).Any())
						{
							Console.WriteLine("В колоде не должно быть дубликатов!");
						}
						else
						{
							AddCard(new Card(MyValue, MySuit));
							count++;
						}
					}
					else
					{
						Console.WriteLine("Некорректная масть");
					}
				}
				else
				{
					Console.WriteLine("Некорректная карта");
				}
				
			}
		}

		private int ReadInt() //вынесенный метод для проверки ввода
        {
			while (true) 
			{
				if (int.TryParse(Console.ReadLine(), out int num))
				{
					return num;
				}
				else
				{
					Console.WriteLine("Введите корректное число");
				}
			}
		}

		/// <summary>
		/// Сериализация колоды в json
		/// </summary>
		public void Save()
		{
            try
            {
				Console.WriteLine("Сохранение вашей колоды");

				string path = DeckName + ".json";
				string json = JsonConvert.SerializeObject(this);

				File.WriteAllText(path, json);
			}
            catch (Exception exception)
            {
				Console.WriteLine($"Колода не может быть сохранена. {exception.ToString()}");
            }

		}

		/// <summary>
		/// Десериализация
		/// </summary>
		/// <returns></returns>
		public static Deck Extract()
		{
			Console.WriteLine("Введите название колоды для загрузки");

			string path = Console.ReadLine() + ".json";
			string json = File.ReadAllText(path);

			Deck deck = JsonConvert.DeserializeObject<Deck>(json);

			return deck;
		}

		[JsonProperty]
		private List<Card> _deck = new List<Card>(); //лист карт

		private int _maxDeckSize = 15; //по умолчанию оставил маленькое значение для удобства и наглядности демонстрации
									   //работы методов на рандомной колоде, для пользовательской задается руками
	}
}

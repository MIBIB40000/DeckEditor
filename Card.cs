using System;

namespace DeckEditor
{
	/// <summary>
	/// Класс карты
	/// Объект составляется из двух значений enum - самой карты и ее масти
	/// </summary>
    public class Card 
	{
		public CardValue Value { get; private set; }
		public CardSuit Suit { get; private set; }

		public Card(CardValue value, CardSuit suit) 
		{ 
			Value = value; 
			Suit = suit; 
		}

		public void Print()
		{
			Console.WriteLine($"{Value} {Suit}");
		}


	}
}

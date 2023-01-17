using System.ComponentModel;
using System.Diagnostics;

namespace Hangman;

public partial class MainPage : ContentPage, INotifyPropertyChanged
{
	private string spotlight;
	private string message;
	private string gameStatus;
	private string currentImage = "img0.jpg";
	private int mistakes = 0;
	private int maxWrong = 6;
	private string answer = string.Empty;
	private List<char> letters = new();
	private List<char> guessed = new();
	private List<string> words = new()
	{
		"python",
		"javascript",
		"csharp",
		"java",
		"sql",
		"stephenking",
		"excel",
		"arnold",
		"shining",
		"word",
		"maui",
		"cpp"
	};

	public MainPage()
	{
		InitializeComponent();
		Letters.AddRange("abcdefghijklmnopqrstuvwxyz");
		BindingContext = this;
		PickWord();
		CalculateWord(answer, guessed);
	}

	public string Spotlight
	{
		get => spotlight;
		set
		{
			spotlight = value;
			OnPropertyChanged();
		}
	}

	public List<char> Letters
	{
		get => letters;
		set
		{
			letters = value;
			OnPropertyChanged();
		}
	}

	public string Message
	{
		get => message;
		set
		{
			message = value;
			OnPropertyChanged();
		}
	}

	public string GameStatus
	{
		get => gameStatus;
		set
		{
			gameStatus = value;
			OnPropertyChanged();
		}
	}

	public string CurrentImage
	{
		get => currentImage;
		set
		{
			currentImage = value;
			OnPropertyChanged();
		}
	}

	private void PickWord()
	{
		answer = words[new Random().Next(words.Count)];

		Debug.WriteLine(answer);
	}

	private void CalculateWord(string answer, List<char> guessed)
	{
		var letters = answer
			.Select(x => guessed.IndexOf(x) >= 0 ? x : '_')
			.ToArray();

		Spotlight = string.Join(' ', letters);
	}

	private void BtnLetter_Clicked(object sender, EventArgs e)
	{
		var btn = sender as Button;
		if (btn is not null)
		{
			var letter = btn.Text;
			btn.IsEnabled = false;
			HandleGuess(letter[0]);
		}
	}

	private void HandleGuess(char letter)
	{
		if (!guessed.Contains(letter))
		{
			guessed.Add(letter);
		}

		if (answer.Contains(letter))
		{
			CalculateWord(answer, guessed);
			CheckIfGameWon();
		}
		else if (!answer.Contains(letter))
		{
			mistakes++;
			CurrentImage = $"img{mistakes}.jpg";
			UpdateStatus();
			CheckIfGameLost();
		}
	}

	private void CheckIfGameLost()
	{
		if (mistakes == maxWrong)
		{
			Message = "You Lost!";
			DisableLetters();
		}
	}

	private void CheckIfGameWon()
	{
		if (spotlight.Replace(" ", "") == answer)
		{
			Message = "You win!";
			DisableLetters();
		}
	}

	private void DisableLetters()
	{
		foreach (var children in LettersContainer.Children)
		{
			var btn = children as Button;
			if (btn is not null && btn.IsEnabled)
			{
				btn.IsEnabled = false;
			}
		}
	}

	private void EnableLetters()
	{
		foreach (var children in LettersContainer.Children)
		{
			var btn = children as Button;
			if (btn is not null && !btn.IsEnabled)
			{
				btn.IsEnabled = true;
			}
		}
	}

	private void UpdateStatus()
	{
		GameStatus = $"Errors: {mistakes} of {maxWrong}";
	}

	private void BtnReset_Clicked(object sender, EventArgs e)
	{
		mistakes = 0;
		Message = "";
		CurrentImage = "img0.jpg";
		guessed = new List<char>();
		PickWord();
		CalculateWord(answer, guessed);
		EnableLetters();
		UpdateStatus();
	}
}

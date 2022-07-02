using System;
using FluentAssertions;
using NUnit.Framework;

namespace Challenge
{
	[TestFixture]
	public class WordsStatistics_Tests
	{
		public virtual IWordsStatistics CreateStatistics()
		{
			// меняется на разные реализации при запуске exe
			return new WordsStatistics();
		}

		private IWordsStatistics wordsStatistics;

		[SetUp]
		public void SetUp()
		{
			wordsStatistics = CreateStatistics();
		}

		[Test]
		public void GetStatistics_IsEmpty_AfterCreation()
		{
			wordsStatistics.GetStatistics().Should().BeEmpty();
		}

		[Test]
		public void GetStatistics_ContainsItem_AfterAddition()
		{
			wordsStatistics.AddWord("abc");
			wordsStatistics.GetStatistics().Should().Equal(new WordCount("abc", 1));
		}

		[Test]
		public void GetStatistics_ContainsManyItems_AfterAdditionOfDifferentWords()
		{
			wordsStatistics.AddWord("abc");
			wordsStatistics.AddWord("def");
			wordsStatistics.GetStatistics().Should().HaveCount(2);
		}

		[Test]
		public void GetStatistics_ShouldTrunkWordTo10Symbols()
		{
			wordsStatistics.AddWord("1234567890oooooooo");
			wordsStatistics.GetStatistics().Should().Equal(new WordCount("1234567890", 1));
		}

		[Test]
		public void GetStatistics_ThrowsWhenWordIsNull()
		{
			Assert.Throws<ArgumentNullException>(() => wordsStatistics.AddWord(null));
		}

		[Test]
		public void GetStatistics_IsEmpty_AfterAddingEmptyString()
		{
			wordsStatistics.AddWord("");
			wordsStatistics.GetStatistics().Should().BeEmpty();
		}

		[Test]
		public void GetStatistics_IsEmpty_AfterAddingWhiteSpace()
		{
			wordsStatistics.AddWord(" ");
			wordsStatistics.GetStatistics().Should().BeEmpty();
		}


		[Test]
		public void GetStatistics_ShouldNotTrunkWordWith10Symbols()
		{
			wordsStatistics.AddWord("1234567890");
			wordsStatistics.GetStatistics().Should().Equal(new WordCount("1234567890", 1));
		}

		[Test]
		public void GetStatistics_ContainsOneItemWithDifferentRegisters()
		{
			wordsStatistics.AddWord("abc");
			wordsStatistics.AddWord("Abc");
			wordsStatistics.GetStatistics().Should().Equal(new WordCount("abc", 2));
		}

		[Test]
		public void GetStatistics_InOrderByWord()
		{
			wordsStatistics.AddWord("abc");
			wordsStatistics.AddWord("def");
			wordsStatistics.GetStatistics().Should().Equal(
				new WordCount("abc", 1),
				new WordCount("def", 1));
		}

		[Test]
		public void GetStatistics_InOrderByCount()
		{
			wordsStatistics.AddWord("abc");
			wordsStatistics.AddWord("def");
			wordsStatistics.AddWord("def");
			wordsStatistics.GetStatistics().Should().Equal(
				new WordCount("def", 2),
				new WordCount("abc", 1));
		}

		[Test]
		public void GetStatistics_NotEmpty_AfterAdding10SpacesBeginningString()
		{
			wordsStatistics.AddWord("           p");
			wordsStatistics.GetStatistics().Should().NotBeEmpty();
		}

		[Test]
		public void GetStatistics_ShouldSumTrunkedWords()
		{
			wordsStatistics.AddWord("12345678901");
			wordsStatistics.AddWord("123456789011");
			wordsStatistics.GetStatistics().Should().Equal(new WordCount("1234567890", 2));
		}

		[Test]
		public void GetStatistics_ShouldHaveEndlessCount()
		{
			for (var i = 0; i < 1500; i++)
				wordsStatistics.AddWord(i.ToString());

			wordsStatistics.GetStatistics().Should().HaveCount(1500);
		}

		// [Test]
		// public void GetStatistics_CountShouldBePositive()
		// {
		// 	wordsStatistics.AddWord("abc");
		// 	wordsStatistics.AddWord("abc");
		// 	wordsStatistics.AddWord("abc");
		// 	wordsStatistics.AddWord("def");
		// 	wordsStatistics.AddWord("def");
		// 	wordsStatistics.AddWord("def");
		// 	wordsStatistics.AddWord("def");
		// 	wordsStatistics.GetStatistics().Should().Equal(
		// 		new WordCount("def", 4),
		// 		new WordCount("abc", 3));
		// }

		[Test]
		public void GetStatistics_ShouldBeSameOnMultipleInvokes()
		{
			wordsStatistics.AddWord("abc");
			wordsStatistics.GetStatistics().Should().Equal(new WordCount("abc", 1));
			wordsStatistics.GetStatistics().Should().Equal(new WordCount("abc", 1));
		}
		
		[Test]
		public void GetStatistics_ShouldChangeAfterInvoke()
		{
			wordsStatistics.AddWord("abc");
			wordsStatistics.GetStatistics().Should().Equal(new WordCount("abc", 1));
			wordsStatistics.AddWord("abc");
			wordsStatistics.GetStatistics().Should().Equal(new WordCount("abc", 2));
		}
		
		[Test]
		public void GetStatistics_ShouldBeCorrectOnToLower()
		{
			wordsStatistics.AddWord("ẞ");
			wordsStatistics.GetStatistics().Should().Equal(new WordCount("ẞ".ToLower(), 1));
		}
		
		[Test]
		public void GetStatistics_ShouldHaveDifferentInstances()
		{
			wordsStatistics.AddWord("abc");
			var ws2 = CreateStatistics();
			wordsStatistics.GetStatistics().Should().Equal(new WordCount("abc", 1));
		}
	}
}

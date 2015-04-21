﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using NMoneys.Support;
using NUnit.Framework;

namespace NMoneys.Tests
{
	[TestFixture]
	public partial class CurrencyTester
	{
		#region Parse

		[Test]
		public void Code_Defined_UpperCased_AlphabeticCode_CodeParsed()
		{
			Assert.That(Currency.Code.Parse("USD"), Is.EqualTo(CurrencyIsoCode.USD));
		}

		[Test]
		public void Code_Defined_LowerCased_AlphabeticCode_CodeParsed()
		{
			Assert.That(Currency.Code.Parse("eur"), Is.EqualTo(CurrencyIsoCode.EUR));
		}

		[Test]
		public void Code_Defined_MixedCased_AlphabeticCode_CodeParsed()
		{
			Assert.That(Currency.Code.Parse("NoK"), Is.EqualTo(CurrencyIsoCode.NOK));
		}

		[Test]
		public void Code_Defined_NumericCode_CodeParsed()
		{
			Assert.That(Currency.Code.Parse("999"), Is.EqualTo(CurrencyIsoCode.XXX));
		}

		[Test]
		public void Parse_Defined_PadedNumericCode_CodeParsed()
		{
			Assert.That(Currency.Code.Parse("036"), Is.EqualTo(CurrencyIsoCode.AUD));
		}

		[Test]
		public void Parse_Null_Exception()
		{
			Assert.That(() => Currency.Code.Parse(null), Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void Parse_Undefined_AlphabeticCode_Exception()
		{
			Assert.That(() => Currency.Code.Parse("notAnIsoCode"), Throws.InstanceOf<InvalidEnumArgumentException>());
		}

		[Test]
		public void Parse_Undefined_NumericCode_Exception()
		{
			Assert.That(() => Currency.Code.Parse("0"), Throws.InstanceOf<InvalidEnumArgumentException>());
		}

		[Test]
		public void Parse_Overflowing_NumericCode_Exception()
		{
			long overflowingCode = short.MinValue + 1L;
			Assert.That(() => Currency.Code.Parse(overflowingCode.ToString()), Throws.InstanceOf<InvalidEnumArgumentException>());
		}

		#endregion

		#region TryParse

		[Test]
		public void TryParse_Defined_UpperCased_AlphabeticCode_CodeParsed()
		{
			CurrencyIsoCode? parsed;
			Assert.That(Currency.Code.TryParse("USD", out parsed), Is.True);
			Assert.That(parsed, Is.EqualTo(CurrencyIsoCode.USD));

		}

		[Test]
		public void TryParse_Defined_LowerCased_AlphabeticCode_CodeParsed()
		{
			CurrencyIsoCode? parsed;
			Assert.That(Currency.Code.TryParse("eur", out parsed), Is.True);
			Assert.That(parsed, Is.EqualTo(CurrencyIsoCode.EUR));
		}

		[Test]
		public void TryParse_Defined_MixedCased_AlphabeticCode_CodeParsed()
		{
			CurrencyIsoCode? parsed;
			Assert.That(Currency.Code.TryParse("NoK", out parsed), Is.True);
			Assert.That(parsed, Is.EqualTo(CurrencyIsoCode.NOK));
		}

		[Test]
		public void TryParse_Defined_NumericCode_CodeParsed()
		{
			CurrencyIsoCode? parsed;
			Assert.That(Currency.Code.TryParse("999", out parsed), Is.True);
			Assert.That(parsed, Is.EqualTo(CurrencyIsoCode.XXX));
		}

		[Test]
		public void TryParse_Defined_PaddedNumericCode_CodeParsed()
		{
			CurrencyIsoCode? parsed;
			Assert.That(Currency.Code.TryParse("036", out parsed), Is.True);
			Assert.That(parsed, Is.EqualTo(CurrencyIsoCode.AUD));
		}

		[Test]
		public void TryParse_Null_Exception()
		{
			CurrencyIsoCode? parsed;
			Assert.That(Currency.Code.TryParse(null, out parsed), Is.False);
			Assert.That(parsed, Is.Null);
		}

		[Test]
		public void TryParse_Undefined_AlphabeticCode_Exception()
		{
			CurrencyIsoCode? parsed;
			Assert.That(Currency.Code.TryParse("notAnIsoCode", out parsed), Is.False);
			Assert.That(parsed, Is.Null);
		}

		[Test]
		public void TryParse_Undefined_NumericCode_Exception()
		{
			CurrencyIsoCode? parsed;
			Assert.That(Currency.Code.TryParse("0", out parsed), Is.False);
			Assert.That(parsed, Is.Null);
		}

		[Test]
		public void TryParse_Overflowing_NumericCode_Exception()
		{
			CurrencyIsoCode? parsed;
			long overflowingCode = short.MinValue + 1L;
			Assert.That(Currency.Code.TryParse(overflowingCode.ToString(), out parsed), Is.False);
			Assert.That(parsed, Is.Null);
		}

		#endregion

		#region Cast

		[Test]
		public void Cast_Defined_CastedIsoCode()
		{
			Assert.That(Currency.Code.Cast(36), Is.EqualTo(CurrencyIsoCode.AUD));
		}

		[Test]
		public void Cast_UndefinedValue_Exception()
		{
			Assert.That(() => Currency.Code.Cast(46),
				Throws.InstanceOf<InvalidEnumArgumentException>()
					.With.Message.StringContaining("46")
					.And.Message.StringContaining("CurrencyIsoCode"));
		}

		#endregion

		#region TryCast

		[Test]
		public void TryCast_DefinedValue_CastedIsoCode()
		{
			CurrencyIsoCode? casted;
			Assert.That(Currency.Code.TryCast(36, out casted), Is.True);
			Assert.That(casted, Is.EqualTo(CurrencyIsoCode.AUD));
		}

		[Test]
		public void TryCast_UndefinedValue_False()
		{
			CurrencyIsoCode? casted;
			Assert.That(Currency.Code.TryCast(46, out casted), Is.False);
			Assert.That(casted, Is.Null);
		}

		#endregion

		#region Comparer

		[Test, NUnit.Framework.Category("Performance"), Platform(Include = "Net-2.0")]
		public void Comparer_BetterPerformance_ThanDefaultComparer()
		{
			CurrencyIsoCode[] values = Enumeration.GetValues<CurrencyIsoCode>();

			int iterations = 1000000;
			TimeSpan fast = run(iterations, i => 
				populateDictionary(new Dictionary<CurrencyIsoCode, int>(Currency.Code.Comparer), values, i));
			TimeSpan @default = run(iterations, i => 
				populateDictionary(new Dictionary<CurrencyIsoCode, int>(EqualityComparer<CurrencyIsoCode>.Default), values, i));

			Assert.That(fast, Is.LessThan(@default), "{0} < {1}", fast, @default);

			// not only faster, more than 5 times faster
			Assert.That(fast.TotalMilliseconds * 5, Is.LessThan(@default.Milliseconds), "{0} << {1}", fast, @default);
		}

		[Test, NUnit.Framework.Category("Performance")]
		public void Comparer_SlightlyBetterPerformance_ThanDefaultComparer()
		{
			CurrencyIsoCode[] values = Enumeration.GetValues<CurrencyIsoCode>();

			int iterations = 1000000;
			TimeSpan fast = run(iterations, i => 
				populateDictionary(new Dictionary<CurrencyIsoCode, int>(Currency.Code.Comparer), values, i));
			TimeSpan @default = run(iterations, i => 
				populateDictionary(new Dictionary<CurrencyIsoCode, int>(EqualityComparer<CurrencyIsoCode>.Default), values, i));

			Assert.That(fast, Is.LessThan(@default), "{0} < {1}", fast, @default);
		}

		private static void populateDictionary(Dictionary<CurrencyIsoCode, int> map, CurrencyIsoCode[] values, int iters)
		{
			for (var i = 0; i < iters; i++)
			{
				CurrencyIsoCode code = values[i%values.Length];
				map[code] = i;
			}
		}

		private static TimeSpan run(int numberOfIterations, Action<int> action)
		{
			action(numberOfIterations);
			var stopwatch = new Stopwatch();
			var elapsed = TimeSpan.Zero;
			int runs = 3;
			for (var i = 0; i < runs; i++)
			{
				stopwatch.Start();
				action(numberOfIterations);
				stopwatch.Stop();
				elapsed += stopwatch.Elapsed;
				stopwatch.Reset();
			}
			elapsed = TimeSpan.FromTicks(elapsed.Ticks / runs);

			Debug.WriteLine(elapsed.TotalMilliseconds + " ms");

			return elapsed;
		}

		#endregion
	}
}
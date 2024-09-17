using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using NUnit.Framework;
using UnityEngine;


namespace Meta.Tests.Editor
{
	[TestFixture]
	public class TestsLuaFirst
	{
		private Script _script;
		
		[SetUp]
		public void SetUp()
		{
			_script = new Script();
			Script.DefaultOptions.DebugPrint = Debug.Log;
			_script.Globals["print"] = (System.Action<DynValue>) Debug.Log;
		}

		private double MoonSharpFactorial()
		{
			string scriptCode = @"    
		-- defines a factorial function
		function fact (n)
			if (n == 0) then
				return 1
			else
				return n*fact(n - 1)
			end
		end";

	
			Script script = new Script();
			script.DoString(scriptCode);
			DynValue luaFactFunction = script.Globals.Get("fact");
			DynValue res = script.Call(luaFactFunction, DynValue.NewNumber(4));
			return res.Number;
		}


		[Test]
		public void TestLuaFactorial_Step1()
		{
			double result = MoonSharpFactorial();
			Assert.AreEqual(24, result);
		}
		
		[Test]
		public void TestLuaHelloWorld()
		{
			string script = @"print('Hello, World!')";
			_script.DoString(script);
		}


		[Test]
		public void TestGetTypeValue()
		{
			// Create a new number
			DynValue v1 = DynValue.NewNumber(1);
			// and a new string
			DynValue v2 = DynValue.NewString("ciao");
			// and another string using the automagic converters
			DynValue v3 = DynValue.FromObject(new Script(), "hello");
			
			// This prints Number - String - String
			Debug.Log($"{v1.Type} - {v2.Type} - {v3.Type}");
			Debug.Log($"{v1.Number} - {v2.String} - {v3.String}");
			
		}


		[Test]
		public void TestReturn()
		{
			DynValue ret = Script.RunString("return true, 'ciao', 2*3");
			// prints "Tuple"
			Debug.Log(string.Format("{0}", ret.Type));



			// Prints:
			//   Boolean = true
			//   String = "ciao"
			//   Number = 6
			for (int i = 0; i < ret.Tuple.Length; i++) //массив
				Debug.Log(string.Format("{0} = {1}", ret.Tuple[i].Type, ret.Tuple[i]));
		}
	
		
		private double CallbackTest()
		{
			string scriptCode = @"    
	        -- defines a factorial function
	        function fact (n)
	            if (n == 0) then
	                return 1
	            else
					return Mul(n, fact(n - 1)); --//here we call Mul function from C# code
	            end
	        end";

			Script script = new Script();
			script.Globals["Mul"] = (Func<int, int, int>) Mul;
			
			script.DoString(scriptCode);
			DynValue res = script.Call(script.Globals["fact"], 4);
			return res.Number;
		}
		
		private int Mul(int a, int b)
		{
			return a * b;
		}
		
		[Test]
		public void TestLuaCallback()
		{
			double result = CallbackTest();
			Assert.AreEqual(24, result);
		}


		
		
		
		private IEnumerable<int> GetNumbers()
		{
			for (int i = 1; i <= 10; i++)
				yield return i;
		}

		private double EnumerableTest()
		{
			string scriptCode = @"    
		        total = 0;
		        
		        for i in getNumbers() do
		            total = total + i;
		        end

		        return total;
				";

			Script script = new Script();
			script.Globals["getNumbers"] = (Func<IEnumerable<int>>) GetNumbers;
			DynValue res = script.DoString(scriptCode);
			return res.Number;
		}
		
		[Test]
		public void TestLuaEnumerable()
		{
			double result = EnumerableTest();
			Assert.AreEqual(55, result);
		}	




	}


}

using NUnit.Framework;
using System;
namespace HeroClassTester
{
	[TestFixture()]
	public class HeroClassTester
	{
        Hero hero;

		[Test()]
		public void TestCase()
		{
            hero = new Hero();
		}
	}
}

using System.Linq;
using Xunit;

namespace System.Unicode.Tests
{
	public sealed class UnihanCharacterDataTests
	{
		private static readonly UnicodeBlock[] Blocks = UnicodeInfo.GetBlocks();

		[Theory]
		[InlineData(0x29C0A, 191, -5)]
		[InlineData(0x33473, 212, -9)]
		public void NegativeRadicalStrokeCountsShouldBePreserved(int codePoint, int radical, int strokeCount)
		{
			var radicalStrokeCount = Assert.Single(UnicodeInfo.GetCharInfo(codePoint).UnicodeRadicalStrokeCounts);
			Assert.Equal(radical, radicalStrokeCount.Radical);
			Assert.Equal(strokeCount, radicalStrokeCount.StrokeCount);
		}

		[Theory]
		[InlineData("CJK Unified Ideographs")]
		[InlineData("CJK Unified Ideographs Extension A")]
		[InlineData("CJK Unified Ideographs Extension B")]
		[InlineData("CJK Unified Ideographs Extension C")]
		[InlineData("CJK Unified Ideographs Extension D")]
		[InlineData("CJK Unified Ideographs Extension E")]
		[InlineData("CJK Unified Ideographs Extension F")]
		[InlineData("CJK Unified Ideographs Extension G")]
		[InlineData("CJK Unified Ideographs Extension H")]
		[InlineData("CJK Unified Ideographs Extension I")]
		[InlineData("CJK Unified Ideographs Extension J")]
		[InlineData("CJK Compatibility Ideographs")]
		[InlineData("CJK Compatibility Ideographs Supplement")]
		public void CodePointPackingShouldRoundTrip(string blockName)
		{
			var block = Blocks.Single(b => b.Name == blockName);

			foreach (int codePoint in block.CodePointRange)
			{
				Assert.Equal(codePoint, UnihanCharacterData.UnpackCodePoint(UnihanCharacterData.PackCodePoint(codePoint)));
			}
		}
	}
}

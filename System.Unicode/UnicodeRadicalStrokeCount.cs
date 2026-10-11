using System.Diagnostics;

namespace System.Unicode
{
	/// <summary>Provides information on radical and additional stroke count for a code point.</summary>
	/// <remarks>Values of this type are usually associated with the property kRSUnicode (aka. Unicode_Radical_Stroke).</remarks>
	[DebuggerDisplay(@"{IsSimplified ? ""Simplified"" : ""Traditional"",nq} Radical {Radical} + {StrokeCount} Strokes")]
	public readonly struct UnicodeRadicalStrokeCount
	{
#if NETSTANDARD1_1 || NET45
		internal static readonly UnicodeRadicalStrokeCount[] EmptyArray = new UnicodeRadicalStrokeCount[0];
#endif

		/// <summary>Initializes a new instance of the class <see cref="UnicodeRadicalStrokeCount"/> from raw data.</summary>
		/// <param name="rawRadical">The raw value to use for <see cref="Radical"/>.</param>
		/// <param name="rawStrokeCount">The raw value to use for <see cref="RawStrokeCount"/>.</param>
		internal UnicodeRadicalStrokeCount(byte rawRadical, byte rawStrokeCount)
		{
			Radical = rawRadical;
			RawStrokeCount = rawStrokeCount;
		}

		/// <summary>Initializes a new instance of the class <see cref="UnicodeRadicalStrokeCount"/>.</summary>
		/// <param name="radical">The index of the Kangxi radical of the character.</param>
		/// <param name="strokeCount">The number of additional strokes required to form the character from the radical.</param>
		/// <param name="isSimplified">Indicates whether the character is simplified.</param>
		internal UnicodeRadicalStrokeCount(byte radical, sbyte strokeCount, bool isSimplified)
		{
			// Two's complement doesn't work anymore there, as we have some code points with more than 64 additional strokes.
			// Negative strokes don't seem to go below -9 for now, so we'll map value between -16 and 111 as 112..127;0..111.
			// Clamp values outside the representable range.
			if (strokeCount < -16) strokeCount = -16;
			else if (strokeCount > 127 - 16) strokeCount = 127 - 16;

			Radical = radical;
			// Pack strokeCount together with isSimplified in a single byte.
			RawStrokeCount = unchecked((byte)(strokeCount & 0x7F | (isSimplified ? 0x80 : 0x00)));
		}

		/// <summary>Gets the index of the Kangxi radical of the character.</summary>
		/// <remarks>The Kangxi radicals are numbered from 1 to 214 inclusive.</remarks>
		/// <value>The index of the Kangxi radical.</value>
		public byte Radical { get; }

		/// <summary>Gets the value of <see cref="StrokeCount"/> packed with <see cref="IsSimplified"/>.</summary>
		/// <remarks>
		/// The stroke count is stored as a 7bit value, together with the <see cref="IsSimplified"/> flag as a 1bit value.
		/// Raw values between 112 and 127 represent negative stroke counts -16 to -1.
		/// </remarks>
		/// <value>The raw value of <see cref="StrokeCount"/>.</value>
		internal byte RawStrokeCount { get; }

		/// <summary>Gets the additional stroke count.</summary>
		/// <value>The additional stroke count.</value>
		public sbyte StrokeCount => (RawStrokeCount & 0x7F) is int c && c > 111 ? unchecked((sbyte)(c - 128)) : unchecked((sbyte)c);

		/// <summary>Gets a value indicating whether the information is based on the simplified form of the radical.</summary>
		/// <value><see langword="true" /> if the information is based on the simplified form of the radical; otherwise, <see langword="false" />.</value>
		public bool IsSimplified => (RawStrokeCount & 0x80) != 0;
	}
}

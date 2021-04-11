using System;
using System.Linq;
using System.Text;

namespace RailFenceCipher
{
	public static class RailFenceCipher
    {
	    public static string Encode(string message, int railCount)
        {
			if (string.IsNullOrWhiteSpace(message))
				return string.Empty;

			if (railCount < 2)
				throw new ArgumentOutOfRangeException($"{nameof(railCount)} must be greater or equal 2.");

			var rails = new StringBuilder[railCount];

            for (var index = 0; index < railCount; index++)
				rails[index] = new StringBuilder();

            IterateElementsByZigZag(message.Length, railCount,
	            (currentRail, index) => rails[currentRail].Append(message[index]));

	        return new string(rails.SelectMany(rail => rail.ToString()).ToArray());
        }

        public static string Decode(string encodedMessage, int railCount)
        {
	        if (string.IsNullOrWhiteSpace(encodedMessage))
		        return string.Empty;

	        if (railCount < 2)
		        throw new ArgumentOutOfRangeException($"{nameof(railCount)} must be greater or equal 2.");

	        var railLengths = GetEachRailLength(encodedMessage.Length, railCount);
	        var railStrings = GetRailStrings(railCount, railLengths, encodedMessage);
	        var originalMessage = new char[encodedMessage.Length];
			var railIndexCounter = new int[railCount];

			IterateElementsByZigZag(encodedMessage.Length, railCount,
				(currentRail, index) =>
				{
					originalMessage[index] = railStrings[currentRail][railIndexCounter[currentRail]];
					railIndexCounter[currentRail]++;
				});

			return new string(originalMessage);
        }

        private static string[] GetRailStrings(int railCount, int[] railLengths, string encodedMessage)
        {
	        var railStrings = new string[railCount];
	        var startIndex = 0;

	        for (var index = 0; index < railCount; index++)
	        {
		        railStrings[index] = encodedMessage.Substring(startIndex, railLengths[index]);
		        startIndex += railLengths[index];
	        }

	        return railStrings;
        }

        private static int[] GetEachRailLength(int messageLength, int railCount)
		{
			var railLengths = new int[railCount];

			IterateElementsByZigZag(messageLength, railCount,
				(currentRail, _) => railLengths[currentRail]++);

			return railLengths;
		}

		private static void IterateElementsByZigZag(int messageLength, int railCount, Action<int, int> doAction)
		{
			var currentRail = 0;
			var isCountDown = false;

			for (var index = 0; index < messageLength; index++)
			{
				doAction?.Invoke(currentRail, index);

				if (currentRail == railCount - 1)
					isCountDown = true;
				else if (currentRail == 0)
					isCountDown = false;

				currentRail = isCountDown ? --currentRail : ++currentRail;
			}
		}
    }
}

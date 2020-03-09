using Algorithms.Encoding;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Algorithm.Encoding.Test
{
    [TestClass]
    public class HuffmanAlgorithmUnitTest
    {
        [TestMethod]
        public void HuffmanGreedyTestMethod()
        {
            var alphabetCharacters = new Dictionary<char, int>
            {
                ['a'] = 3,
                ['b'] = 2,
                ['c'] = 6,
                ['d'] = 8,
                ['e'] = 2,
                ['f'] = 6
            };

            void AssertCorrectHuffmanCode(IDictionary<char, int> alphabet, IDictionary<char, string> huffmanCodes)
            {
                Assert.AreEqual(alphabet.Count, huffmanCodes.Count, "Invalid huffman codes length");
                foreach (var alphabetKey in alphabet.Keys)
                    Assert.IsTrue(huffmanCodes.ContainsKey(alphabetKey), $"alphabet key: '{alphabetKey}' not found int huffman codes");

                foreach (var code in huffmanCodes.Values)
                    Assert.IsTrue(huffmanCodes.Values.Count(x => x.StartsWith(code)) == 1, $"Code: {code} is prefix of another code");
            }

            var huffman = new HuffmanAlgorithm();
            var codes = huffman.Encode(alphabetCharacters);
            AssertCorrectHuffmanCode(alphabetCharacters, codes);

            void AssertHuffmanCodeLength(int length, char symbol)
            {
                Assert.AreEqual(length, codes[symbol].Length, $"Invalid code length ofr symbol: '{symbol}'");
            }
            AssertHuffmanCodeLength(3, 'a');
            AssertHuffmanCodeLength(4, 'b');
            AssertHuffmanCodeLength(2, 'c');
            AssertHuffmanCodeLength(2, 'd');
            AssertHuffmanCodeLength(4, 'e');
            AssertHuffmanCodeLength(2, 'f');
        }
    }
}

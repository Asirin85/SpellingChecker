using System;
using System.Linq;
using TestApplication.Interface;

namespace TestApplication.Implementation
{
    public class LevenshteinDistance : IDistance
    {
        private const int MAX_INT_FILLING_VALUE = int.MaxValue - 1000; // const used to set unused elements to high value, not using raw int.MaxValue because of overflow problem
        /// <summary>
        /// Method to calculate minimum of three numbers
        /// </summary>
        /// <param name="delete"></param>
        /// <param name="insert"></param>
        /// <param name="substitute"></param>
        /// <returns>Minimum value and corresponding operation</returns>
        private static (int min, OperationEnum minOperation) Minimum(int delete, int insert, int substitute)
        {
            OperationEnum operation;
            int min;
            if (insert < delete && insert < substitute)
            {
                min = insert;
                operation = OperationEnum.Insertion;
            }
            else if (delete < insert && delete < substitute)
            {
                min = delete;
                operation = OperationEnum.Deletion;
            }
            else
            {
                min = substitute;
                operation = OperationEnum.Substitution;
            }
            return (min, operation);
        }
        /// <summary>
        /// Levenshtein distance algorithm with threshold and improvements based on using rows instead of a matrix
        /// Levenshtein uses three operations: deletion, insertion and substitution
        /// in this case substitution is deletion along with insertion
        /// because of that its cost equals to that of two operations
        /// </summary>
        /// <param name="firstWord"></param>
        /// <param name="secondWord"></param>
        /// <param name="threshold"></param>
        /// <returns>Distance between words or -1 if it can not be reached with current threshold</returns>
        public int CalculateDistance(string firstWord, string secondWord, int threshold = 2)
        {
            if (firstWord is null) throw new ArgumentNullException(nameof(firstWord), "First word must not be null");
            if (secondWord is null) throw new ArgumentNullException(nameof(secondWord), "Second word must not be null");
            if (threshold <= 0) throw new ArgumentException("Threshold must be set more than 0", nameof(threshold));
            if (Math.Abs(firstWord.Length - secondWord.Length) > threshold) return -1;

            var longWordInLower = firstWord.Length > secondWord.Length ? firstWord.ToLowerInvariant() : secondWord.ToLowerInvariant(); // word with bigger length is always presented as a column of a distance matrix
            var shortWordInLower = firstWord.Length > secondWord.Length ? secondWord.ToLowerInvariant() : firstWord.ToLowerInvariant();
            var n = longWordInLower.Length;
            var m = shortWordInLower.Length;

            var fillBound = Math.Min(m, threshold);
            var previous = new int[m + 1];
            for (int i = 0; i <= fillBound; previous[i] = i++) ; // first row filling up to threshold
            for (int i = fillBound + 1; i <= m; previous[i++] = MAX_INT_FILLING_VALUE) ; // first row filling after threshold
            var current = new int[m + 1];

            var prevOperations = new OperationEnum[m + 1]; // array of operations in a previous row
            var currentOperations = new OperationEnum[m + 1]; // array of current operations
            for (int i = 0; i <= m; current[i++] = MAX_INT_FILLING_VALUE) ;
            for (int i = 0; i < n; i++)
            {
                var stripeStart = Math.Max(0, i - threshold);
                var stripeEnd = Math.Min(m, i + threshold) - 1;
                if (stripeStart > 0) current[stripeStart] = MAX_INT_FILLING_VALUE;
                for (int j = stripeStart; j <= stripeEnd; j++)
                {
                    var twiceDeletion = prevOperations[j + 1] is OperationEnum.Deletion ? MAX_INT_FILLING_VALUE : 0;
                    var twiceInsertion = currentOperations[j] is OperationEnum.Insertion or OperationEnum.Substitution ? MAX_INT_FILLING_VALUE : 0;
                    var substitutionCost = longWordInLower[i] == shortWordInLower[j] ? 0 : 2;
                    var subAfterDel = prevOperations[j] is OperationEnum.Deletion && substitutionCost > 0 ? MAX_INT_FILLING_VALUE : 0;
                    (current[j + 1], currentOperations[j + 1]) = Minimum(checked(Math.Max(previous[j + 1] + 1, twiceDeletion)), checked(Math.Max(current[j] + 1, twiceInsertion)), checked(Math.Max(previous[j] + substitutionCost, subAfterDel))); // check might be necessary because of overflow
                    if (currentOperations[j + 1] is OperationEnum.Substitution && substitutionCost == 0) // check if new value didn't change and it's substitution
                    {
                        currentOperations[j + 1] = OperationEnum.None;
                    }
                }
                if (!current.Any(x => x <= threshold)) return -1; // short circuit for going over threshold
                previous = current.ToArray();
                prevOperations = currentOperations.ToArray();
            }
            if (previous[m] > threshold) return -1;
            return previous[m];
        }

    }
}

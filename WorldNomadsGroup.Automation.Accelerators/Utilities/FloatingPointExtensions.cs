using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldNomadsGroup.Automation.Accelerators.Utilities
{
 public    static class FloatingPointExtensions
    {

        public static bool LessThan(this float float1, float float2, int precision)
        {
            return (System.Math.Round(float1 - float2, precision) < 0);
        }

        /// <summary>
        /// Determines if the float value is less than or equal to the float parameter according to the defined precision.
        /// </summary>
        /// <param name="float1">The float1.</param>
        /// <param name="float2">The float2.</param>
        /// <param name="precision">The precision.  The number of digits after the decimal that will be considered when comparing.</param>
        /// <returns></returns>
        public static bool LessThanOrEqualTo(this float float1, float float2, int precision)
        {
            return (System.Math.Round(float1 - float2, precision) <= 0);
        }

        /// <summary>
        /// Determines if the float value is greater than (>) the float parameter according to the defined precision.
        /// </summary>
        /// <param name="float1">The float1.</param>
        /// <param name="float2">The float2.</param>
        /// <param name="precision">The precision.  The number of digits after the decimal that will be considered when comparing.</param>
        /// <returns></returns>
        public static bool GreaterThan(this float float1, float float2, int precision)
        {
            return (System.Math.Round(float1 - float2, precision) > 0);
        }

        /// <summary>
        /// Determines if the float value is greater than or equal to (>=) the float parameter according to the defined precision.
        /// </summary>
        /// <param name="float1">The float1.</param>
        /// <param name="float2">The float2.</param>
        /// <param name="precision">The precision.  The number of digits after the decimal that will be considered when comparing.</param>
        /// <returns></returns>
        public static bool GreaterThanOrEqualTo(this float float1, float float2, int precision)
        {
            return (System.Math.Round(float1 - float2, precision) >= 0);
        }

        /// <summary>
        /// Determines if the float value is equal to (==) the float parameter according to the defined precision.
        /// </summary>
        /// <param name="float1">The float1.</param>
        /// <param name="float2">The float2.</param>
        /// <param name="precision">The precision.  The number of digits after the decimal that will be considered when comparing.</param>
        /// <returns></returns>
        public static bool AlmostEquals(this float float1, float float2, int precision)
        {
            return (System.Math.Round(float1 - float2, precision) == 0);
        }


        public static bool AlmostEqualsPlusMinusTenDecimal(this double float1, double float2)
        {
            try
            {
                double decimalDifference = 0.10;
                double actualDifferenceDifference =
                    (float1 - float2);

                if (Math.Abs(actualDifferenceDifference) <= 0.11 )
                {
                    return true;
                }
                else if (actualDifferenceDifference > decimalDifference)
                {
                    return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
    }

}

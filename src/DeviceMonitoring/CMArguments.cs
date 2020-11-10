using System;
using System.Linq;

namespace DeviceMonitoring
{
    public static class CMArguments
    {
        /// <summary>
        /// Get int argument from <c>Command line arguments</c>
        /// </summary>
        /// <param name="array">Array with command line arguments</param>
        /// <param name="ArgumentName">Searched argument name</param>
        /// <param name="DefaultValue">Default value of argument</param>
        /// <param name="Aliases">Aliases of this argument</param>
        /// <exception cref="ArgumentNullException">Occurs when value or argument not found</exception>
        /// <returns></returns>
        public static int GetIntArgument(this string[] array, string ArgumentName, int? DefaultValue = null, params string[] Aliases)
        {
            int index = Array.FindIndex(array, x => Aliases.Contains(x.ToLower())) + 1;

            if (index > 0 && index < array.Length && int.TryParse(array[index], out int i))
                return i;
            else if (DefaultValue.HasValue)
                return DefaultValue.Value;

            throw new ArgumentNullException(ArgumentName);
        }

        /// <summary>
        /// Get string argument from <c>Command line arguments</c>
        /// </summary>
        /// <param name="array">Array with command line arguments</param>
        /// <param name="ArgumentName">Searched argument name</param>
        /// <param name="DefaultValue">Default value of argument</param>
        /// <param name="Aliases">Aliases of this argument</param>
        /// <exception cref="ArgumentNullException">Occurs when value or argument not found</exception>
        /// <returns></returns>
        public static string GetStringArgument(this string[] array, string ArgumentName, string DefaultValue = null, params string[] Aliases)
        {
            int index = Array.FindIndex(array, x => Aliases.Contains(x.ToLower()));

            if (index > 0 && index < array.Length)
                return array[index].Replace("'", "").Replace("\"", "");
            else
                return DefaultValue;
        }
    }
}

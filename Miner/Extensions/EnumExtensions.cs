using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Miner.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets next enum value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <returns></returns>
        public static T GetNextValue<T>(T e)
        {
            T[] all = (T[])Enum.GetValues(typeof(T));
            int i = Array.IndexOf(all, e);
            if (i < 0)
                throw new InvalidEnumArgumentException();
            if (i == all.Length - 1)
                i = -1;
            return all[i + 1];
        }
    }
}

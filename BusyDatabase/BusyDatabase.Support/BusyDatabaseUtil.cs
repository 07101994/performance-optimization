﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace BusyDatabase.Support
{
    public class BusyDatabaseUtil
    {
        private static readonly Dictionary<string, string> Query = new Dictionary<string, string>();

        private const string ResourceFileBase = "BusyDatabase.Support.";

        public static string GetQuery(string word)
        {
            // Try to get the result in the static Dictionary
            string result;
            if (Query.TryGetValue(word, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public static void PutQuery(string key, string file )
        {                    
            Query.Add(key, GetSqlQuery(file));
        }

        private static string GetSqlQuery(string file)
        {
            var resFile = ResourceFileBase + file;

            // Get the assembly that this class is in
            var assembly = Assembly.GetAssembly(typeof(BusyDatabaseUtil));

            var stream = assembly.GetManifestResourceStream(resFile);

            if (null == stream)
            {
                var resourceList = assembly.GetManifestResourceNames();

                throw new ApplicationException(string.Format("Could not find resource: {0} in [{1}]", resFile, String.Join(" , ", resourceList)));
            }

            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}

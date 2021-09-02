using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DanilovSoft.Radius
{
    internal static class PenDictionary
    {
        // С какой строки начать разбор файла.
        private const int StartLine = 20;
        private const string FileName = "PRIVATE ENTERPRISE NUMBERS.txt";
        private static WeakReference<Dictionary<int, PenVendorInfo>> _weakDict;
        private static readonly object _obj = new object();

        public static PenVendorInfo TryGetVendorInfo(int vendorId)
        {
            lock (_obj)
            {
                Dictionary<int, PenVendorInfo> dict;
                if (_weakDict == null)
                {
                    dict = new Dictionary<int, PenVendorInfo>();
                    _weakDict = new WeakReference<Dictionary<int, PenVendorInfo>>(dict);
                }
                else
                {
                    if(!_weakDict.TryGetTarget(out dict))
                    {
                        dict = new Dictionary<int, PenVendorInfo>();
                        _weakDict.SetTarget(dict);
                    }
                }

                if (dict.TryGetValue(vendorId, out PenVendorInfo vendorInfo))
                {
                    return vendorInfo;
                }
                else
                {
                    TryLoad(vendorId, out vendorInfo);
                    dict.Add(vendorId, vendorInfo);
                    return vendorInfo;
                }
            }
        }

        private static bool TryLoad(int vendorId, out PenVendorInfo vendorInfo)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string[] resources = assembly.GetManifestResourceNames();
            string fileName = resources.FirstOrDefault(x => x.EndsWith(FileName));
            if (fileName != null)
            {
                using (Stream stream = assembly.GetManifestResourceStream(fileName))
                {
                    int lineNum = 0;
                    int skipLines = 0;
                    using (var sr = new StreamReader(stream))
                    {
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            lineNum++;
                            if (lineNum >= StartLine)
                            {
                                if(skipLines != 0)
                                {
                                    skipLines--;
                                    continue;
                                }

                                if (int.TryParse(line, out int vId))
                                {
                                    if(vId == vendorId)
                                    {
                                        string organization = sr.ReadLine();
                                        if (sr.EndOfStream)
                                            goto exit;
                                        string contact = sr.ReadLine();
                                        if (sr.EndOfStream)
                                            goto exit;
                                        string email = sr.ReadLine();
                                        if (sr.EndOfStream)
                                            goto exit;

                                        vendorInfo = new PenVendorInfo(organization.Trim(), contact.Trim(), email.Trim());
                                        return true;
                                    }
                                    else
                                    {
                                        skipLines = 3;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            exit:
            vendorInfo = null;
            return false;
        }
    }
}

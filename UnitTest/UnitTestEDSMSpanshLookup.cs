using EliteDangerousCore;
using EliteDangerousCore.EDSM;
using EliteDangerousCore.JournalEvents;
using EliteDangerousCore.Spansh;
using QuickJSON;
using System;
using System.Collections.Generic;
using System.IO;
using static BaseUtils.UnitTests.CheckerHelpers;

namespace UnitTest
{
    public static class UnitTestEDSMLookup
    {
        [BaseUtils.UnitTests.Test]
        public static void SpanshEDSMLookup()
        {

#if false
            // grab edsm data for making test files
            if ( false )
            {
                EDSMClass edsm = new EDSMClass();
                var sa = new SystemAddress(83919508178);
                var jobj = edsm.GetBodiesByID64(sa);
                File.WriteAllText($@"c:\code\edsmv4__{jobj["name"].Str()}__{sa.Value.ToStringInvariant()}.json", jobj.ToString(true));
            }

            if ( true)
            {
                
                EDSMClass edsm = new EDSMClass();
                var sa = new SystemAddress(1178708478315Ul);
                SystemClass sys = new SystemClass("Alpha Centuri", sa);
                var bodyresults = EDSMClass.GetBodiesList(sys, true);

            }
#endif

            CheckSection("SpanshEDSMLookup");

            if (!Directory.Exists(outpath))
                outpath = null;

            var flist = Directory.EnumerateFiles(testfolder, "*.json");

            foreach( var filepath in flist)
            {
                var ss = new EliteDangerousCore.StarScan2.StarScan();

                string fn = Path.GetFileNameWithoutExtension(filepath);
                int firstdouble = fn.IndexOf("__")+2;
                int seconddouble = fn.IndexOf("__", firstdouble);
                SystemClass sys = new SystemClass(fn.Substring(firstdouble, seconddouble - firstdouble), new SystemAddress(fn.Substring(seconddouble + 2).InvariantParseULong(0)));
                string source = fn.Substring(0, firstdouble-2);

                JObject dump = JObject.Parse(File.ReadAllText(filepath));
                CheckThat(dump).IsNotNull();

                if (source.EqualsIIC("spanshv4"))
                {
                    JArray jscans = SpanshClass.GetJournalScansJsonsFromDump(dump);
                    CheckThat(jscans).IsNotNull();

                    foreach (JObject scan in jscans)
                    {
                        JournalScan js = new JournalScan(scan);
                        CheckThat(js.SystemAddress).Is(sys.SystemAddress);
                        ss.AddJournalScan(js, new SystemClass(js.StarSystem, js.SystemAddress));
                    }
                }
                else
                {
                    SystemClass rxsys = new SystemClass(dump["name"].Str("Unknown"), new SystemAddress(dump["id64"].ULong()));

                    foreach (JObject edsmbody in dump["bodies"])
                    {
                        try
                        {
                            JObject jbody = EDSMClass.ConvertFromEDSMBodies(edsmbody, rxsys);

                            JournalScan js = new JournalScan(jbody);
                            ss.AddJournalScan(js, new SystemClass(js.StarSystem, js.SystemAddress));
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.Assert(false,$"EDSM Excepted {ex}");
                        }
                    }

                }

                var node = ss.FindSystemSynchronous(sys, false);
                CheckThat(node).IsNotNull();

                if (outpath != null)
                {
                    string path = Path.Combine(outpath, source + "__" + node.System.Name.SafeFileString()) + $"-{sys.Name}.png";
                    node.DrawSystemToFile(path, 1920, true);
                }
            }


        }

        static string testfolder = $@"..\..\..\UnitTest\WebLookups\";
        static string outpath = @"c:\code\images\dump";
    }
}


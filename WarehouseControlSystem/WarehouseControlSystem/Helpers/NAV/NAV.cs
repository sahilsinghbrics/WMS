// ----------------------------------------------------------------------------------
// Copyright © 2017, Oleg Lobakov, Contacts: <oleg.lobakov@gmail.com>
// Licensed under the GNU GENERAL PUBLIC LICENSE, Version 3.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// https://github.com/OlegLobakov/WarehouseControlSystem/blob/master/LICENSE
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using WarehouseControlSystem.Model.NAV;
using WarehouseControlSystem.Model;
using Xamarin.Forms;

namespace WarehouseControlSystem.Helpers.NAV
{
    public class NAV
    {
        public class SoapParams
        {
            public string FunctionName { get; set; }
            public XElement SoapBody { get; set; }
            public XNamespace NameSpace { get; set; }

            public SoapParams(string functionname, XElement sb, XNamespace ns)
            {
                FunctionName = functionname;
                SoapBody = sb;
                NameSpace = ns;
            }
        }

        static XNamespace ns = "http://schemas.xmlsoap.org/soap/envelope/";

        private static XNamespace GetNameSpace()
        {
            XNamespace myns = "urn:microsoft-dynamics-schemas/codeunit/" + Global.CurrentConnection.Codeunit;
            return myns;
        }

        private static bool IsConnectionFaild()
        {
            if (!(Global.CurrentConnection is Connection))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static async Task GetIntFromNAV(TaskCompletionSource<int> tcs, SoapParams sp, CancellationTokenSource cts)
        {
            try
            {
                XElement soapbodynode = await Process(sp, false, cts).ConfigureAwait(false);
                if (string.IsNullOrEmpty(soapbodynode.Value))
                {
                    tcs.SetResult(0);
                }
                else
                {
                    tcs.SetResult(StringToInt(soapbodynode.Value));
                }
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
        }
        private static async Task GetStringFromNAV(TaskCompletionSource<string> tcs, SoapParams sp, CancellationTokenSource cts)
        {
            try
            {
                XElement soapbodynode = await Process(sp, false, cts).ConfigureAwait(false);
                if (string.IsNullOrEmpty(soapbodynode.Value))
                {
                    tcs.SetResult("");
                }
                else
                {
                    tcs.SetResult(soapbodynode.Value);
                }
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
        }

        public static Task<int> GetPlanWidth(CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(0);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "GetPlanWidth";
            XElement body = new XElement(myns + functionname);
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetIntFromNAV(tcs, sp, cts));
            return tcs.Task;
        }
        public static Task<int> GetPlanHeight(CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();

            if (IsConnectionFaild())
            {
                tcs.SetResult(0);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "GetPlanHeight";
            XElement body = new XElement(myns + functionname);
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetIntFromNAV(tcs, sp, cts));
            return tcs.Task;
        }
        public static Task<int> SetPlanWidth(int value, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();

            if (IsConnectionFaild())
            {
                tcs.SetResult(0);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "SetPlanWidth";
            XElement body = new XElement(myns + functionname,
                new XElement(myns + "value", value));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetIntFromNAV(tcs, sp, cts));
            return tcs.Task;
        }
        public static Task<int> SetPlanHeight(int value, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(0);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "SetPlanHeight";
            XElement body = new XElement(myns + functionname,
                new XElement(myns + "value", value));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetIntFromNAV(tcs, sp, cts));

            return tcs.Task;
        }
        public static Task<int> APIVersion(CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(0);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "APIVersion";
            XElement body = new XElement(myns + functionname);
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetIntFromNAV(tcs, sp, cts));
            return tcs.Task;
        }


        public static XElement[] PositionCount(XNamespace myns, int position, int count)
        {
            return new XElement[]
            {
              new XElement(myns + "entriesPosition", position),
              new XElement(myns + "entriesCount", count),
              new XElement(myns + "responseDocument", "")
            };
        }

        public static XElement[] SetLocationParams(XNamespace myns, Location location)
        {
            return new XElement[]
            {
                new XElement(myns + "name", location.Name),
                new XElement(myns + "address", location.Address),
                new XElement(myns + "phoneNo", location.PhoneNo),
                new XElement(myns + "hexColor", location.HexColor),
                new XElement(myns + "planWidth", location.PlanWidth),
                new XElement(myns + "planHeight", location.PlanHeight),
                new XElement(myns + "left", location.Left),
                new XElement(myns + "top", location.Top),
                new XElement(myns + "width", location.Width),
                new XElement(myns + "height", location.Height),
                new XElement(myns + "schemeVisible", location.SchemeVisible),
                new XElement(myns + "binMandatory", location.BinMandatory),
                new XElement(myns + "requireReceive", location.RequireReceive),
                new XElement(myns + "requireShipment", location.RequireShipment),
                new XElement(myns + "requirePick", location.RequirePick),
                new XElement(myns + "requirePutaway", location.RequirePutaway)
            };
        }

        #region Location
        public static Task<int> CreateLocation(Location location, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();

            if (IsConnectionFaild())
            {
                tcs.SetResult(0);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "CreateLocation";
            XElement body =
                new XElement(myns + functionname,
                    new XElement(myns + "code", location.Code),
                    SetLocationParams(myns, location));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetIntFromNAV(tcs, sp, cts));
            return tcs.Task;
        }
        public static Task<int> ModifyLocation(Location location, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(0);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "ModifyLocation";
            XElement body =
                new XElement(myns + functionname,
                    new XElement(myns + "code", location.Code),
                    new XElement(myns + "prevCode", location.PrevCode),
                    SetLocationParams(myns, location));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetIntFromNAV(tcs, sp, cts));
            return tcs.Task;
        }
        public static Task<int> SetLocationVisible(Location location, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(0);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "SetLocationVisible";
            XElement body =
            new XElement(myns + functionname,
                new XElement(myns + "code", location.Code),
                new XElement(myns + "visible", location.SchemeVisible.ToString()));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetIntFromNAV(tcs, sp, cts));

            return tcs.Task;
        }
        public static Task<int> DeleteLocation(string locationcode, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(0);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "DeleteLocation";
            XElement body =
                new XElement(myns + functionname,
                    new XElement(myns + "code", locationcode));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetIntFromNAV(tcs, sp, cts));
            return tcs.Task;
        }
        public static Task<int> GetLocationCount(string codefilter, bool onlyvisibled, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(0);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "GetLocationCount";
            XElement body =
                new XElement(myns + functionname,
                    new XElement(myns + "codeFilter", codefilter),
                    new XElement(myns + "onlyVisibled", onlyvisibled.ToString()));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetIntFromNAV(tcs, sp, cts));
            return tcs.Task;
        }
        public static Task<List<Location>> GetLocationList(string codefilter, bool onlyvisibled1, int position1, int count1, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<List<Location>>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(null);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "GetLocationList";

            XElement body =
                new XElement(myns + functionname,
                    new XElement(myns + "codeFilter", codefilter),
                    new XElement(myns + "onlyVisibled", onlyvisibled1.ToString()),
                    PositionCount(myns, position1, count1));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetLocationListFromNAV(tcs, sp, cts));
            return tcs.Task;
        }
        private static async Task GetLocationListFromNAV(TaskCompletionSource<List<Location>> tcs, SoapParams sp, CancellationTokenSource cts)
        {
            try
            {
                XElement soapbodynode = await Process(sp, false, cts).ConfigureAwait(false);
                List<Location> rv = new List<Location>();
                string response = soapbodynode.Value;
                XDocument document = GetDoc(response);
                foreach (XElement currentnode in document.Root.Elements())
                {
                    Location location = GetLocationFromXML(currentnode);
                    rv.Add(location);
                }
                tcs.SetResult(rv);
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
        }

        private static Location GetLocationFromXML(XElement currentnode)
        {
            Location location = new Location();
            foreach (XAttribute currentatribute in currentnode.Attributes())
            {
                GetLocationFromXML1(location, currentatribute);
                GetLocationFromXML2(location, currentatribute);
                GetLocationFromXML3(location, currentatribute);
                GetLocationFromXML4(location, currentatribute);
            }
            return location;
        }
        private static void GetLocationFromXML1(Location location, XAttribute currentatribute)
        {
            switch (currentatribute.Name.LocalName)
            {
                case "Code":
                    location.Code = currentatribute.Value;
                    location.PrevCode = location.Code;
                    break;
                case "Name":
                    location.Name = currentatribute.Value;
                    break;
                case "Address":
                    location.Address = currentatribute.Value;
                    break;
                case "PhoneNo":
                    location.PhoneNo = currentatribute.Value;
                    break;
                case "HexColor":
                    location.HexColor = currentatribute.Value;
                    break;
            }
        }
        private static void GetLocationFromXML2(Location location, XAttribute currentatribute)
        {
            switch (currentatribute.Name.LocalName)
            {
                case "Left":
                    location.Left = StringToInt(currentatribute.Value);
                    break;
                case "Top":
                    location.Top = StringToInt(currentatribute.Value);
                    break;
                case "Width":
                    location.Width = StringToInt(currentatribute.Value);
                    break;
                case "Height":
                    location.Height = StringToInt(currentatribute.Value);
                    break;
                case "PlanWidth":
                    location.PlanWidth = StringToInt(currentatribute.Value);
                    break;
                case "PlanHeight":
                    location.PlanHeight = StringToInt(currentatribute.Value);
                    break;
            }
        }
        private static void GetLocationFromXML3(Location location, XAttribute currentatribute)
        {
            switch (currentatribute.Name.LocalName)
            {
                case "BinMandatory":
                    location.BinMandatory = StringToBool(currentatribute.Value);
                    break;
                case "RequireReceive":
                    location.RequireReceive = StringToBool(currentatribute.Value);
                    break;
                case "RequireShipment":
                    location.RequireShipment = StringToBool(currentatribute.Value);
                    break;
                case "RequirePick":
                    location.RequirePick = StringToBool(currentatribute.Value);
                    break;
                case "RequirePutaway":
                    location.RequirePutaway = StringToBool(currentatribute.Value);
                    break;
            }
        }
        private static void GetLocationFromXML4(Location location, XAttribute currentatribute)
        {
            switch (currentatribute.Name.LocalName)
            {
                case "UseAsInTransit":
                    location.Transit = StringToBool(currentatribute.Value);
                    break;
                case "ZoneQuantity":
                    location.ZoneQuantity = StringToInt(currentatribute.Value);
                    break;
                case "BinQuantity":
                    location.BinQuantity = StringToInt(currentatribute.Value);
                    break;
                case "SchemeVisible":
                    location.SchemeVisible = StringToBool(currentatribute.Value);
                    break;
            }
        }

        public static Task<List<Indicator>> GetIndicatorsByLocation(string locationcode, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<List<Indicator>>();

            if (IsConnectionFaild())
            {
                tcs.SetResult(null);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "GetIndicatorsByLocation";
            XElement body =
                new XElement(myns + functionname,
                    new XElement(myns + "locationCode", locationcode),
                    new XElement(myns + "responseDocument", ""));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetIndicatorsByLocationNAV(tcs, sp, cts));
            return tcs.Task;
        }
        private static async Task GetIndicatorsByLocationNAV(TaskCompletionSource<List<Indicator>> tcs, SoapParams sp, CancellationTokenSource cts)
        {
            try
            {
                XElement soapbodynode = await Process(sp, false, cts).ConfigureAwait(false);
                string response = soapbodynode.Value;
                XDocument document = GetDoc(response);
                List<Indicator> rv = new List<Indicator>();
                foreach (XElement currentnode in document.Root.Elements())
                {
                    Indicator bi = GetIndicatorsByLocationFromXML(currentnode);
                    rv.Add(bi);
                }
                tcs.SetResult(rv);
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
        }
        private static Indicator GetIndicatorsByLocationFromXML(XElement currentnode)
        {
            Indicator ind = new Indicator();
            foreach (XAttribute currentatribute in currentnode.Attributes())
            {
                switch (currentatribute.Name.LocalName)
                {
                    case "Header":
                        {
                            ind.Header = currentatribute.Value;
                            break;
                        }
                    case "Description":
                        {
                            ind.Description = currentatribute.Value;
                            break;
                        }
                    case "Value":
                        {
                            ind.Value = currentatribute.Value;
                            break;
                        }
                    case "Color":
                        {
                            ind.ValueColor = currentatribute.Value;
                            break;
                        }
                    case "Position":
                        {
                            ind.Position = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "URL":
                        {
                            ind.URL = currentatribute.Value;
                            break;
                        }
                    case "ID":
                        {
                            ind.ID = currentatribute.Value;
                            break;
                        }
                    case "Parameters":
                        {
                            ind.Parameters = currentatribute.Value;
                            break;
                        }
                }
            }
            return ind;
        }

        #endregion

        #region Zone
        public static XElement[] SetZoneParams(XNamespace myns, Zone zone)
        {
            return new XElement[]
            {
                new XElement(myns + "description", zone.Description),
                new XElement(myns + "binTypeCode", zone.BinTypeCode),
                new XElement(myns + "zoneRanking", zone.ZoneRanking),
                new XElement(myns + "crossDockBinZone", zone.CrossDockBinZone),
                new XElement(myns + "specialEquipmentCode", zone.SpecialEquipmentCode),
                new XElement(myns + "warehouseClassCode", zone.WarehouseClassCode),
                new XElement(myns + "hexColor", zone.HexColor),
                new XElement(myns + "planWidth", zone.PlanWidth),
                new XElement(myns + "planHeight", zone.PlanHeight),
                new XElement(myns + "left", zone.Left),
                new XElement(myns + "top", zone.Top),
                new XElement(myns + "width", zone.Width),
                new XElement(myns + "height", zone.Height),
                new XElement(myns + "schemeVisible", zone.SchemeVisible)
            };
        }
        public static Task<int> CreateZone(Zone zone, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(0);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "CreateZone";
            XElement body =
                new XElement(myns + functionname,
                new XElement(myns + "locationCode", zone.LocationCode),
                new XElement(myns + "code", zone.Code),
                SetZoneParams(myns, zone));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetIntFromNAV(tcs, sp, cts));
            return tcs.Task;
        }
        public static Task<int> ModifyZone(Zone zone, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(0);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "ModifyZone";
            XElement body =
                new XElement(myns + functionname,
                new XElement(myns + "locationCode", zone.LocationCode),
                new XElement(myns + "code", zone.Code),
                new XElement(myns + "prevCode", zone.PrevCode),
                SetZoneParams(myns, zone));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetIntFromNAV(tcs, sp, cts));
            return tcs.Task;
        }
        public static Task<int> SetZoneVisible(Zone zone, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(0);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "SetZoneVisible";
            XElement body =
                new XElement(myns + functionname,
                new XElement(myns + "locationCode", zone.LocationCode),
                new XElement(myns + "code", zone.Code),
                new XElement(myns + "visible", zone.SchemeVisible));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetIntFromNAV(tcs,sp , cts));
            return tcs.Task;
        }
        public static Task<int> DeleteZone(Zone zone, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(0);
                return tcs.Task;
            }
            XNamespace myns = GetNameSpace();
            string functionname = "DeleteZone";
            XElement body =
                new XElement(myns + functionname,
                    new XElement(myns + "locationCode", zone.LocationCode),
                    new XElement(myns + "code", zone.Code));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetIntFromNAV(tcs, sp, cts));
            return tcs.Task;
        }
        public static Task<int> GetZoneCount(string locationfilter, string codefilter, bool onlyvisibled, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(0);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "GetZoneCount";
            XElement body =
                new XElement(myns + functionname,
                            new XElement(myns + "locationFilter", locationfilter),
                            new XElement(myns + "codeFilter", codefilter),
                            new XElement(myns + "onlyVisibled", onlyvisibled));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetIntFromNAV(tcs, sp, cts));
            return tcs.Task;
        }
        public static Task<List<Zone>> GetZoneList(string locationfilter, string codefilter2, bool onlyvisibled2, int position2, int count2, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<List<Zone>>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(null);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "GetZoneList";
            XElement body =
                new XElement(myns + functionname,
                    new XElement(myns + "locationFilter", locationfilter),
                    new XElement(myns + "codeFilter", codefilter2),
                    new XElement(myns + "onlyVisibled", onlyvisibled2),
                    PositionCount(myns, position2, count2));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetZoneListNAV(tcs, sp, cts));
            return tcs.Task;
        }
        private static async Task GetZoneListNAV(TaskCompletionSource<List<Zone>> tcs, SoapParams sp, CancellationTokenSource cts)
        {
            try
            {
                XElement soapbodynode = await Process(sp, false, cts).ConfigureAwait(false);
                string response = soapbodynode.Value;
                XDocument document = GetDoc(response);
                List<Zone> rv = new List<Zone>();
                foreach (XElement currentnode in document.Root.Elements())
                {
                    Zone zone = GetZoneFromXML(currentnode);
                    rv.Add(zone);
                }
                tcs.SetResult(rv);
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
        }
        private static Zone GetZoneFromXML(XElement currentnode)
        {
            Zone zone = new Zone();
            foreach (XAttribute currentatribute in currentnode.Attributes())
            {
                GetZoneFromXML1(zone, currentatribute);
                GetZoneFromXML2(zone, currentatribute);
                GetZoneFromXML3(zone, currentatribute);
            }
            return zone;
        }
        private static void GetZoneFromXML1(Zone zone, XAttribute currentatribute)
        {
            switch (currentatribute.Name.LocalName)
            {
                case "LocationCode":
                    zone.LocationCode = currentatribute.Value;
                    break;
                case "Code":
                    zone.Code = currentatribute.Value;
                    zone.PrevCode = zone.Code;
                    break;
                case "Description":
                    zone.Description = currentatribute.Value;
                    break;
                case "BinTypeCode":
                    zone.BinTypeCode = currentatribute.Value;
                    break;
                case "CrossDockBinZone":
                    zone.CrossDockBinZone = StringToBool(currentatribute.Value);
                    break;
                case "HexColor":
                    zone.HexColor = currentatribute.Value;
                    break;
            }
        }
        private static void GetZoneFromXML2(Zone zone, XAttribute currentatribute)
        {
            switch (currentatribute.Name.LocalName)
            {
                case "Left":
                    zone.Left = StringToInt(currentatribute.Value);
                    break;
                case "Top":
                    zone.Top = StringToInt(currentatribute.Value);
                    break;
                case "Width":
                    zone.Width = StringToInt(currentatribute.Value);
                    break;
                case "Height":
                    zone.Height = StringToInt(currentatribute.Value);
                    break;
                case "PlanWidth":
                    zone.PlanWidth = StringToInt(currentatribute.Value);
                    break;
                case "PlanHeight":
                    zone.PlanHeight = StringToInt(currentatribute.Value);
                    break;
            }
        }
        private static void GetZoneFromXML3(Zone zone, XAttribute currentatribute)
        {
            switch (currentatribute.Name.LocalName)
            {
                case "SpecialEquipmentCode":

                    zone.SpecialEquipmentCode = currentatribute.Value;
                    break;
                case "WarehouseClassCode":
                    zone.WarehouseClassCode = currentatribute.Value;
                    break;
                case "BinQuantity":
                    zone.BinQuantity = StringToInt(currentatribute.Value);
                    break;
                case "RackQuantity":
                    zone.RackQuantity = StringToInt(currentatribute.Value);
                    break;
                case "SchemeVisible":
                    zone.SchemeVisible = StringToBool(currentatribute.Value);
                    break;
            }
        }

        public static Task<List<Indicator>> GetIndicatorsByZone(string locationcode, string zonecode, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<List<Indicator>>();

            if (IsConnectionFaild())
            {
                tcs.SetResult(null);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "GetIndicatorsByZone";
            XElement body =
                new XElement(myns + functionname,
                    new XElement(myns + "locationCode", locationcode),
                    new XElement(myns + "zoneCode", zonecode),
                    new XElement(myns + "responseDocument", ""));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetIndicatorsByZoneNAV(tcs, sp, cts));
            return tcs.Task;
        }
        private static async Task GetIndicatorsByZoneNAV(TaskCompletionSource<List<Indicator>> tcs, SoapParams sp, CancellationTokenSource cts)
        {
            try
            {
                XElement soapbodynode = await Process(sp, false, cts).ConfigureAwait(false);
                string response = soapbodynode.Value;
                XDocument document = GetDoc(response);
                List<Indicator> rv = new List<Indicator>();
                foreach (XElement currentnode in document.Root.Elements())
                {
                    Indicator bi = GetIndicatorsByZoneFromXML(currentnode);
                    rv.Add(bi);
                }
                tcs.SetResult(rv);
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
        }
        private static Indicator GetIndicatorsByZoneFromXML(XElement currentnode)
        {
            Indicator ind = new Indicator();
            foreach (XAttribute currentatribute in currentnode.Attributes())
            {
                switch (currentatribute.Name.LocalName)
                {
                    case "Header":
                        {
                            ind.Header = currentatribute.Value;
                            break;
                        }
                    case "Description":
                        {
                            ind.Description = currentatribute.Value;
                            break;
                        }
                    case "Value":
                        {
                            ind.Value = currentatribute.Value;
                            break;
                        }
                    case "Color":
                        {
                            ind.ValueColor = currentatribute.Value;
                            break;
                        }
                    case "Position":
                        {
                            ind.Position = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "URL":
                        {
                            ind.URL = currentatribute.Value;
                            break;
                        }
                    case "ID":
                        {
                            ind.ID = currentatribute.Value;
                            break;
                        }
                    case "Parameters":
                        {
                            ind.ID = currentatribute.Value;
                            break;
                        }
                }
            }
            return ind;
        }
        #endregion


        #region Indicator Content
        public static Task<List<IndicatorContent>> GetIndicatorContent(string locationcode, string zonecode, string bincode, string id, string parameters, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<List<IndicatorContent>>();

            if (IsConnectionFaild())
            {
                tcs.SetResult(null);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "LoadIndicatorContent";
            XElement body =
                new XElement(myns + functionname,
                    new XElement(myns + "locationCode", locationcode),
                    new XElement(myns + "zoneCode", zonecode),
                    new XElement(myns + "binCode", bincode),
                    new XElement(myns + "iD", id),
                    new XElement(myns + "parameters", parameters),
                    new XElement(myns + "responseDocument", ""));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetGetIndicatorContentNAV(tcs, sp, cts));
            return tcs.Task;
        }
        private static async Task GetGetIndicatorContentNAV(TaskCompletionSource<List<IndicatorContent>> tcs, SoapParams sp, CancellationTokenSource cts)
        {
            try
            {
                XElement soapbodynode = await Process(sp, false, cts).ConfigureAwait(false);
                string response = soapbodynode.Value;
                XDocument document = GetDoc(response);
                List<IndicatorContent> rv = new List<IndicatorContent>();
                foreach (XElement currentnode in document.Root.Elements())
                {
                    IndicatorContent bi = GetGetIndicatorContentFromXML(currentnode);
                    rv.Add(bi);
                }
                tcs.SetResult(rv);
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
        }
        private static IndicatorContent GetGetIndicatorContentFromXML(XElement currentnode)
        {
            IndicatorContent ind = new IndicatorContent();
            foreach (XAttribute currentatribute in currentnode.Attributes())
            {
                switch (currentatribute.Name.LocalName)
                {
                    case "Header":
                        {
                            ind.Header = currentatribute.Value;
                            break;
                        }
                    case "Description":
                        {
                            ind.Description = currentatribute.Value;
                            break;
                        }
                    case "Detail":
                        {
                            ind.Detail = currentatribute.Value;
                            break;
                        }
                    case "LeftValue":
                        {
                            ind.LeftValue = currentatribute.Value;
                            break;
                        }
                    case "RightValue":
                        {
                            ind.RightValue = currentatribute.Value;
                            break;
                        }
                    case "Color":
                        {
                            ind.Color = currentatribute.Value;
                            break;
                        }
                    case "SortOrder":
                        {
                            ind.SortOrder = StringToDec(currentatribute.Value);
                            break;
                        }
                }
            }
            return ind;
        }

        #endregion
        #region Rack
        public static XElement[] SetRackParams(XNamespace myns, Rack rack)
        {
            return new XElement[]
            {
                new XElement(myns + "sections", rack.Sections),
                new XElement(myns + "levels", rack.Levels),
                new XElement(myns + "depth", rack.Depth),
                new XElement(myns + "left", rack.Left),
                new XElement(myns + "top", rack.Top),
                new XElement(myns + "width", rack.Width),
                new XElement(myns + "height", rack.Height),
                new XElement(myns + "rackOrientation", (int)rack.RackOrientation),
                new XElement(myns + "schemeVisible", rack.SchemeVisible),
                new XElement(myns + "comment", rack.Comment),
                new XElement(myns + "numberingPrefix", rack.NumberingPrefix),
                new XElement(myns + "rackSectionSeparator", rack.RackSectionSeparator),
                new XElement(myns + "sectionLevelSeparator", rack.SectionLevelSeparator),
                new XElement(myns + "levelDepthSeparator", rack.LevelDepthSeparator),
                new XElement(myns + "reversSectionNumbering", rack.ReversSectionNumbering),
                new XElement(myns + "reversLevelNumbering", rack.ReversLevelNumbering),
                new XElement(myns + "reversDepthNumbering", rack.ReversDepthNumbering),
                new XElement(myns + "numberingSectionBegin", rack.NumberingSectionBegin),
                new XElement(myns + "numberingLevelBegin", rack.NumberingLevelBegin),
                new XElement(myns + "numberingDepthBegin", rack.NumberingDepthBegin),
                new XElement(myns + "numberingSectionDigitsQty", rack.NumberingSectionDigitsQuantity),
                new XElement(myns + "numberingLevelDigitsQty", rack.NumberingLevelDigitsQuantity),
                new XElement(myns + "numberingDepthDigitsQty", rack.NumberingDepthDigitsQuantity),
                new XElement(myns + "stepNumberingSection", rack.StepNumberingSection),
                new XElement(myns + "stepNumberingLevel", rack.StepNumberingLevel),
                new XElement(myns + "stepNumberingDepth", rack.StepNumberingDepth),
                new XElement(myns + "binTemplateCode", rack.BinTemplateCode)
            };
        }
        public static Task<int> CreateRack(Rack rack, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(0);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "CreateRack";
            XElement body =
                new XElement(myns + functionname,
                new XElement(myns + "locationCode", rack.LocationCode),
                new XElement(myns + "zoneCode", rack.ZoneCode),
                new XElement(myns + "no", rack.No),
                SetRackParams(myns,rack));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetIntFromNAV(tcs, sp, cts));
            return tcs.Task;
        }
        public static Task<int> ModifyRack(Rack rack, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(0);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "ModifyRack";
            XElement body =
                new XElement(myns + functionname,
                new XElement(myns + "iD", rack.ID),
                new XElement(myns + "no", rack.No),
                SetRackParams(myns, rack));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetIntFromNAV(tcs, sp, cts));
            return tcs.Task;
        }
        public static Task<int> DeleteRack(int rackid, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(0);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "DeleteRack";
            XElement body =
                new XElement(myns + functionname,
                new XElement(myns + "iD", rackid));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetIntFromNAV(tcs, sp, cts));
            return tcs.Task;
        }
        public static Task<int> DeleteBinsFromRack(Rack rack, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(0);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "DeleteBinsFromRack";
            XElement body =
                new XElement(myns + functionname,
                new XElement(myns + "iD", rack.ID));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetIntFromNAV(tcs, sp, cts));
            return tcs.Task;
        }
        public static Task<int> SetRackVisible(Rack rack, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(0);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "SetRackVisible";
            XElement body =
                new XElement(myns + functionname,
                    new XElement(myns + "iD", rack.ID),
                    new XElement(myns + "visible", rack.SchemeVisible));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetIntFromNAV(tcs, sp, cts));
            return tcs.Task;
        }
        public static Task<int> GetRackCount(string locationfilter, string codefilter, string nofilter, bool onlyvisibled, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(0);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "GetRackCount";
            XElement body =
                new XElement(myns + functionname,
                    new XElement(myns + "locationCodeFilter", locationfilter),
                    new XElement(myns + "zoneCodeFilter", codefilter),
                    new XElement(myns + "nOFilter", nofilter),
                    new XElement(myns + "onlyVisibled", onlyvisibled));

            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetIntFromNAV(tcs, sp, cts));
            return tcs.Task;
        }
        public static Task<List<Rack>> GetRackList(string locationfilter, string zonefilter, bool onlyvisibled3, int position3, int count3, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<List<Rack>>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(null);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "GetRackList";
            XElement body =
                new XElement(myns + functionname,
                    new XElement(myns + "locationCodeFilter", locationfilter),
                    new XElement(myns + "zoneCodeFilter", zonefilter),
                    new XElement(myns + "onlyVisibled", onlyvisibled3),
                    PositionCount(myns, position3, count3));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetRackListFromNAV(tcs, sp, cts));
            return tcs.Task;
        }
        private static async Task GetRackListFromNAV(TaskCompletionSource<List<Rack>> tcs, SoapParams sp, CancellationTokenSource cts)
        {
            try
            {
                XElement soapbodynode = await Process(sp, false, cts).ConfigureAwait(false);
                string response = soapbodynode.Value;
                XDocument document = GetDoc(response);
                List<Rack> rv = new List<Rack>();
                foreach (XElement currentnode in document.Root.Elements())
                {
                    Rack rack = GetRackFromXML(currentnode);
                    rv.Add(rack);
                }
                tcs.SetResult(rv);
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
        }

        private static Rack GetRackFromXML(XElement currentnode)
        {
            Rack rack = new Rack();
            foreach (XAttribute currentatribute in currentnode.Attributes())
            {
                GetRackFromXML1(rack, currentatribute);
                GetRackFromXML2(rack, currentatribute);
                GetRackFromXML3(rack, currentatribute);
            }
            return rack;
        }
        private static void GetRackFromXML1(Rack rack, XAttribute currentatribute)
        {
            switch (currentatribute.Name.LocalName)
            {
                case "ID":
                    rack.ID = StringToInt(currentatribute.Value);
                    break;
                case "LocationCode":
                    rack.LocationCode = currentatribute.Value;
                    break;
                case "ZoneCode":
                    rack.ZoneCode = currentatribute.Value;
                    break;
                case "No":
                    rack.No = currentatribute.Value;
                    break;
                case "Sections":
                    rack.Sections = StringToInt(currentatribute.Value);
                    break;
                case "Levels":
                    rack.Levels = StringToInt(currentatribute.Value);
                    break;
                case "Depth":
                    rack.Depth = StringToInt(currentatribute.Value);
                    break;
            }
        }
        private static void GetRackFromXML2(Rack rack, XAttribute currentatribute)
        {
            switch (currentatribute.Name.LocalName)
            {
                case "Comment":
                    rack.Comment = currentatribute.Value;
                    break;
                case "Left":
                    rack.Left = StringToInt(currentatribute.Value);
                    break;
                case "Top":
                    rack.Top = StringToInt(currentatribute.Value);
                    break;
                case "SchemeVisible":
                    rack.SchemeVisible = StringToBool(currentatribute.Value);
                    break;
                case "RackOrientation":
                    int i = StringToInt(currentatribute.Value);
                    switch (i)
                    {
                        case 0:
                            rack.RackOrientation = RackOrientationEnum.HorizontalLeft;
                            break;
                        case 1:
                            rack.RackOrientation = RackOrientationEnum.HorizontalRight;
                            break;
                        case 2:
                            rack.RackOrientation = RackOrientationEnum.VerticalUp;
                            break;
                        case 3:
                            rack.RackOrientation = RackOrientationEnum.VerticalDown;
                            break;
                        default:
                            throw new InvalidOperationException("Impossible value");
                    }
                    break;
            }
        }
        private static void GetRackFromXML3(Rack rack, XAttribute currentatribute)
        {
            switch (currentatribute.Name.LocalName)
            {
                case "NumberingPrefix":
                    rack.NumberingPrefix = currentatribute.Value;
                    break;

                case "RackSectionSeparator":
                    rack.RackSectionSeparator = currentatribute.Value;
                    break;
                case "SectionLevelSeparator":
                    rack.SectionLevelSeparator = currentatribute.Value;
                    break;
                case "LevelDepthSeparator":
                    rack.LevelDepthSeparator = currentatribute.Value;
                    break;

                case "ReversSectionNumbering":
                    rack.ReversSectionNumbering = StringToBool(currentatribute.Value);
                    break;
                case "ReversLevelNumbering":
                    rack.ReversLevelNumbering = StringToBool(currentatribute.Value);
                    break;
                case "ReversDepthNumbering":
                    rack.ReversDepthNumbering = StringToBool(currentatribute.Value);
                    break;

                case "NumberingSectionBegin":
                    rack.NumberingSectionBegin = StringToInt(currentatribute.Value);
                    break;
                case "NumberingLevelBegin":
                    rack.NumberingLevelBegin = StringToInt(currentatribute.Value);
                    break;
                case "NumberingDepthBegin":
                    rack.NumberingDepthBegin = StringToInt(currentatribute.Value);
                    break;

                case "NumberingSectionDigitsQty":
                    rack.NumberingSectionDigitsQuantity = StringToInt(currentatribute.Value);
                    break;
                case "NumberingLevelDigitsQty":
                    rack.NumberingLevelDigitsQuantity = StringToInt(currentatribute.Value);
                    break;
                case "NumberingDepthDigitsQty":
                    rack.NumberingDepthDigitsQuantity = StringToInt(currentatribute.Value);
                    break;

                case "StepNumberingSection":
                    rack.StepNumberingSection = StringToInt(currentatribute.Value);
                    break;
                case "StepNumberingLevel":
                    rack.StepNumberingLevel = StringToInt(currentatribute.Value);
                    break;
                case "StepNumberingDepth":
                    rack.StepNumberingDepth = StringToInt(currentatribute.Value);
                    break;

                case "BinTemplateCode":
                    rack.BinTemplateCode = currentatribute.Value;
                    break;
            }
        }
        #endregion

        #region Bin
        public static Task<int> CreateBin(string bintemplatecode, Bin bin, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(0);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "CreateBin";
            XElement body =
                new XElement(myns + functionname,
                new XElement(myns + "binTemplateCode", bintemplatecode),
                new XElement(myns + "binCode", bin.Code),
                new XElement(myns + "rackID", bin.RackID),
                new XElement(myns + "section", bin.Section),
                new XElement(myns + "level", bin.Level),
                new XElement(myns + "depth", bin.Depth),
                new XElement(myns + "sectionSpan", bin.SectionSpan),
                new XElement(myns + "levelSpan", bin.LevelSpan),
                new XElement(myns + "depthSpan", bin.DepthSpan));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetIntFromNAV(tcs, sp, cts));
            return tcs.Task;
        }
        public static Task<int> ModifyBin(Bin bin, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(0);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "ModifyBin";
            XElement body =
                new XElement(myns + functionname,
                    new XElement(myns + "locationCode", bin.LocationCode),
                    new XElement(myns + "binCode", bin.Code),
                    new XElement(myns + "prevBinCode", bin.PrevCode),
                    new XElement(myns + "rackID", bin.RackID),
                    new XElement(myns + "section", bin.Section),
                    new XElement(myns + "level", bin.Level),
                    new XElement(myns + "depth", bin.Depth),
                    new XElement(myns + "sectionSpan", bin.SectionSpan),
                    new XElement(myns + "levelSpan", bin.LevelSpan),
                    new XElement(myns + "depthSpan", bin.DepthSpan),
                    new XElement(myns + "binRanking", bin.BinRanking),
                    new XElement(myns + "maximumCubage", bin.MaximumCubage),
                    new XElement(myns + "maximumWeight", bin.MaximumWeight),
                    new XElement(myns + "blockMovement", bin.BlockMovement),
                    new XElement(myns + "binTypeCode", bin.BinType),
                    new XElement(myns + "warehouseClassCode", bin.WarehouseClassCode),
                    new XElement(myns + "specialEquipmentCode", bin.SpecialEquipmentCode));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetIntFromNAV(tcs, sp, cts));

            return tcs.Task;
        }
        public static Task<int> DeleteBin(string locationcode, string bincode, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(0);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "DeleteBin";
            XElement body =
                new XElement(myns + functionname,
                new XElement(myns + "locationCodeFilter", locationcode),
                new XElement(myns + "code", bincode));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetIntFromNAV(tcs, sp, cts));
            return tcs.Task;
        }
        public static Task<int> GetBinCount(string locationfilter, string codefilter, string rackidfilter, string bincodefilter, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(0);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "GetBinCount";
            XElement body =
                new XElement(myns + functionname,
                new XElement(myns + "locationCodeFilter", locationfilter),
                new XElement(myns + "zoneCodeFilter", codefilter),
                new XElement(myns + "rackIDFilter", rackidfilter),
                new XElement(myns + "binCodeFilter", bincodefilter));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetIntFromNAV(tcs, sp, cts));
            return tcs.Task;
        }
        public static Task<List<Bin>> GetBinList(NAVFilter Filter, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<List<Bin>>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(null);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "GetBinList";
            XElement body =
                new XElement(myns + functionname,
                    new XElement(myns + "locationCodeFilter", Filter.LocationCodeFilter),
                    new XElement(myns + "zoneCodeFilter", Filter.ZoneCodeFilter),
                    new XElement(myns + "rackIDFilter", Filter.RackIDFilter),
                    new XElement(myns + "binCodeFilter", Filter.BinCodeFilter),
                    PositionCount(myns, Filter.ItemsPosition, Filter.ItemsCount));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetBinListFromNAV(tcs, sp, cts));
            return tcs.Task;
        }
        private static async Task GetBinListFromNAV(TaskCompletionSource<List<Bin>> tcs, SoapParams sp, CancellationTokenSource cts)
        {
            try
            {
                XElement soapbodynode = await Process(sp, false, cts).ConfigureAwait(false);
                string response = soapbodynode.Value;
                XDocument document = GetDoc(response);
                List<Bin> rv = new List<Bin>();
                foreach (XElement currentnode in document.Root.Elements())
                {
                    Bin bin = GetBinFromXML(currentnode);
                    rv.Add(bin);
                }
                tcs.SetResult(rv);
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
        }

        private static Bin GetBinFromXML(XElement currentnode)
        {
            Bin bin = new Bin();
            foreach (XAttribute currentatribute in currentnode.Attributes())
            {
                GetBinFromXML1(bin, currentatribute);
                GetBinFromXML2(bin, currentatribute);
                GetBinFromXML3(bin, currentatribute);
            }
            return bin;
        }
        private static void GetBinFromXML1(Bin bin, XAttribute currentatribute)
        {
            switch (currentatribute.Name.LocalName)
            {
                case "LocationCode":
                    bin.LocationCode = currentatribute.Value;
                    break;
                case "ZoneCode":
                    bin.ZoneCode = currentatribute.Value;
                    break;
                case "Code":
                    bin.Code = currentatribute.Value;
                    break;
                case "Description":
                    bin.Description = currentatribute.Value;
                    break;
                case "BinType":
                    bin.BinType = currentatribute.Value;
                    break;
                case "Empty":
                    bin.Empty = StringToBool(currentatribute.Value);
                    break;
                case "BlockMovement":
                    bin.BlockMovement = StringToInt(currentatribute.Value);
                    break;
                case "RackID":
                    bin.RackID = StringToInt(currentatribute.Value);
                    break;
            }
        }
        private static void GetBinFromXML2(Bin bin, XAttribute currentatribute)
        {
            switch (currentatribute.Name.LocalName)
            {
                case "Section":
                    bin.Section = StringToInt(currentatribute.Value);
                    break;
                case "Level":
                    bin.Level = StringToInt(currentatribute.Value);
                    break;
                case "Depth":
                    bin.Depth = StringToInt(currentatribute.Value);
                    break;
                case "LevelSpan":
                    bin.LevelSpan = StringToInt(currentatribute.Value);
                    break;
                case "SectionSpan":
                    bin.SectionSpan = StringToInt(currentatribute.Value);
                    break;
                case "DepthSpan":
                    bin.DepthSpan = StringToInt(currentatribute.Value);
                    break;
            }
        }
        private static void GetBinFromXML3(Bin bin, XAttribute currentatribute)
        {
            switch (currentatribute.Name.LocalName)
            {
                case "BinRanking":
                    bin.BinRanking = StringToInt(currentatribute.Value);
                    break;
                case "MaximumCubage":
                    bin.MaximumCubage = StringToDec(currentatribute.Value);
                    break;
                case "MaximumWeight":
                    bin.MaximumWeight = StringToDec(currentatribute.Value);
                    break;
                case "AdjustmentBin":
                    bin.AdjustmentBin = StringToBool(currentatribute.Value);
                    break;
                case "SpecialEquipmentCode":
                    bin.SpecialEquipmentCode = currentatribute.Value;
                    break;
                case "WarehouseClassCode":
                    bin.WarehouseClassCode = currentatribute.Value;
                    break;
            }
        }

        public static Task<List<BinValues>> GetBinValuesByRackID(int rackid, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<List<BinValues>>();

            if (IsConnectionFaild())
            {
                tcs.SetResult(null);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "GetBinValuesByRackID";
            XElement body =
                new XElement(myns + functionname,
                    new XElement(myns + "iD", rackid),
                    new XElement(myns + "responseDocument", ""));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetBinValuesByRackIDNAV(tcs, sp, cts));
            return tcs.Task;
        }
        private static async Task GetBinValuesByRackIDNAV(TaskCompletionSource<List<BinValues>> tcs, SoapParams sp, CancellationTokenSource cts)
        {
            try
            {
                XElement soapbodynode = await Process(sp, false, cts).ConfigureAwait(false);
                string response = soapbodynode.Value;
                XDocument document = GetDoc(response);
                List<BinValues> rv = new List<BinValues>();
                foreach (XElement currentnode in document.Root.Elements())
                {
                    BinValues bv = GetBinValuesFromXML(currentnode);
                    bv.Code = currentnode.Value;
                    rv.Add(bv);
                }
                tcs.SetResult(rv);
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
        }
        private static BinValues GetBinValuesFromXML(XElement currentnode)
        {
            BinValues bv = new BinValues();
            foreach (XAttribute currentatribute in currentnode.Attributes())
            {
                switch (currentatribute.Name.LocalName)
                {
                    case "Right":
                        {
                            bv.RightValue = currentatribute.Value;
                            break;
                        }
                    case "Left":
                        {
                            bv.LeftValue = currentatribute.Value;
                            break;
                        }
                }
            }
            return bv;
        }

        public static Task<List<BinInfo>> GetBinInfo(string locationcode, string bincode, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<List<BinInfo>>();

            if (IsConnectionFaild())
            {
                tcs.SetResult(null);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "GetBinInfo";
            XElement body =
                new XElement(myns + functionname,
                    new XElement(myns + "locationCode", locationcode),
                    new XElement(myns + "binCode", bincode),
                    new XElement(myns + "responseDocument", ""));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetBinInfoNAV(tcs, sp, cts));
            return tcs.Task;
        }
        private static async Task GetBinInfoNAV(TaskCompletionSource<List<BinInfo>> tcs, SoapParams sp, CancellationTokenSource cts)
        {
            try
            {
                XElement soapbodynode = await Process(sp, false, cts).ConfigureAwait(false);
                string response = soapbodynode.Value;
                XDocument document = GetDoc(response);
                List<BinInfo> rv = new List<BinInfo>();
                foreach (XElement currentnode in document.Root.Elements())
                {
                    BinInfo bi = GetBinInfoFromXML(currentnode);
                    rv.Add(bi);
                }
                tcs.SetResult(rv);
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
        }
        private static BinInfo GetBinInfoFromXML(XElement currentnode)
        {
            BinInfo bi = new BinInfo();
            foreach (XAttribute currentatribute in currentnode.Attributes())
            {
                switch (currentatribute.Name.LocalName)
                {
                    case "Caption":
                        {
                            bi.Caption = currentatribute.Value;
                            break;
                        }
                    case "Description":
                        {
                            bi.Description = currentatribute.Value;
                            break;
                        }
                    case "Detail":
                        {
                            bi.Detail = currentatribute.Value;
                            break;
                        }
                    case "Value1":
                        {
                            bi.Value1 = currentatribute.Value;
                            break;
                        }
                    case "Value2":
                        {
                            bi.Value2 = currentatribute.Value;
                            break;
                        }
                    case "Value3":
                        {
                            bi.Value3 = currentatribute.Value;
                            break;
                        }
                    case "ImageURL":
                        {
                            bi.ImageURL = currentatribute.Value;
                            break;
                        }
                }
            }
            return bi;
        }

        public static Task<List<BinImage>> GetBinImagesByRackID(int rackid, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<List<BinImage>>();

            if (IsConnectionFaild())
            {
                tcs.SetResult(null);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "GetBinImagesByRackID";
            XElement body =
                new XElement(myns + functionname,
                    new XElement(myns + "iD", rackid),
                    new XElement(myns + "responseDocument", ""));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetBinImagesByRackIDNAV(tcs, sp, cts));
            return tcs.Task;
        }
        private static async Task GetBinImagesByRackIDNAV(TaskCompletionSource<List<BinImage>> tcs, SoapParams sp, CancellationTokenSource cts)
        {
            try
            {
                XElement soapbodynode = await Process(sp, false, cts).ConfigureAwait(false);
                string response = soapbodynode.Value;
                XDocument document = GetDoc(response);
                List<BinImage> rv = new List<BinImage>();
                foreach (XElement currentnode in document.Root.Elements())
                {
                    BinImage bi = GetBinImageFromXML(currentnode);
                    bi.BinCode = currentnode.Value;
                    rv.Add(bi);
                }
                tcs.SetResult(rv);
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
        }
        private static BinImage GetBinImageFromXML(XElement currentnode)
        {
            BinImage bv = new BinImage();
            foreach (XAttribute currentatribute in currentnode.Attributes())
            {
                switch (currentatribute.Name.LocalName)
                {
                    case "Image":
                        {
                            bv.ImageURL = currentatribute.Value;
                            break;
                        }
                }
            }
            return bv;
        }
        #endregion
        #region BinTemplate
        public static XElement[] SetBinTemplateParams(XNamespace myns, BinTemplate bintemplate)
        {
            return new XElement[]
            {
                new XElement(myns + "zoneCode", bintemplate.ZoneCode),
                new XElement(myns + "code", bintemplate.Code),
                new XElement(myns + "description", bintemplate.Description),
                new XElement(myns + "binTypeCode", bintemplate.BinTypeCode),
                new XElement(myns + "warehouseClassCode", bintemplate.WarehouseClassCode),
                new XElement(myns + "blockMovement", bintemplate.BlockMovement),
                new XElement(myns + "specialEquipmentCode", bintemplate.SpecialEquipmentCode),
                new XElement(myns + "binRanking", bintemplate.BinRanking),
                new XElement(myns + "maximumCubage", bintemplate.MaximumCubage),
                new XElement(myns + "maximumWeight", bintemplate.MaximumWeight),
                new XElement(myns + "dedicated1", bintemplate.Dedicated)
            };
        }
        public static Task<int> CreateBinTemplate(BinTemplate bintemplate, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(0);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "CreateBinTemplate";
            XElement body =
                new XElement(myns + functionname,
                    new XElement(myns + "locationCode", bintemplate.LocationCode),
                    SetBinTemplateParams(myns, bintemplate));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetIntFromNAV(tcs, sp, cts));
            return tcs.Task;
        }
        public static Task<int> ModifyBinTemplate(BinTemplate bintemplate, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(0);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "ModifyBinTemplate";

            XElement body =
                new XElement(myns + functionname,
                new XElement(myns + "locationCode", bintemplate.LocationCode),
                SetBinTemplateParams(myns, bintemplate));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetIntFromNAV(tcs, sp, cts));
            return tcs.Task;
        }
        public static Task<int> DeleteBinTemplate(BinTemplate bintemplate, CancellationTokenSource cts)
        {
            //<element minOccurs="1" maxOccurs="1" name="code" type="string"/>
            var tcs = new TaskCompletionSource<int>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(0);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "DeleteBinTemplate";
            XElement body =
                new XElement(myns + functionname,
                new XElement(myns + "code", bintemplate.Code));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetIntFromNAV(tcs, sp, cts));
            return tcs.Task;
        }
        public static Task<int> GetBinTemplateCount(CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(0);
                return tcs.Task;
            }
            XNamespace myns = GetNameSpace();
            string functionname = "GetBinTemplateCount";
                XElement body = new XElement(myns + functionname);
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetIntFromNAV(tcs, sp, cts));
            return tcs.Task;
        }
        public static Task<List<BinTemplate>> GetBinTemplateList(int position, int count, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<List<BinTemplate>>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(null);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "GetBinTemplateList";
            XElement body =
                new XElement(myns + functionname,
                    PositionCount(myns, position, count));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetBinTemplateListFromNAV(tcs, sp, cts));
            return tcs.Task;
        }
        private static async Task GetBinTemplateListFromNAV(TaskCompletionSource<List<BinTemplate>> tcs, SoapParams sp, CancellationTokenSource cts)
        {
            try
            {
                XElement soapbodynode = await Process(sp, false, cts).ConfigureAwait(false);
                string response = soapbodynode.Value;
                XDocument document = GetDoc(response);
                List<BinTemplate> rv = new List<BinTemplate>();
                foreach (XElement currentnode in document.Root.Elements())
                {
                    BinTemplate bintemplate = GetBinTemplateFromXML(currentnode);
                    rv.Add(bintemplate);
                }
                tcs.SetResult(rv);
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
        }
        private static BinTemplate GetBinTemplateFromXML(XElement currentnode)
        {
            BinTemplate bintemplate = new BinTemplate();
            foreach (XAttribute currentatribute in currentnode.Attributes())
            {
                GetBinTemplateFromXML1(bintemplate, currentatribute);
                GetBinTemplateFromXML2(bintemplate, currentatribute);
            }
            return bintemplate;
        }
        private static void GetBinTemplateFromXML1(BinTemplate bintemplate, XAttribute currentatribute)
        {
            switch (currentatribute.Name.LocalName)
            {
                case "Code":
                    bintemplate.Code = currentatribute.Value;
                    break;
                case "Description":
                    bintemplate.Description = currentatribute.Value;
                    break;
                case "LocationCode":
                    bintemplate.LocationCode = currentatribute.Value;
                    break;
                case "ZoneCode":
                    bintemplate.ZoneCode = currentatribute.Value;
                    break;
                case "BinTypeCode":
                    bintemplate.BinTypeCode = currentatribute.Value;
                    break;
                case "BlockMovement":
                    bintemplate.BlockMovement = StringToInt(currentatribute.Value);
                    break;
            }
        }
        private static void GetBinTemplateFromXML2(BinTemplate bintemplate, XAttribute currentatribute)
        {
            switch (currentatribute.Name.LocalName)
            {
                case "BinDescription":
                    bintemplate.BinDescription = currentatribute.Value;
                    break;
                case "MaximumCubage":
                    bintemplate.MaximumCubage = StringToDec(currentatribute.Value);
                    break;
                case "MaximumWeight":
                    bintemplate.MaximumWeight = StringToDec(currentatribute.Value);
                    break;
                case "SpecialEquipmentCode":
                    bintemplate.SpecialEquipmentCode = currentatribute.Value;
                    break;
                case "WarehouseClassCode":
                    bintemplate.WarehouseClassCode = currentatribute.Value;
                    break;
                case "BinRanking":
                    bintemplate.BinRanking = StringToInt(currentatribute.Value);
                    break;
                case "Dedicated":
                    bintemplate.Dedicated = StringToBool(currentatribute.Value);
                    break;
            }
        }
        #endregion
        #region BinType
        public static Task<int> GetBinTypeCount(CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(0);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "GetBinTypeCount";
            XElement body = new XElement(myns + functionname);
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetIntFromNAV(tcs, sp, cts));
            return tcs.Task;
        }
        public static Task<List<BinType>> GetBinTypeList(int position, int count, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<List<BinType>>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(null);
                return tcs.Task;
            }
            XNamespace myns = GetNameSpace();
            string functionname = "GetBinTypeList";
            XElement body =
                new XElement(myns + functionname,
                    PositionCount(myns, position, count));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetBinTypeListFromNAV(tcs, sp, cts));
            return tcs.Task;
        }
        private static async Task GetBinTypeListFromNAV(TaskCompletionSource<List<BinType>> tcs, SoapParams sp, CancellationTokenSource cts)
        {
            try
            {
                XElement soapbodynode = await Process(sp, false, cts).ConfigureAwait(false);
                string response = soapbodynode.Value;
                XDocument document = GetDoc(response);
                List<BinType> rv = new List<BinType>();
                foreach (XElement currentnode in document.Root.Elements())
                {
                    BinType bintype = GetBinTypeFromXML(currentnode);
                    rv.Add(bintype);
                }
                tcs.SetResult(rv);
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
        }
        private static BinType GetBinTypeFromXML(XElement currentnode)
        {
            BinType bintype = new BinType();
            foreach (XAttribute currentatribute in currentnode.Attributes())
            {
                switch (currentatribute.Name.LocalName)
                {
                    case "Code":
                        bintype.Code = currentatribute.Value;
                        break;
                    case "Description":
                        bintype.Description = currentatribute.Value;
                        break;
                    case "Pick":
                        bintype.Pick = StringToBool(currentatribute.Value);
                        break;
                    case "PutAway":
                        bintype.PutAway = StringToBool(currentatribute.Value);
                        break;
                    case "Receive":
                        bintype.Receive = StringToBool(currentatribute.Value);
                        break;
                    case "Ship":
                        bintype.Ship = StringToBool(currentatribute.Value);
                        break;
                }
            }
            return bintype;
        }
        #endregion
        #region BinContent
        public static XElement[] BinContentFilters(XNamespace myns, NAVFilter Filter)
        {
            return new XElement[]
            {
                new XElement(myns + "locationCodeFilter", Filter.LocationCodeFilter),
                new XElement(myns + "zoneCodeFilter", Filter.ZoneCodeFilter),
                new XElement(myns + "binCodeFilter", Filter.BinCodeFilter),
                new XElement(myns + "itemNoFilter", Filter.ItemNoFilter),
                new XElement(myns + "variantCodeFilter", Filter.VariantCodeFilter)
            };
        }

        public static Task<int> GetBinContentCount(NAVFilter Filter, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(0);
                return tcs.Task;
            }
            XNamespace myns = GetNameSpace();
            string functionname = "GetBinContentCount";
            XElement body =
                new XElement(myns + functionname,
                    BinContentFilters(myns, Filter));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetIntFromNAV(tcs, sp, cts));
            return tcs.Task;
        }
        public static Task<List<BinContent>> GetBinContentList(NAVFilter Filter, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<List<BinContent>>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(null);
                return tcs.Task;
            }
            XNamespace myns = GetNameSpace();
            string functionname = "GetBinContentList";
            XElement body =
                new XElement(myns + functionname,
                    BinContentFilters(myns,Filter),
                    PositionCount(myns, Filter.ItemsPosition, Filter.ItemsCount));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetBinContentFromNAV(tcs, sp, cts));
            return tcs.Task;
        }
        private static async Task GetBinContentFromNAV(TaskCompletionSource<List<BinContent>> tcs, SoapParams sp, CancellationTokenSource cts)
        {
            try
            {
                XElement soapbodynode = await Process(sp, false, cts).ConfigureAwait(false);
                string response = soapbodynode.Value;
                XDocument document = GetDoc(response);
                List<BinContent> rv = new List<BinContent>();
                foreach (XElement currentnode in document.Root.Elements())
                {
                    BinContent bincontent = GetBinContentFromXML(currentnode);
                    rv.Add(bincontent);
                }
                tcs.SetResult(rv);
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
        }
        private static BinContent GetBinContentFromXML(XElement currentnode)
        {
            BinContent bincontent = new BinContent();
            foreach (XAttribute currentatribute in currentnode.Attributes())
            {
                GetBinContentFromXML1(bincontent, currentatribute);
                GetBinContentFromXML2(bincontent, currentatribute);
            }
            return bincontent;
        }
        private static void GetBinContentFromXML1(BinContent bincontent, XAttribute currentatribute)
        {
            switch (currentatribute.Name.LocalName)
            {
                case "LocationCode":
                    bincontent.LocationCode = currentatribute.Value;
                    break;
                case "ZoneCode":
                    bincontent.ZoneCode = currentatribute.Value;
                    break;
                case "BinCode":
                    bincontent.BinCode = currentatribute.Value;
                    break;
                case "BinType":
                    bincontent.BinType = currentatribute.Value;
                    break;
                case "BlockMovement":
                    bincontent.BlockMovement = StringToInt(currentatribute.Value);
                    break;
                case "ItemNo":
                    bincontent.ItemNo = currentatribute.Value;
                    break;
                case "VariantCode":
                    bincontent.VariantCode = currentatribute.Value;
                    break;
            }
        }
        private static void GetBinContentFromXML2(BinContent bincontent, XAttribute currentatribute)
        {
            switch (currentatribute.Name.LocalName)
            {
                case "Description":
                    bincontent.Description = currentatribute.Value;
                    break;
                case "NegAdjmtQty":
                    bincontent.NegAdjmtQty = StringToDec(currentatribute.Value);
                    break;
                case "PickQty":
                    bincontent.PickQty = StringToDec(currentatribute.Value);
                    break;
                case "PosAdjmtQty":
                    bincontent.PosAdjmtQty = StringToDec(currentatribute.Value);
                    break;
                case "PutawayQty":
                    bincontent.PutawayQty = StringToDec(currentatribute.Value);
                    break;
                case "Quantity":
                    bincontent.Quantity = StringToDec(currentatribute.Value);
                    break;
                case "QuantityBase":
                    bincontent.QuantityBase = StringToDec(currentatribute.Value);
                    break;
                case "UnitofMeasureCode":
                    bincontent.UnitofMeasureCode = currentatribute.Value;
                    break;
                case "ImageURL":
                    bincontent.ImageURL = currentatribute.Value;
                    break;
            }
        }
        #endregion

        #region ItemIdentifiers
        public static Task<int> GetItemIdentifierCount(string barCodeCodeFilter, string itemNoFilter, string variantCodeFilter, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(0);
                return tcs.Task;
            }
            XNamespace myns = GetNameSpace();
            string functionname = "GetItemIdentifierCount";
            XElement body =
                new XElement(myns + functionname,
                    new XElement(myns + "barCodeFilter", barCodeCodeFilter),
                    new XElement(myns + "itemNoFilter", itemNoFilter),
                    new XElement(myns + "variantCodeFilter", variantCodeFilter));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetIntFromNAV(tcs, sp, cts));
            return tcs.Task;
        }
        public static Task<List<ItemIdentifier>> GetItemIdentifierList(string barCodeCodeFilter, string itemNoFilter, string variantCodeFilter, int position, int count, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<List<ItemIdentifier>>();

            if (IsConnectionFaild())
            {
                tcs.SetResult(null);
                return tcs.Task;
            }
            XNamespace myns = GetNameSpace();
            string functionname = "GetItemIdentifierList";
            XElement body =
                new XElement(myns + functionname,
                    new XElement(myns + "barCodeFilter", barCodeCodeFilter),
                    new XElement(myns + "itemNoFilter", itemNoFilter),
                    new XElement(myns + "variantCodeFilter", variantCodeFilter),
                    PositionCount(myns, position, count));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetItemIdentifierListFromNAV(tcs, sp, cts));

            return tcs.Task;
        }
        private static async Task GetItemIdentifierListFromNAV(TaskCompletionSource<List<ItemIdentifier>> tcs, SoapParams sp, CancellationTokenSource cts)
        {
            try
            {
                XElement soapbodynode = await Process(sp, false, cts).ConfigureAwait(false);
                string response = soapbodynode.Value;
                XDocument document = GetDoc(response);
                List<ItemIdentifier> rv = new List<ItemIdentifier>();
                foreach (XElement currentnode in document.Root.Elements())
                {
                    ItemIdentifier itemidentifier = GetItemIdentifierFromXML(currentnode);
                    rv.Add(itemidentifier);
                }
                tcs.SetResult(rv);
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
        }
        private static ItemIdentifier GetItemIdentifierFromXML(XElement currentnode)
        {
            ItemIdentifier itemidentifier = new ItemIdentifier();
            foreach (XAttribute currentatribute in currentnode.Attributes())
            {
                switch (currentatribute.Name.LocalName)
                {
                    case "BarCode":
                        itemidentifier.BarCode = currentatribute.Value;
                        break;
                    case "ItemNo":
                        itemidentifier.ItemNo = currentatribute.Value;
                        break;
                    case "VariantCode":
                        itemidentifier.VariantCode = currentatribute.Value;
                        break;
                    case "UoM":
                        itemidentifier.UoM = currentatribute.Value;
                        break;
                }
            }
            return itemidentifier;
        }

        #endregion
        #region WarehouseClass
        public static Task<int> GetWarehouseClassCount(CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(0);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "GetWarehouseClassCount";
            XElement body = new XElement(myns + functionname);
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetIntFromNAV(tcs, sp, cts));
            return tcs.Task;
        }
        public static Task<List<WarehouseClass>> GetWarehouseClassList(int position, int count, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<List<WarehouseClass>>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(null);
                return tcs.Task;
            }
            XNamespace myns = GetNameSpace();
            string functionname = "GetWarehouseClassList";
            XElement body =
                new XElement(myns + functionname,
                    PositionCount(myns, position, count));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetWarehouseClassListFromNAV(tcs, sp, cts));

            return tcs.Task;
        }
        private static async Task GetWarehouseClassListFromNAV(TaskCompletionSource<List<WarehouseClass>> tcs, SoapParams sp, CancellationTokenSource cts)
        {
            try
            {
                XElement soapbodynode = await Process(sp, false, cts).ConfigureAwait(false);
                string response = soapbodynode.Value;
                XDocument document = GetDoc(response);
                List<WarehouseClass> rv = new List<WarehouseClass>();
                foreach (XElement currentnode in document.Root.Elements())
                {
                    WarehouseClass warehouseclass = GetWarehouseClassFromXML(currentnode);
                    rv.Add(warehouseclass);
                }
                tcs.SetResult(rv);
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
        }
        private static WarehouseClass GetWarehouseClassFromXML(XElement currentnode)
        {
            WarehouseClass warehouseclass = new WarehouseClass();
            foreach (XAttribute currentatribute in currentnode.Attributes())
            {
                switch (currentatribute.Name.LocalName)
                {
                    case "Code":
                        warehouseclass.Code = currentatribute.Value;
                        break;
                    case "Description":
                        warehouseclass.Description = currentatribute.Value;
                        break;
                }
            }
            return warehouseclass;
        }
        #endregion

        #region SpecialEquipment
        public static Task<int> GetSpecialEquipmentCount(CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(0);
                return tcs.Task;
            }
            XNamespace myns = GetNameSpace();
            string functionname = "GetSpecialEquipmentCount";
            XElement body = new XElement(myns + functionname);
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetIntFromNAV(tcs, sp, cts));
            return tcs.Task;
        }
        public static Task<List<SpecialEquipment>> GetSpecialEquipmentList(int position, int count, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<List<SpecialEquipment>>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(null);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "GetSpecialEquipmentList";
            XElement body =
                new XElement(myns + functionname,
                    PositionCount(myns, position, count));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetSpecialEquipmentListFromNAV(tcs, sp, cts));

            return tcs.Task;
        }
        private static async Task GetSpecialEquipmentListFromNAV(TaskCompletionSource<List<SpecialEquipment>> tcs, SoapParams sp, CancellationTokenSource cts)
        {
            try
            {
                XElement soapbodynode = await Process(sp, false, cts).ConfigureAwait(false);
                string response = soapbodynode.Value;
                XDocument document = GetDoc(response);
                List<SpecialEquipment> rv = new List<SpecialEquipment>();
                foreach (XElement currentnode in document.Root.Elements())
                {
                    SpecialEquipment specialequipment = GetSpecialEquipmentFromXML(currentnode);
                    rv.Add(specialequipment);
                }
                tcs.SetResult(rv);
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
        }
        private static SpecialEquipment GetSpecialEquipmentFromXML(XElement currentnode)
        {
            SpecialEquipment specialequipment = new SpecialEquipment();
            foreach (XAttribute currentatribute in currentnode.Attributes())
            {
                switch (currentatribute.Name.LocalName)
                {
                    case "Code":
                        specialequipment.Code = currentatribute.Value;
                        break;
                    case "Description":
                        specialequipment.Description = currentatribute.Value;
                        break;
                }
            }
            return specialequipment;
        }
        #endregion

        #region WarehouseEntry
        public static Task<int> GetWarehouseEntryCount(NAVFilter Filter, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(0);
                return tcs.Task;
            }
            XNamespace myns = GetNameSpace();
            string functionname = "GetWarehouseEntryCount";
            XElement body =
                new XElement(myns + functionname,
                    new XElement(myns + "locationCodeFilter", Filter.LocationCodeFilter),
                    new XElement(myns + "zoneCodeFilter", Filter.ZoneCodeFilter),
                    new XElement(myns + "binCodeFilter", Filter.BinCodeFilter),
                    new XElement(myns + "itemNoFilter", Filter.ItemNoFilter),
                    new XElement(myns + "variantCodeFilter", Filter.VariantCodeFilter));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetIntFromNAV(tcs, sp, cts));
            return tcs.Task;
        }
        public static Task<List<WarehouseEntry>> GetWarehouseEntryList(NAVFilter Filter, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<List<WarehouseEntry>>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(null);
                return tcs.Task;
            }
            XNamespace myns = GetNameSpace();
            string functionname = "GetWarehouseEntryList";

            XElement body =
                new XElement(myns + functionname,
                    BinContentFilters(myns,Filter),
                    PositionCount(myns, Filter.ItemsPosition, Filter.ItemsCount));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => GetWarehouseEntryListFromNAV(tcs, sp, cts));
            return tcs.Task;
        }
        private static async Task GetWarehouseEntryListFromNAV(TaskCompletionSource<List<WarehouseEntry>> tcs, SoapParams sp, CancellationTokenSource cts)
        {
            try
            {
                XElement soapbodynode = await Process(sp, false, cts).ConfigureAwait(false);
                string response = soapbodynode.Value;
                XDocument document = GetDoc(response);
                List<WarehouseEntry> rv = new List<WarehouseEntry>();
                foreach (XElement currentnode in document.Root.Elements())
                {
                    WarehouseEntry we = GetWarehouseEntryFromXML(currentnode);
                    rv.Add(we);
                }
                tcs.SetResult(rv);
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
        }
        private static WarehouseEntry GetWarehouseEntryFromXML(XElement currentnode)
        {
            WarehouseEntry we = new WarehouseEntry();
            foreach (XAttribute currentatribute in currentnode.Attributes())
            {
                GetWarehouseEntryFromXML1(we, currentatribute);
                GetWarehouseEntryFromXML2(we, currentatribute);
            }
            return we;
        }
        private static void GetWarehouseEntryFromXML1(WarehouseEntry we, XAttribute currentatribute)
        {
            switch (currentatribute.Name.LocalName)
            {
                case "LocationCode":
                    we.LocationCode = currentatribute.Value;
                    break;
                case "ZoneCode":
                    we.ZoneCode = currentatribute.Value;
                    break;
                case "BinCode":
                    we.BinCode = currentatribute.Value;
                    break;
                case "ItemNo":
                    we.ItemNo = currentatribute.Value;
                    break;
                case "VariantCode":
                    we.VariantCode = currentatribute.Value;
                    break;
                case "Description":
                    we.Description = currentatribute.Value;
                    break;
            }
        }
        private static void GetWarehouseEntryFromXML2(WarehouseEntry we, XAttribute currentatribute)
        {
            switch (currentatribute.Name.LocalName)
            {
                case "EntryType":
                    we.EntryType = StringToInt(currentatribute.Value);
                    break;
                case "RegisteringDate":
                    we.RegisteringDate = currentatribute.Value;
                    break;
                case "SourceNo":
                    we.SourceNo = currentatribute.Value;
                    break;
                case "Quantity":
                    we.Quantity = StringToDec(currentatribute.Value);
                    break;
                case "QuantityBase":
                    we.QuantityBase = StringToDec(currentatribute.Value);
                    break;
                case "UnitofMeasureCode":
                    we.UnitofMeasureCode = currentatribute.Value;
                    break;
            }
        }
        #endregion
        #region UserDefinedSelection
        public static Task<List<UserDefinedSelection>> LoadUDS(string locationCode, string zoneCode, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<List<UserDefinedSelection>>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(null);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "LoadUserDefinedSelectionList";
            XElement body =
                new XElement(myns + functionname,
                    new XElement(myns + "locationCode", locationCode),
                    new XElement(myns + "zoneCode", zoneCode),
                    new XElement(myns + "responseDocument", ""));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => LoadUDSNAV(tcs, sp, cts));
            return tcs.Task;
        }
        private static async Task LoadUDSNAV(TaskCompletionSource<List<UserDefinedSelection>> tcs, SoapParams sp, CancellationTokenSource cts)
        {
            try
            {
                XElement soapbodynode = await Process(sp, false, cts).ConfigureAwait(false);
                string response = soapbodynode.Value;
                XDocument document = GetDoc(response);
                List<UserDefinedSelection> rv = new List<UserDefinedSelection>();
                foreach (XElement currentnode in document.Root.Elements())
                {
                    UserDefinedSelection uds = GetUserDefinedSelectionFromXML(currentnode);
                    rv.Add(uds);
                }
                tcs.SetResult(rv);
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
        }
        private static UserDefinedSelection GetUserDefinedSelectionFromXML(XElement currentnode)
        {
            UserDefinedSelection uds = new UserDefinedSelection();
            foreach (XAttribute currentatribute in currentnode.Attributes())
            {
                switch (currentatribute.Name.LocalName)
                {
                    case "ID":
                        {
                            uds.ID = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "Name":
                        {
                            uds.Name = currentatribute.Value;
                            break;
                        }
                    case "Detail":
                        {
                            uds.Detail = currentatribute.Value;
                            break;
                        }
                    case "HexColor":
                        {
                            uds.HexColor = currentatribute.Value;
                            break;
                        }
                    case "Value":
                        {
                            uds.Value = StringToInt(currentatribute.Value);
                            break;
                        }
                }
            }
            return uds;
        }
        public static Task<List<UserDefinedSelectionResult>> RunUDS(string locationCode, string zoneCode, int i, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<List<UserDefinedSelectionResult>>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(null);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "RunUDS";
            XElement body =
                new XElement(myns + functionname,
                    new XElement(myns + "locationCode", locationCode),
                    new XElement(myns + "zoneCode", zoneCode),
                    new XElement(myns + "functionIndex", i),
                    new XElement(myns + "responseDocument", ""));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => RunUDSNAV(tcs, sp, cts));
            return tcs.Task;
        }
        private static async Task RunUDSNAV(TaskCompletionSource<List<UserDefinedSelectionResult>> tcs, SoapParams sp, CancellationTokenSource cts)
        {
            try
            {
                XElement soapbodynode = await Process(sp, false, cts).ConfigureAwait(false);
                string response = soapbodynode.Value;
                XDocument document = GetDoc(response);
                List<UserDefinedSelectionResult> rv = new List<UserDefinedSelectionResult>();
                foreach (XElement currentnode in document.Root.Elements())
                {
                    UserDefinedSelectionResult uds = GetUserDefinedSelectionResultnFromXML(currentnode);
                    rv.Add(uds);
                }
                tcs.SetResult(rv);
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
        }
        private static UserDefinedSelectionResult GetUserDefinedSelectionResultnFromXML(XElement currentnode)
        {
            UserDefinedSelectionResult uds = new UserDefinedSelectionResult();
            foreach (XAttribute currentatribute in currentnode.Attributes())
            {
                switch (currentatribute.Name.LocalName)
                {
                    case "F":
                        {
                            uds.FunctionID = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "R":
                        {
                            uds.RackID = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "S":
                        {
                            uds.Section = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "L":
                        {
                            uds.Level = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "D":
                        {
                            uds.Depth = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "Q":
                        {
                            uds.Value = (int)Math.Round(StringToDec(currentatribute.Value));
                            break;
                        }
                    case "C":
                        {
                            uds.HexColor = currentatribute.Value;
                            break;
                        }
                }
            }
            return uds;
        }
        #endregion
        #region UserDefinedFunctions
        public static Task<List<UserDefinedFunction>> LoadUserDefinedFunctionList(string locationCode, string zoneCode, int rackid, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<List<UserDefinedFunction>>();

            if (IsConnectionFaild())
            {
                tcs.SetResult(null);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "LoadUserDefinedFunctionList";
            XElement body =
                new XElement(myns + functionname,
                    new XElement(myns + "locationCode", locationCode),
                    new XElement(myns + "zoneCode", zoneCode),
                    new XElement(myns + "rackID", rackid),
                    new XElement(myns + "responseDocument", ""));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => LoadUserDefinedFunctionListNAV(tcs, sp, cts));
            return tcs.Task;
        }
        private static async Task LoadUserDefinedFunctionListNAV(TaskCompletionSource<List<UserDefinedFunction>> tcs, SoapParams sp, CancellationTokenSource cts)
        {
            try
            {
                XElement soapbodynode = await Process(sp, false, cts).ConfigureAwait(false);
                string response = soapbodynode.Value;
                XDocument document = GetDoc(response);
                List<UserDefinedFunction> rv = new List<UserDefinedFunction>();
                foreach (XElement currentnode in document.Root.Elements())
                {
                    UserDefinedFunction udf = GetUserDefinedFunctionFromXML(currentnode);
                    rv.Add(udf);
                }
                tcs.SetResult(rv);
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
        }
        private static UserDefinedFunction GetUserDefinedFunctionFromXML(XElement currentnode)
        {
            UserDefinedFunction udf = new UserDefinedFunction();
            foreach (XAttribute currentatribute in currentnode.Attributes())
            {
                switch (currentatribute.Name.LocalName)
                {
                    case "ID":
                        {
                            udf.ID = StringToInt(currentatribute.Value);
                            break;
                        }
                    case "Name":
                        {
                            udf.Name = currentatribute.Value;
                            break;
                        }
                    case "Detail":
                        {
                            udf.Detail = currentatribute.Value;
                            break;
                        }
                    case "Confirm":
                        {
                            udf.Confirm = currentatribute.Value;
                            break;
                        }
                }
            }
            return udf;
        }

        public static Task<string> RunFunction(int i, NAVFilter Filter, decimal quantity, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<string>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(null);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "RunFunction";
            XElement body =
                new XElement(myns + functionname,
                    new XElement(myns + "functionindex", i),
                    new XElement(myns + "locationCode", Filter.LocationCodeFilter),
                    new XElement(myns + "zoneCode", Filter.ZoneCodeFilter),
                    new XElement(myns + "rackID", Filter.RackIDFilter),
                    new XElement(myns + "binCode", Filter.BinCodeFilter),
                    new XElement(myns + "itemNo", Filter.ItemNoFilter),
                    new XElement(myns + "variantCode", Filter.VariantCodeFilter),
                    new XElement(myns + "quantity", quantity));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => RunFunctionNAV(tcs, sp, cts));
            return tcs.Task;
        }

        public static Task<string> RunBCTap(NAVFilter Filter, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<string>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(null);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "RunBinContentTap";
            XElement body =
                new XElement(myns + functionname,
                    new XElement(myns + "locationCode", Filter.LocationCodeFilter),
                    new XElement(myns + "zoneCode", Filter.ZoneCodeFilter),
                    new XElement(myns + "rackID", Filter.RackIDFilter),
                    new XElement(myns + "binCode", Filter.BinCodeFilter),
                    new XElement(myns + "itemNo", Filter.ItemNoFilter),
                    new XElement(myns + "description", Filter.DescriptionFilter));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(() => RunFunctionNAV(tcs, sp, cts));
            return tcs.Task;
        }


        private static async Task RunFunctionNAV(TaskCompletionSource<string> tcs, SoapParams sp, CancellationTokenSource cts)
        {
            try
            {
                XElement soapbodynode = await Process(sp, false, cts).ConfigureAwait(false);
                string rv = soapbodynode.Value;
                tcs.SetResult(rv);
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
        }
        #endregion

        #region Search
        public static Task<List<SearchResponse>> Search(string locationcode, string searchrequest, CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<List<SearchResponse>>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(null);
                return tcs.Task;
            }

            XNamespace myns = GetNameSpace();
            string functionname = "Search";
            XElement body =
                new XElement(myns + functionname,
                    new XElement(myns + "locationCode", locationcode),
                    new XElement(myns + "request", searchrequest),
                    new XElement(myns + "responseDocument", ""));
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(async () =>
                {
                    try
                    {
                        XElement soapbodynode = await Process(sp, false, cts).ConfigureAwait(false);
                        string response = soapbodynode.Value;
                        XDocument document = GetDoc(response);
                        List<SearchResponse> rv = new List<SearchResponse>();
                        foreach (XElement currentnode in document.Root.Elements())
                        {
                            SearchResponse searchresponse = GetSearchResponseFromXML(currentnode);
                            rv.Add(searchresponse);
                        }
                        tcs.SetResult(rv);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });
            return tcs.Task;
        }
        private static SearchResponse GetSearchResponseFromXML(XElement currentnode)
        {
            SearchResponse searchresponse = new SearchResponse();
            foreach (XAttribute currentatribute in currentnode.Attributes())
            {
                GetSearchResponseFromXML1(searchresponse, currentatribute);
                GetSearchResponseFromXML2(searchresponse, currentatribute);
            }
            return searchresponse;
        }

        private static void GetSearchResponseFromXML1(SearchResponse searchresponse, XAttribute currentatribute)
        {
            switch (currentatribute.Name.LocalName)
            {
                case "Z":
                    searchresponse.ZoneCode = currentatribute.Value;
                    break;
                case "B":
                    searchresponse.BinCode = currentatribute.Value;
                    break;
                case "R":
                    searchresponse.RackID = StringToInt(currentatribute.Value);
                    break;
            }
        }
        private static void GetSearchResponseFromXML2(SearchResponse searchresponse, XAttribute currentatribute)
        {
            switch (currentatribute.Name.LocalName)
            {
                case "S":
                    searchresponse.Section = StringToInt(currentatribute.Value);
                    break;
                case "L":
                    searchresponse.Level = StringToInt(currentatribute.Value);
                    break;
                case "D":
                    searchresponse.Depth = StringToInt(currentatribute.Value);
                    break;
                case "Q":
                    searchresponse.QuantityBase = (int)Math.Round(StringToDec(currentatribute.Value));
                    break;
            }
        }
        #endregion
        public static Task<int> TestConnection(CancellationTokenSource cts)
        {
            var tcs = new TaskCompletionSource<int>();
            if (IsConnectionFaild())
            {
                tcs.SetResult(0);
                return tcs.Task;
            }
            XNamespace myns = "urn:microsoft-dynamics-schemas/codeunit/" + Global.TestConnection.Codeunit;
            string functionname = "APIVersion";
            XElement body = new XElement(myns + functionname);
            SoapParams sp = new SoapParams(functionname, body, myns);
            Task.Run(async () =>
                {
                    try
                    {
                        XElement soapbodynode = await Process(sp, true, cts).ConfigureAwait(false);
                        int rv = StringToInt(soapbodynode.Value);
                        tcs.SetResult(rv);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        tcs.SetException(e);
                    }
                });

            return tcs.Task;
        }

        private static Connection SelectConnection(bool test)
        {
            if (test)
            {
                if (!(Global.TestConnection is Connection))
                {
                    return null;
                }
                return Global.TestConnection;
            }
            else
            {
                if (!(Global.CurrentConnection is Connection))
                {
                    return null;
                }
                return Global.CurrentConnection;
            }
        }

        private static HttpClientHandler GetHandler(Connection connection)
        {
            var handler = new HttpClientHandler
            {
                UseDefaultCredentials = false,
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            };

            switch (connection.ClientCredentialType)
            {
                case ClientCredentialTypeEnum.Windows:
                    handler.Credentials = new NetworkCredential(connection.Domen + "\\" + connection.User, connection.Password, "");
                    break;
                case ClientCredentialTypeEnum.Basic:
                    handler.Credentials = connection.GetCreditials();
                    break;
                default:
                    throw new InvalidOperationException("Impossible value");
            }

            return handler;
        }

        private static HttpRequestMessage CreateHttpRequest(SoapParams sp,Connection connection)
        {
            string requestbody = GetRequestText(CreateSOAPRequest(sp.SoapBody, sp.NameSpace));
            var request = new HttpRequestMessage
            {
                RequestUri = connection.GetUri(),
                Method = HttpMethod.Post
            };
            request.Content = new StringContent(requestbody, Encoding.UTF8, "text/xml");
            return request;
        }
        private static XElement GetResponseContent(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                Task<Stream> streamTask = response.Content.ReadAsStreamAsync();
                Stream stream = streamTask.Result;
                var sr = new StreamReader(stream);
                XDocument xmldoc = XDocument.Load(sr);
                XElement bodysopeenvelopenode = xmldoc.Root.Element(ns + "Body");
                if (bodysopeenvelopenode is XElement)
                {
                    return bodysopeenvelopenode;
                }
            }
            else
            {
                if (response.ReasonPhrase == "Internal Server Error")
                {
                    Task<Stream> streamTask = response.Content.ReadAsStreamAsync();
                    Stream stream = streamTask.Result;
                    var sr = new StreamReader(stream);
                    XDocument xmldoc = XDocument.Load(sr);
                    throw CreateNAVException(xmldoc);
                }
                if (response.ReasonPhrase == "Unauthorized")
                {
                    throw new NAVErrorException("Unauthorized", "Unauthorized", "Unauthorized");
                }
                else
                {
                    NAVUnknowException unknown = new NAVUnknowException(response.ReasonPhrase);
                    throw unknown;
                }
            }
            return null;
        }

        public static async Task<XElement> Process(SoapParams sp, bool testconnection, CancellationTokenSource cts)
        {
            Connection connection = SelectConnection(testconnection);
            try
            {
                var handler = GetHandler(connection);
                using (var client = new HttpClient(handler))
                {
                    HttpRequestMessage request = CreateHttpRequest(sp, connection);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                    //client.DefaultRequestHeaders.Add("SOAPAction", connection.GetSoapActionTxt() + "/" + sp.FunctionName);
                    //переход на версию SOAP 1.0 для совместимости с nAV2009
                    client.DefaultRequestHeaders.Add("SOAPAction", "\"" + connection.GetSoapActionV1() + ":" +  sp.FunctionName+ "\"");

                    using (var response = await client.SendAsync(request, cts.Token))
                    {
                        return GetResponseContent(response);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// SOAP ERROR (NAV ERROR)
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <returns></returns>
        private static NAVErrorException CreateNAVException(XDocument xmldoc)
        {
            NAVErrorException rv = null;
            XElement bodysopeenvelopenode = xmldoc.Root.Element(ns + "Body");
            if (bodysopeenvelopenode is XElement)
            {
                XElement faultnode = bodysopeenvelopenode.Element(ns + "Fault");
                if (faultnode is XElement)
                {
                    string faultcodetxt = "";
                    string faultstringtxt = "";
                    string detailstringtxt = "";
                    XElement faultcodenode = faultnode.Element("faultcode");
                    faultcodetxt = faultcodenode?.Value;
                    XElement faultstringnode = faultnode.Element("faultstring");
                    faultstringtxt = faultstringnode?.Value;
                    XElement detailnode = faultnode.Element("detail");
                    detailstringtxt = detailnode?.Value;
                    rv = new NAVErrorException(faultcodetxt, faultstringtxt, detailstringtxt);
                }
            }
            return rv;
        }

        private static XDocument CreateSOAPRequest(XElement body, XNamespace myns)
        {
            XNamespace ns = "http://schemas.xmlsoap.org/soap/envelope/";
            XNamespace nsxsi = "http://www.w3.org/2001/XMLSchema-instance";
            XNamespace nsxsd = "http://www.w3.org/2001/XMLSchema";
            XDocument requestdoc = new XDocument(
                            new XElement(ns + "Envelope",
                                new XAttribute(XNamespace.Xmlns + "soap", ns),
                                new XAttribute(XNamespace.Xmlns + "xsi", nsxsi),
                                new XAttribute(XNamespace.Xmlns + "xsd", nsxsd),
                                new XElement(ns + "Body",
                                    body)));
            requestdoc.Declaration = new XDeclaration("1.0", "UTF-8", null);

            //XDocument requestdoc = new XDocument(
            //                new XElement(ns + "Envelope",
            //                    new XAttribute(XNamespace.Xmlns + "soapenv", ns),
            //                    new XAttribute(XNamespace.Xmlns + "ns1", myns),
            //                    new XElement(ns + "Body",
            //                        body)));
            //requestdoc.Declaration = new XDeclaration("1.0", "UTF-8", null);

            return requestdoc;
        }

        private static System.Xml.Linq.XDocument GetDoc(string input)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(input);
            MemoryStream stream = new MemoryStream(byteArray);
            System.Xml.Linq.XDocument doc = XDocument.Load(stream);
            return doc;
        }

        public static string GetRequestText(XDocument doc)
        {
            string rv = "";
            var settings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = true,
                NewLineHandling = NewLineHandling.None
            };

            using (var sw = new StringWriter())
            {
                using (var xw = XmlWriter.Create(sw, settings))
                {
                    doc.WriteTo(xw);
                }
                rv = sw.ToString();
            }

            return rv;
        }

        //private class Utf8StringWriter : StringWriter
        //{
        //    public override Encoding Encoding { get { return Encoding.UTF8; } }
        //}

        public static int StringToInt(string value)
        {
            int.TryParse(value, out int int1);
            return int1;
        }
        public static decimal StringToDec(string value)
        {
            decimal res = 0;
            if (decimal.TryParse(value, out res))
            {
                return res;
            }
            else
            {
                return 0;
            }
        }
        public static bool StringToBool(string value)
        {
            if (value == "1")
            {
                return true;
            }

            if (value == "0")
            {
                return false;
            }

            if (value.ToUpper() == "TRUE")
            {
                return true;
            }

            if (value.ToUpper() == "FALSE")
            {
                return false;
            }

            if (bool.TryParse(value, out bool rv))
            {
                return rv;
            }

            return false;
        }
    }
}

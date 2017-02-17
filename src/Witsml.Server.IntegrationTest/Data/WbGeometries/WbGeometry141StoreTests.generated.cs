//----------------------------------------------------------------------- 
// PDS.Witsml.Server, 2016.1
//
// Copyright 2016 Petrotechnical Data Systems
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//-----------------------------------------------------------------------

// ----------------------------------------------------------------------
// <auto-generated>
//     Changes to this file may cause incorrect behavior and will be lost
//     if the code is regenerated.
// </auto-generated>
// ----------------------------------------------------------------------

using Energistics.DataAccess;
using Energistics.DataAccess.WITSML141;
using Energistics.DataAccess.WITSML141.ComponentSchemas;
using Energistics.DataAccess.WITSML141.ReferenceData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

using WbGeometry = Energistics.DataAccess.WITSML141.StandAloneWellboreGeometry;
using WbGeometryList = Energistics.DataAccess.WITSML141.WellboreGeometryList;

namespace PDS.Witsml.Server.Data.WbGeometries
{
    [TestClass]
    public partial class WbGeometry141StoreTests : WbGeometry141TestBase
    {
        partial void BeforeEachTest();

        partial void AfterEachTest();

        protected override void OnTestSetUp()
        {
            BeforeEachTest();
        }

        protected override void OnTestCleanUp()
        {
            AfterEachTest();
        }

        [TestMethod]
        public void WbGeometry141DataAdapter_GetFromStore_Can_Get_WbGeometry()
        {
            AddParents();
            DevKit.AddAndAssert<WbGeometryList, WbGeometry>(WbGeometry);
            DevKit.GetAndAssert<WbGeometryList, WbGeometry>(WbGeometry);
       }

        [TestMethod]
        public void WbGeometry141DataAdapter_AddToStore_Can_Add_WbGeometry()
        {
            AddParents();
            DevKit.AddAndAssert<WbGeometryList, WbGeometry>(WbGeometry);
        }

        [TestMethod]
        public void WbGeometry141DataAdapter_UpdateInStore_Can_Update_WbGeometry()
        {
            AddParents();
            DevKit.AddAndAssert<WbGeometryList, WbGeometry>(WbGeometry);
            DevKit.UpdateAndAssert<WbGeometryList, WbGeometry>(WbGeometry);
            DevKit.GetAndAssert<WbGeometryList, WbGeometry>(WbGeometry);
        }

        [TestMethod]
        public void WbGeometry141DataAdapter_DeleteFromStore_Can_Delete_WbGeometry()
        {
            AddParents();
            DevKit.AddAndAssert<WbGeometryList, WbGeometry>(WbGeometry);
            DevKit.DeleteAndAssert<WbGeometryList, WbGeometry>(WbGeometry);
            DevKit.GetAndAssert<WbGeometryList, WbGeometry>(WbGeometry, isNotNull: false);
        }

        [TestMethod]
        public void WbGeometry141WitsmlStore_GetFromStore_Can_Transform_WbGeometry()
        {
            AddParents();
            DevKit.AddAndAssert<WbGeometryList, WbGeometry>(WbGeometry);

            // Re-initialize all capServer providers
            DevKit.Store.CapServerProviders = null;
            DevKit.Container.BuildUp(DevKit.Store);

            string typeIn, queryIn;
            var query = DevKit.List(DevKit.CreateQuery(WbGeometry));
            DevKit.SetupParameters<WbGeometryList, WbGeometry>(query, ObjectTypes.WbGeometry, out typeIn, out queryIn);

            var options = OptionsIn.Join(OptionsIn.ReturnElements.All, OptionsIn.DataVersion.Version131);
            var request = new WMLS_GetFromStoreRequest(typeIn, queryIn, options, null);
            var response = DevKit.Store.WMLS_GetFromStore(request);

            Assert.IsFalse(string.IsNullOrWhiteSpace(response.XMLout));
            Assert.AreEqual((short)ErrorCodes.Success, response.Result);

            var result = WitsmlParser.Parse(response.XMLout);
            var version = ObjectTypes.GetVersion(result.Root);
            Assert.AreEqual(OptionsIn.DataVersion.Version131.Value, version);
        }

        [TestMethod]
        public void WbGeometry141DataAdapter_AddToStore_Creates_ChangeLog()
        {
            AddParents();

            DevKit.AddAndAssert<WbGeometryList, WbGeometry>(WbGeometry);

            var result = DevKit.GetAndAssert<WbGeometryList, WbGeometry>(WbGeometry);
            var expectedHistoryCount = 1;
            var expectedChangeType = ChangeInfoType.add;
            DevKit.AssertChangeLog(result, expectedHistoryCount, expectedChangeType);
        }

        [TestMethod]
        public void WbGeometry141DataAdapter_UpdateInStore_Updates_ChangeLog()
        {
            AddParents();

            DevKit.AddAndAssert<WbGeometryList, WbGeometry>(WbGeometry);

            // Update the WbGeometry141
            WbGeometry.Name = "Change";
            DevKit.UpdateAndAssert(WbGeometry);

            var result = DevKit.GetAndAssert<WbGeometryList, WbGeometry>(WbGeometry);
            var expectedHistoryCount = 2;
            var expectedChangeType = ChangeInfoType.update;
            DevKit.AssertChangeLog(result, expectedHistoryCount, expectedChangeType);
        }

        [TestMethod]
        public void WbGeometry141DataAdapter_DeleteFromStore_Updates_ChangeLog()
        {
            AddParents();

            DevKit.AddAndAssert<WbGeometryList, WbGeometry>(WbGeometry);

            // Delete the WbGeometry141
            DevKit.DeleteAndAssert(WbGeometry);

            var expectedHistoryCount = 2;
            var expectedChangeType = ChangeInfoType.delete;
            DevKit.AssertChangeLog(WbGeometry, expectedHistoryCount, expectedChangeType);
        }

        [TestMethod]
        public void WbGeometry141DataAdapter_ChangeLog_Tracks_ChangeHistory_For_Add_Update_Delete()
        {
            AddParents();

            // Add the WbGeometry141
            DevKit.AddAndAssert<WbGeometryList, WbGeometry>(WbGeometry);

            // Verify ChangeLog for Add
            var result = DevKit.GetAndAssert<WbGeometryList, WbGeometry>(WbGeometry);
            var expectedHistoryCount = 1;
            var expectedChangeType = ChangeInfoType.add;
            DevKit.AssertChangeLog(result, expectedHistoryCount, expectedChangeType);

            // Update the WbGeometry141
            WbGeometry.Name = "Change";
            DevKit.UpdateAndAssert(WbGeometry);

            result = DevKit.GetAndAssert<WbGeometryList, WbGeometry>(WbGeometry);
            expectedHistoryCount = 2;
            expectedChangeType = ChangeInfoType.update;
            DevKit.AssertChangeLog(result, expectedHistoryCount, expectedChangeType);

            // Delete the WbGeometry141
            DevKit.DeleteAndAssert(WbGeometry);

            expectedHistoryCount = 3;
            expectedChangeType = ChangeInfoType.delete;
            DevKit.AssertChangeLog(WbGeometry, expectedHistoryCount, expectedChangeType);

            // Re-add the same WbGeometry141...
            DevKit.AddAndAssert<WbGeometryList, WbGeometry>(WbGeometry);

            //... the same changeLog should be reused.
            result = DevKit.GetAndAssert<WbGeometryList, WbGeometry>(WbGeometry);
            expectedHistoryCount = 4;
            expectedChangeType = ChangeInfoType.add;
            DevKit.AssertChangeLog(result, expectedHistoryCount, expectedChangeType);

            DevKit.AssertChangeHistoryTimesUnique(result);
        }

        [TestMethod]
        public void WbGeometry141DataAdapter_GetFromStore_Filter_ExtensionNameValue()
        {
            AddParents();

            var extensionName1 = DevKit.ExtensionNameValue("Ext-1", "1.0", "m");
            var extensionName2 = DevKit.ExtensionNameValue("Ext-2", "2.0", "cm", PrimitiveType.@float);
            extensionName2.MeasureClass = MeasureClass.Length;
            var extensionName3 = DevKit.ExtensionNameValue("Ext-3", "3.0", "cm", PrimitiveType.unknown);

            WbGeometry.CommonData = new CommonData()
            {
                ExtensionNameValue = new List<ExtensionNameValue>()
                {
                    extensionName1, extensionName2, extensionName3
                }
            };

            // Add the WbGeometry141
            DevKit.AddAndAssert(WbGeometry);

            // Query for first extension
            var commonDataXml = "<commonData>" + Environment.NewLine +
                                "<extensionNameValue uid=\"\">" + Environment.NewLine +
                                "<name />{0}" + Environment.NewLine +
                                "</extensionNameValue>" + Environment.NewLine +
                                "</commonData>";

            var extValueQuery = string.Format(commonDataXml, "<dataType>double</dataType>");
            var queryXml = string.Format(BasicXMLTemplate, WbGeometry.UidWell, WbGeometry.UidWellbore, WbGeometry.Uid, extValueQuery);
            var result = DevKit.Query<WbGeometryList, WbGeometry>(ObjectTypes.WbGeometry, queryXml, null, OptionsIn.ReturnElements.Requested);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            var resultWbGeometry = result[0];
            Assert.IsNotNull(resultWbGeometry);

            var commonData = resultWbGeometry.CommonData;
            Assert.IsNotNull(commonData);
            Assert.AreEqual(1, commonData.ExtensionNameValue.Count);

            var env = commonData.ExtensionNameValue[0];
            Assert.IsNotNull(env);
            Assert.AreEqual(extensionName1.Uid, env.Uid);
            Assert.AreEqual(extensionName1.Name, env.Name);

            // Query for second extension
            extValueQuery = string.Format(commonDataXml, "<measureClass>length</measureClass>");
            queryXml = string.Format(BasicXMLTemplate, WbGeometry.UidWell, WbGeometry.UidWellbore, WbGeometry.Uid, extValueQuery);
            result = DevKit.Query<WbGeometryList, WbGeometry>(ObjectTypes.WbGeometry, queryXml, null, OptionsIn.ReturnElements.Requested);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            resultWbGeometry = result[0];
            Assert.IsNotNull(resultWbGeometry);

            commonData = resultWbGeometry.CommonData;
            Assert.IsNotNull(commonData);
            Assert.AreEqual(1, commonData.ExtensionNameValue.Count);

            env = commonData.ExtensionNameValue[0];
            Assert.IsNotNull(env);
            Assert.AreEqual(extensionName2.Uid, env.Uid);
            Assert.AreEqual(extensionName2.Name, env.Name);

            // Query for third extension
            extValueQuery = string.Format(commonDataXml, "<dataType>unknown</dataType>");
            queryXml = string.Format(BasicXMLTemplate, WbGeometry.UidWell, WbGeometry.UidWellbore, WbGeometry.Uid, extValueQuery);
            result = DevKit.Query<WbGeometryList, WbGeometry>(ObjectTypes.WbGeometry, queryXml, null, OptionsIn.ReturnElements.Requested);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            resultWbGeometry = result[0];
            Assert.IsNotNull(resultWbGeometry);

            commonData = resultWbGeometry.CommonData;
            Assert.IsNotNull(commonData);
            Assert.AreEqual(1, commonData.ExtensionNameValue.Count);

            env = commonData.ExtensionNameValue[0];
            Assert.IsNotNull(env);
            Assert.AreEqual(extensionName3.Uid, env.Uid);
            Assert.AreEqual(extensionName3.Name, env.Name);
        }

        [TestMethod]
        public void WbGeometry141DataAdapter_ChangeLog_Syncs_WbGeometry_Name_Changes()
        {
            AddParents();

            // Add the WbGeometry141
            DevKit.AddAndAssert<WbGeometryList, WbGeometry>(WbGeometry);

            // Assert that all WbGeometry names match corresponding changeLog names
            DevKit.AssertChangeLogNames(WbGeometry);

            // Update the WbGeometry141 names
            WbGeometry.Name = "Change";
            WbGeometry.NameWell = "Well Name Change";

            WbGeometry.NameWellbore = "Wellbore Name Change";

            DevKit.UpdateAndAssert(WbGeometry);

            // Assert that all WbGeometry names match corresponding changeLog names
            DevKit.AssertChangeLogNames(WbGeometry);
        }
    }
}
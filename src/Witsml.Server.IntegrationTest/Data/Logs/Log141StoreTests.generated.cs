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

namespace PDS.Witsml.Server.Data.Logs
{
    [TestClass]
    public partial class Log141StoreTests : Log141TestBase
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
        public void Log141DataAdapter_GetFromStore_Can_Get_Log()
        {
            AddParents();
            DevKit.AddAndAssert<LogList, Log>(Log);
            DevKit.GetAndAssert<LogList, Log>(Log);
       }

        [TestMethod]
        public void Log141DataAdapter_AddToStore_Can_Add_Log()
        {
            AddParents();
            DevKit.AddAndAssert<LogList, Log>(Log);
        }

        [TestMethod]
        public void Log141DataAdapter_UpdateInStore_Can_Update_Log()
        {
            AddParents();
            DevKit.AddAndAssert<LogList, Log>(Log);
            DevKit.UpdateAndAssert<LogList, Log>(Log);
            DevKit.GetAndAssert<LogList, Log>(Log);
        }

        [TestMethod]
        public void Log141DataAdapter_DeleteFromStore_Can_Delete_Log()
        {
            AddParents();
            DevKit.AddAndAssert<LogList, Log>(Log);
            DevKit.DeleteAndAssert<LogList, Log>(Log);
            DevKit.GetAndAssert<LogList, Log>(Log, isNotNull: false);
        }

        [TestMethod]
        public void Log141WitsmlStore_GetFromStore_Can_Transform_Log()
        {
            AddParents();
            DevKit.AddAndAssert<LogList, Log>(Log);

            // Re-initialize all capServer providers
            DevKit.Store.CapServerProviders = null;
            DevKit.Container.BuildUp(DevKit.Store);

            string typeIn, queryIn;
            var query = DevKit.List(DevKit.CreateQuery(Log));
            DevKit.SetupParameters<LogList, Log>(query, ObjectTypes.Log, out typeIn, out queryIn);

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
        public void Log141DataAdapter_AddToStore_Creates_ChangeLog()
        {
            AddParents();

            DevKit.AddAndAssert<LogList, Log>(Log);

            var result = DevKit.GetAndAssert<LogList, Log>(Log);
            var expectedHistoryCount = 1;
            var expectedChangeType = ChangeInfoType.add;
            DevKit.AssertChangeLog(result, expectedHistoryCount, expectedChangeType);
        }

        [TestMethod]
        public void Log141DataAdapter_UpdateInStore_Updates_ChangeLog()
        {
            AddParents();

            DevKit.AddAndAssert<LogList, Log>(Log);

            // Update the Log141
            Log.Name = "Change";
            DevKit.UpdateAndAssert(Log);

            var result = DevKit.GetAndAssert<LogList, Log>(Log);
            var expectedHistoryCount = 2;
            var expectedChangeType = ChangeInfoType.update;
            DevKit.AssertChangeLog(result, expectedHistoryCount, expectedChangeType);
        }

        [TestMethod]
        public void Log141DataAdapter_DeleteFromStore_Updates_ChangeLog()
        {
            AddParents();

            DevKit.AddAndAssert<LogList, Log>(Log);

            // Delete the Log141
            DevKit.DeleteAndAssert(Log);

            var expectedHistoryCount = 2;
            var expectedChangeType = ChangeInfoType.delete;
            DevKit.AssertChangeLog(Log, expectedHistoryCount, expectedChangeType);
        }

        [TestMethod]
        public void Log141DataAdapter_ChangeLog_Tracks_ChangeHistory_For_Add_Update_Delete()
        {
            AddParents();

            // Add the Log141
            DevKit.AddAndAssert<LogList, Log>(Log);

            // Verify ChangeLog for Add
            var result = DevKit.GetAndAssert<LogList, Log>(Log);
            var expectedHistoryCount = 1;
            var expectedChangeType = ChangeInfoType.add;
            DevKit.AssertChangeLog(result, expectedHistoryCount, expectedChangeType);

            // Update the Log141
            Log.Name = "Change";
            DevKit.UpdateAndAssert(Log);

            result = DevKit.GetAndAssert<LogList, Log>(Log);
            expectedHistoryCount = 2;
            expectedChangeType = ChangeInfoType.update;
            DevKit.AssertChangeLog(result, expectedHistoryCount, expectedChangeType);

            // Delete the Log141
            DevKit.DeleteAndAssert(Log);

            expectedHistoryCount = 3;
            expectedChangeType = ChangeInfoType.delete;
            DevKit.AssertChangeLog(Log, expectedHistoryCount, expectedChangeType);

            // Re-add the same Log141...
            DevKit.AddAndAssert<LogList, Log>(Log);

            //... the same changeLog should be reused.
            result = DevKit.GetAndAssert<LogList, Log>(Log);
            expectedHistoryCount = 4;
            expectedChangeType = ChangeInfoType.add;
            DevKit.AssertChangeLog(result, expectedHistoryCount, expectedChangeType);

            DevKit.AssertChangeHistoryTimesUnique(result);
        }

        [TestMethod]
        public void Log141DataAdapter_GetFromStore_Filter_ExtensionNameValue()
        {
            AddParents();

            var extensionName1 = DevKit.ExtensionNameValue("Ext-1", "1.0", "m");
            var extensionName2 = DevKit.ExtensionNameValue("Ext-2", "2.0", "cm", PrimitiveType.@float);
            extensionName2.MeasureClass = MeasureClass.Length;
            var extensionName3 = DevKit.ExtensionNameValue("Ext-3", "3.0", "cm", PrimitiveType.unknown);

            Log.CommonData = new CommonData()
            {
                ExtensionNameValue = new List<ExtensionNameValue>()
                {
                    extensionName1, extensionName2, extensionName3
                }
            };

            // Add the Log141
            DevKit.AddAndAssert(Log);

            // Query for first extension
            var commonDataXml = "<commonData>" + Environment.NewLine +
                                "<extensionNameValue uid=\"\">" + Environment.NewLine +
                                "<name />{0}" + Environment.NewLine +
                                "</extensionNameValue>" + Environment.NewLine +
                                "</commonData>";

            var extValueQuery = string.Format(commonDataXml, "<dataType>double</dataType>");
            var queryXml = string.Format(BasicXMLTemplate, Log.UidWell, Log.UidWellbore, Log.Uid, extValueQuery);
            var result = DevKit.Query<LogList, Log>(ObjectTypes.Log, queryXml, null, OptionsIn.ReturnElements.Requested);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            var resultLog = result[0];
            Assert.IsNotNull(resultLog);

            var commonData = resultLog.CommonData;
            Assert.IsNotNull(commonData);
            Assert.AreEqual(1, commonData.ExtensionNameValue.Count);

            var env = commonData.ExtensionNameValue[0];
            Assert.IsNotNull(env);
            Assert.AreEqual(extensionName1.Uid, env.Uid);
            Assert.AreEqual(extensionName1.Name, env.Name);

            // Query for second extension
            extValueQuery = string.Format(commonDataXml, "<measureClass>length</measureClass>");
            queryXml = string.Format(BasicXMLTemplate, Log.UidWell, Log.UidWellbore, Log.Uid, extValueQuery);
            result = DevKit.Query<LogList, Log>(ObjectTypes.Log, queryXml, null, OptionsIn.ReturnElements.Requested);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            resultLog = result[0];
            Assert.IsNotNull(resultLog);

            commonData = resultLog.CommonData;
            Assert.IsNotNull(commonData);
            Assert.AreEqual(1, commonData.ExtensionNameValue.Count);

            env = commonData.ExtensionNameValue[0];
            Assert.IsNotNull(env);
            Assert.AreEqual(extensionName2.Uid, env.Uid);
            Assert.AreEqual(extensionName2.Name, env.Name);

            // Query for third extension
            extValueQuery = string.Format(commonDataXml, "<dataType>unknown</dataType>");
            queryXml = string.Format(BasicXMLTemplate, Log.UidWell, Log.UidWellbore, Log.Uid, extValueQuery);
            result = DevKit.Query<LogList, Log>(ObjectTypes.Log, queryXml, null, OptionsIn.ReturnElements.Requested);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            resultLog = result[0];
            Assert.IsNotNull(resultLog);

            commonData = resultLog.CommonData;
            Assert.IsNotNull(commonData);
            Assert.AreEqual(1, commonData.ExtensionNameValue.Count);

            env = commonData.ExtensionNameValue[0];
            Assert.IsNotNull(env);
            Assert.AreEqual(extensionName3.Uid, env.Uid);
            Assert.AreEqual(extensionName3.Name, env.Name);
        }

        [TestMethod]
        public void Log141DataAdapter_ChangeLog_Syncs_Log_Name_Changes()
        {
            AddParents();

            // Add the Log141
            DevKit.AddAndAssert<LogList, Log>(Log);

            // Assert that all Log names match corresponding changeLog names
            DevKit.AssertChangeLogNames(Log);

            // Update the Log141 names
            Log.Name = "Change";
            Log.NameWell = "Well Name Change";

            Log.NameWellbore = "Wellbore Name Change";

            DevKit.UpdateAndAssert(Log);

            // Assert that all Log names match corresponding changeLog names
            DevKit.AssertChangeLogNames(Log);
        }

        [TestMethod]
        public void Log141DataAdapter_Log_ObjectGrowing_Not_Toggled_By_Client()
        {
            AddParents();

            // Set the object growing flag to true
            Log.ObjectGrowing = true;

            // Add the Log141
            DevKit.AddAndAssert<LogList, Log>(Log);

            // Assert Log141 is not growing
            var result = DevKit.GetAndAssert(Log);
            Assert.IsFalse(result.ObjectGrowing.GetValueOrDefault(), "Log ObjectGrowing");

            DevKit.UpdateAndAssert(Log);

            // Assert Log141 is not growing
            result = DevKit.GetAndAssert(Log);
            Assert.IsFalse(result.ObjectGrowing.GetValueOrDefault(), "Log ObjectGrowing");
        }
    }
}
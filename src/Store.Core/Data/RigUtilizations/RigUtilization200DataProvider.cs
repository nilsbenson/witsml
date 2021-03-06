﻿//----------------------------------------------------------------------- 
// PDS WITSMLstudio Store, 2017.1
//
// Copyright 2017 Petrotechnical Data Systems
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

using Energistics.DataAccess.WITSML200;
using Energistics.Datatypes;

namespace PDS.WITSMLstudio.Store.Data.RigUtilizations
{
    /// <summary>
    /// RigUtilization200DataProvider
    /// </summary>
    public partial class RigUtilization200DataProvider
    {
        /// <summary>
        /// Sets the additional default values.
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        /// <param name="uri">The URI.</param>
        partial void SetAdditionalDefaultValues(RigUtilization dataObject, EtpUri uri)
        {
            var rigUri = uri.Parent;
            var wellboreUri = rigUri.Parent;

            dataObject.Rig = dataObject.Rig.Create<Rig>(rigUri);
            dataObject.Wellbore = dataObject.Wellbore.Create<Wellbore>(wellboreUri);
        }
    }
}

// <auto-generated>
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
//
// </auto-generated>

namespace Microsoft.DotNet.Helix.Client.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class AnalysisReport
    {
        /// <summary>
        /// Initializes a new instance of the AnalysisReport class.
        /// </summary>
        public AnalysisReport()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the AnalysisReport class.
        /// </summary>
        public AnalysisReport(XUnitWorkItemResult xunit = default(XUnitWorkItemResult), ExternalLinkResult external = default(ExternalLinkResult))
        {
            Xunit = xunit;
            External = external;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "xunit")]
        public XUnitWorkItemResult Xunit { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "external")]
        public ExternalLinkResult External { get; set; }

    }
}

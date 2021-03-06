// <auto-generated>
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
//
// </auto-generated>

namespace Microsoft.DotNet.Maestro.Client.Models
{
    using Microsoft.Rest;
    using Newtonsoft.Json;
    using System.Linq;

    public partial class SubscriptionData
    {
        /// <summary>
        /// Initializes a new instance of the SubscriptionData class.
        /// </summary>
        public SubscriptionData()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the SubscriptionData class.
        /// </summary>
        public SubscriptionData(string channelName, string sourceRepository, string targetRepository, string targetBranch, SubscriptionPolicy policy)
        {
            ChannelName = channelName;
            SourceRepository = sourceRepository;
            TargetRepository = targetRepository;
            TargetBranch = targetBranch;
            Policy = policy;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "channelName")]
        public string ChannelName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "sourceRepository")]
        public string SourceRepository { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "targetRepository")]
        public string TargetRepository { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "targetBranch")]
        public string TargetBranch { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "policy")]
        public SubscriptionPolicy Policy { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (ChannelName == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "ChannelName");
            }
            if (SourceRepository == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "SourceRepository");
            }
            if (TargetRepository == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "TargetRepository");
            }
            if (TargetBranch == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "TargetBranch");
            }
            if (Policy == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Policy");
            }
        }
    }
}

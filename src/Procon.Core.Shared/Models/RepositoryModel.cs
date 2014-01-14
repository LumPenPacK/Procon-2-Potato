﻿using System;
using System.Collections.Generic;

namespace Procon.Core.Shared.Models {
    /// <summary>
    /// A nuget repository source known to Procon.core
    /// </summary>
    [Serializable]
    public class RepositoryModel : CoreModel {
        /// <summary>
        /// The base url of the repository
        /// </summary>
        public String Uri { get; set; }

        /// <summary>
        /// Short directory safe url
        /// </summary>
        public String Slug { get; set; }

        /// <summary>
        /// The name of this repository
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// List of packages available in the repository
        /// </summary>
        public List<PackageWrapperModel> Packages { get; set; }

        /// <summary>
        /// If this is the location orphaned packages should be added if we cannot find a source for them.
        /// </summary>
        public bool IsOrphanage { get; set; }

        /// <summary>
        /// Stores the last error that occured during a cache rebuild.
        /// </summary>
        public String CacheError { get; set; }

        /// <summary>
        /// When the repository was last cached.
        /// </summary>
        public DateTime CacheStamp { get; set; }

        /// <summary>
        /// Initializes a repository model with the default values.
        /// </summary>
        public RepositoryModel() : base() {
            this.Packages = new List<PackageWrapperModel>();
        }
    }
}

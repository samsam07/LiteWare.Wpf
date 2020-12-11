using System;

namespace LiteWare.Wpf
{
    /// <summary>
    /// Enumerate the different methods used to fetch content from a <see cref="Uri"/>.
    /// </summary>
    public enum UriResolutionType
    {
        /// <summary>
        /// Content is fetched from the web.
        /// </summary>
        Web,

        /// <summary>
        /// Content is fetched from a file location.
        /// </summary>
        File,

        /// <summary>
        /// Content is fetched as package resource.
        /// </summary>
        PackageResource,

        /// <summary>
        /// Content is fetched as package content.
        /// </summary>
        PackageContent,

        /// <summary>
        /// Content is fetched from site or origin.
        /// </summary>
        PackageSiteOfOrigin
    }
}
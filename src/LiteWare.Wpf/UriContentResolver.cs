// MIT License
//
// Copyright (c) 2020 Hisham Maudarbocus
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.IO;
using System.IO.Packaging;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Resources;

namespace LiteWare.Wpf
{
    /// <summary>
    /// Allows fetching of stream resource content from <see cref="Uri"/>.
    /// </summary>
    public static class UriContentResolver
    {
        /// <summary>
        /// Fetches the web content from the provided <paramref name="uri"/> as a <see cref="ContentResourceInfo"/>.
        /// </summary>
        /// <param name="uri">The URI of the content.</param>
        /// <returns>A <see cref="ContentResourceInfo"/> containing the content from the <paramref name="uri"/>.</returns>
        public static ContentResourceInfo ResolveFromWeb(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            WebRequest webRequest = WebRequest.Create(uri);
            WebResponse webResponse = webRequest.GetResponse();

            ContentResourceInfo contentResourceInfo = new ContentResourceInfo(webResponse.GetResponseStream(), webResponse.ContentType, UriResolutionType.Web);
            return contentResourceInfo;
        }

        /// <summary>
        /// Fetches the file content from the provided <paramref name="uri"/> as a <see cref="ContentResourceInfo"/>.
        /// </summary>
        /// <param name="uri">The URI of the content.</param>
        /// <returns>A <see cref="ContentResourceInfo"/> containing the content from the <paramref name="uri"/>.</returns>
        public static ContentResourceInfo ResolveFromFile(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            if (uri.IsAbsoluteUri && !uri.IsFile)
            {
                throw new ArgumentException("Absolute uri is not in a file form.");
            }

            Assembly assembly = Assembly.GetEntryAssembly();
            if (!uri.IsAbsoluteUri && assembly != null) // Relative to current directory
            {
                string currentDirectory = Path.GetDirectoryName(assembly.Location) + "\\";
                Uri baseUri = new Uri(currentDirectory);

                uri = new Uri(baseUri, uri);
            }

            WebRequest webRequest = WebRequest.Create(uri);
            WebResponse webResponse = webRequest.GetResponse();

            ContentResourceInfo contentResourceInfo = new ContentResourceInfo(webResponse.GetResponseStream(), webResponse.ContentType, UriResolutionType.File);
            return contentResourceInfo;
        }

        /// <summary>
        /// Fetches the package resource content from the provided <paramref name="uri"/> as a <see cref="ContentResourceInfo"/>.
        /// </summary>
        /// <param name="uri">The URI of the content.</param>
        /// <returns>A <see cref="ContentResourceInfo"/> containing the content from the <paramref name="uri"/>. Returns null if content is not found.</returns>
        public static ContentResourceInfo ResolveFromPackageResource(Uri uri)
        {
            StreamResourceInfo streamResourceInfo = Application.GetResourceStream(uri);

            ContentResourceInfo contentResourceInfo = null;
            if (streamResourceInfo != null)
            {
                contentResourceInfo = new ContentResourceInfo(streamResourceInfo, UriResolutionType.PackageResource);
            }

            return contentResourceInfo;
        }

        /// <summary>
        /// Fetches the package content from the provided <paramref name="uri"/> as a <see cref="ContentResourceInfo"/>.
        /// </summary>
        /// <param name="uri">The URI of the content.</param>
        /// <returns>A <see cref="ContentResourceInfo"/> containing the content from the <paramref name="uri"/>. Returns null if content is not found.</returns>
        public static ContentResourceInfo ResolveFromPackageContent(Uri uri)
        {
            StreamResourceInfo streamResourceInfo = Application.GetContentStream(uri);

            ContentResourceInfo contentResourceInfo = null;
            if (streamResourceInfo != null)
            {
                contentResourceInfo = new ContentResourceInfo(streamResourceInfo, UriResolutionType.PackageContent);
            }

            return contentResourceInfo;
        }

        /// <summary>
        /// Fetches the package site of origin content from the provided <paramref name="uri"/> as a <see cref="ContentResourceInfo"/>.
        /// </summary>
        /// <param name="uri">The URI of the content.</param>
        /// <returns>A <see cref="ContentResourceInfo"/> containing the content from the <paramref name="uri"/>. Returns null if content is not found.</returns>
        public static ContentResourceInfo ResolveFromPackageSiteOfOrigin(Uri uri)
        {
            StreamResourceInfo streamResourceInfo = Application.GetRemoteStream(uri);

            ContentResourceInfo contentResourceInfo = null;
            if (streamResourceInfo != null)
            {
                contentResourceInfo = new ContentResourceInfo(streamResourceInfo, UriResolutionType.PackageSiteOfOrigin);
            }

            return contentResourceInfo;
        }

        private static ContentResourceInfo ResolveByTrial(Uri uri, params UriResolutionType[] trialResolutionTypes)
        {
            ContentResourceInfo contentResourceInfo = null;
            foreach (UriResolutionType trialResolutionType in trialResolutionTypes)
            {
                try
                {
                    contentResourceInfo = trialResolutionType switch
                    {
                        UriResolutionType.Web => ResolveFromWeb(uri),
                        UriResolutionType.File => ResolveFromFile(uri),
                        UriResolutionType.PackageResource => ResolveFromPackageResource(uri),
                        UriResolutionType.PackageContent => ResolveFromPackageContent(uri),
                        UriResolutionType.PackageSiteOfOrigin => ResolveFromPackageSiteOfOrigin(uri)
                    };

                    if (contentResourceInfo != null)
                    {
                        break;
                    }
                }
                catch { /* Ignored */ }
            }

            return contentResourceInfo;
        }

        /// <summary>
        /// Fetches the content from the provided <paramref name="uri"/> as a <see cref="ContentResourceInfo"/> by automatically determining the fetching method.
        /// </summary>
        /// <param name="uri">The URI of the content.</param>
        /// <returns>A <see cref="ContentResourceInfo"/> containing the content from the <paramref name="uri"/>. Returns null if content is not found.</returns>
        public static ContentResourceInfo Resolve(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            ContentResourceInfo contentResourceInfo;
            if (uri.IsAbsoluteUri)
            {
                if (uri.Scheme == PackUriHelper.UriSchemePack)
                {
                    contentResourceInfo = uri.Host.StartsWith("siteoforigin")
                        ? ResolveByTrial(uri, UriResolutionType.PackageSiteOfOrigin)
                        : ResolveByTrial(uri, UriResolutionType.PackageResource, UriResolutionType.PackageContent);
                }
                else if (uri.IsFile)
                {
                    contentResourceInfo = ResolveByTrial(uri, UriResolutionType.File);
                }
                else
                {
                    contentResourceInfo = ResolveByTrial(uri, UriResolutionType.Web);
                }
            }
            else
            {
                contentResourceInfo = ResolveByTrial
                (
                    uri,
                    UriResolutionType.PackageResource,
                    UriResolutionType.PackageContent,
                    UriResolutionType.File,
                    UriResolutionType.PackageSiteOfOrigin
                );
            }

            return contentResourceInfo;
        }
    }
}
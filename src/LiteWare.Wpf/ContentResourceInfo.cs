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
using System.Windows.Resources;

namespace LiteWare.Wpf
{
    /// <summary>
    /// Stores information of a stream resource content fetched from a <see cref="Uri"/>.
    /// </summary>
    public class ContentResourceInfo : StreamResourceInfo
    {
        /// <summary>
        /// Gets the resolution method used to fetch the content.
        /// </summary>
        public UriResolutionType ResolutionType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentResourceInfo"/> class.
        /// </summary>
        public ContentResourceInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentResourceInfo"/> class based on a provided stream.
        /// </summary>
        /// <param name="stream">The reference stream.</param>
        /// <param name="contentType">The Multipurpose Internet Mail Extensions (MIME) content type of the stream.</param>
        /// <param name="resolutionType">The resolution method use to fetch the content.</param>
        public ContentResourceInfo(Stream stream, string contentType, UriResolutionType resolutionType)
            : base(stream, contentType)
        {
            ResolutionType = resolutionType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentResourceInfo"/> class based on a provided <see cref="StreamResourceInfo"/>.
        /// </summary>
        /// <param name="streamResourceInfo">Stream resource information.</param>
        /// <param name="resolutionType">The resolution method use to fetch the content.</param>
        public ContentResourceInfo(StreamResourceInfo streamResourceInfo, UriResolutionType resolutionType)
            : this(streamResourceInfo.Stream, streamResourceInfo.ContentType, resolutionType) { }

        /// <summary>
        /// Reads the whole content stream as a string. The stream is then disposed.
        /// </summary>
        /// <returns>The string represented by the content stream.</returns>
        public string ReadAllString()
        {
            using Stream stream = Stream;
            using StreamReader streamReader = new StreamReader(stream);

            string stringContent = streamReader.ReadToEnd();
            return stringContent;
        }
    }
}
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
using System.ComponentModel;

namespace Maxsys.MediaManager.MusicContext.Domain.ValueObjects
{
    /// <summary>
    /// Provides extensions methods for <see cref="AlbumType"/>
    /// </summary>
    public static class AlbumTypeExtensions
    {
        /// <summary>
        /// Converts the value of this instance to its friendly name string representation
        /// provided by a <see cref="DescriptionAttribute"/>.
        /// </summary>
        /// <param name="value">An <see cref="AlbumType"/> value.</param>
        /// <returns>The friendly name string representation of the value of this instance.</returns>
        public static string ToFriendlyName(this AlbumType value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var attributes = (DescriptionAttribute[])fieldInfo
                .GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes != null && attributes.Length > 0
                ? attributes[0].Description
                : value.ToString();
        }
    }
}
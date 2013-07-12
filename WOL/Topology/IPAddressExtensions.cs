﻿#if NET35

using System.Collections.Generic;
using System.Collections;

namespace System.Net.Topology
{
    /// <summary>Provides extension methods for the <see cref="T:System.Net.IPAddress"/>.</summary>
    public static class IPAddressExtensions
    {
        private const string OnlyIPv4Supported = "Only IPv4 is currently supported";

        /// <summary>Gets the number of siblings an <see cref="T:System.Net.IPAddress"/> can have in a given network. Compliant to RFC 950 (2^n-2).</summary>
        /// <param name="address">The address</param>
        /// <param name="mask">The net mask of the network</param>
        /// <returns>The number of siblings an <see cref="T:System.Net.IPAddress"/> can have in the given network.</returns>
        public static int GetSiblingCount(this IPAddress address, NetMask mask)
        {
            // wtf, no address needed?
            // TODO: Move method to other class.

            return GetSiblingCount(address, mask, SiblingOptions.ExcludeUnusable);
        }

        /// <summary>Gets the number of siblings an <see cref="T:System.Net.IPAddress"/> can have in a given network.</summary>
        /// <param name="address">The address</param>
        /// <param name="mask">The net mask of the network</param>
        /// <param name="options">Options which addresses to include an which not</param>
        /// <returns>The number of siblings an <see cref="T:System.Net.IPAddress"/> can have in the given network.</returns>
        public static int GetSiblingCount(this IPAddress address, NetMask mask, SiblingOptions options)
        {
            // wtf, no address needed?
            // TODO: Move method to other class.

            if (address == null)
                throw new ArgumentNullException("address");
            if (mask == null)
                throw new ArgumentNullException("mask");
            if (address.AddressFamily != Sockets.AddressFamily.InterNetwork)
                throw new NotSupportedException(OnlyIPv4Supported);

            bool includeSelf = BitHelper.IsOptionSet(options, SiblingOptions.IncludeSelf);
            bool includeBroadcast = BitHelper.IsOptionSet(options, SiblingOptions.IncludeBroadcast);
            bool includeNetworkIdentifier = BitHelper.IsOptionSet(options, SiblingOptions.IncludeNetworkIdentifier);

            var hostPartBits = mask.GetMaskBytes().CountFromRight(false);
            var total = 1 << hostPartBits;
            total -= includeSelf ? 1 : 0;
            total -= includeBroadcast ? 1 : 0;
            total -= includeNetworkIdentifier ? 1 : 0;

            // TODO: Testing

            return total;
        }

        /// <summary>Enumerates through the siblings of an <see cref="T:System.Net.IPAddress"/> in a network. Compliant to RFC 950 (2^n-2).</summary>
        /// <param name="address">The address</param>
        /// <param name="mask">The net mask of the network</param>
        public static IEnumerable<IPAddress> GetSiblings(this IPAddress address, NetMask mask)
        {
            return GetSiblings(address, mask, SiblingOptions.ExcludeUnusable);
        }

        /// <summary>Enumerates through the siblings of an <see cref="T:System.Net.IPAddress"/> in a network.</summary>
        /// <param name="address">The address</param>
        /// <param name="mask">The net mask of the network</param>
        /// <param name="options">Options which addresses to include an which not</param>
        public static IEnumerable<IPAddress> GetSiblings(this IPAddress address, NetMask mask, SiblingOptions options)
        {
            if (address == null)
                throw new ArgumentNullException("address");
            if (mask == null)
                throw new ArgumentNullException("mask");
            if (address.AddressFamily != Sockets.AddressFamily.InterNetwork)
                throw new NotSupportedException(OnlyIPv4Supported);

            bool includeSelf = BitHelper.IsOptionSet(options, SiblingOptions.IncludeSelf);
            bool includeBroadcast = BitHelper.IsOptionSet(options,SiblingOptions.IncludeBroadcast);
            bool includeNetworkIdentifier = BitHelper.IsOptionSet(options, SiblingOptions.IncludeNetworkIdentifier);

            throw new NotImplementedException();
        }

        /// <summary>Gets the network prefix of an <see cref="T:System.Net.IPAddress"/>.</summary>
        /// <param name="address">The address</param>
        /// <param name="mask">The net mask of the network</param>
        /// <returns>The network prefix of an <see cref="T:System.Net.IPAddress"/></returns>
        public static IPAddress GetNetworkPrefix(this IPAddress address, NetMask mask)
        {
            //if (address == null)
            //    throw new ArgumentNullException("address");
            //if (mask == null)
            //    throw new ArgumentNullException("mask");
            if (address.AddressFamily != Sockets.AddressFamily.InterNetwork)
                throw new NotSupportedException(OnlyIPv4Supported);

            return mask & address;
        }

        /// <summary>Gets the host identifier (rest) an <see cref="T:System.Net.IPAddress"/>.</summary>
        /// <param name="address">The address</param>
        /// <param name="mask">The net mask of the network</param>
        /// <returns>The host identifier (rest) an <see cref="T:System.Net.IPAddress"/></returns>
        public static IPAddress GetHostIdentifier(this IPAddress address, NetMask mask)
        {
            if (address == null)
                throw new ArgumentNullException("address");
            if (mask == null)
                throw new ArgumentNullException("mask");
            if (address.AddressFamily != Sockets.AddressFamily.InterNetwork)
                throw new NotSupportedException(OnlyIPv4Supported);

            var maskBits = mask.GetMaskBytes();
            var ipBits = address.GetAddressBytes();
            
            // ~Mask & IP
            var retVal = maskBits.Not().And(ipBits);
            var bytes = new byte[NetMask.MaskLength];
            Buffer.BlockCopy(retVal, 0, bytes, 0, bytes.Length);

            return new IPAddress(bytes);
        }
    }
}
#endif
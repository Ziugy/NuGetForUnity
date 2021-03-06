﻿namespace NugetForUnity
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a package available from NuGet.
    /// </summary>
    public class NugetPackage : NugetPackageIdentifier, IEquatable<NugetPackage>, IEqualityComparer<NugetPackage>
    {
        /// <summary>
        /// Gets or sets the title (not ID) of the package.  This is the "friendly" name that only appears in GUIs and on webpages.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description of the NuGet package.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the release notes of the NuGet package.
        /// </summary>
        public string ReleaseNotes { get; set; }

        /// <summary>
        /// Gets or sets the URL for the location of the license of the NuGet package.
        /// </summary>
        public string LicenseUrl { get; set; }

        /// <summary>
        /// Gets or sets the URL for the location of the actual (.nupkg) NuGet package.
        /// </summary>
        public string DownloadUrl { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="NugetPackageSource"/> that contains this package.
        /// </summary>
        public NugetPackageSource PackageSource { get; set; }

        /// <summary>
        /// Gets or sets the icon for the package as a <see cref="UnityEngine.Texture2D"/>. 
        /// </summary>
        public UnityEngine.Texture2D Icon { get; set; }

        /// <summary>
        /// Gets or sets the NuGet packages that this NuGet package depends on.
        /// </summary>
        public List<NugetPackageIdentifier> Dependencies { get; set; }

        /// <summary>
        /// Checks to see if this <see cref="NugetPackage"/> is equal to the given one.
        /// </summary>
        /// <param name="other">The other <see cref="NugetPackage"/> to check equality with.</param>
        /// <returns>True if the packages are equal, otherwise false.</returns>
        public bool Equals(NugetPackage other)
        {
            return other.Id == Id && other.Version == Version;
        }

        /// <summary>
        /// Creates a new <see cref="NugetPackage"/> from the given <see cref="NuspecFile"/>.
        /// </summary>
        /// <param name="nuspec">The <see cref="NuspecFile"/> to use to create the <see cref="NugetPackage"/>.</param>
        /// <returns>The newly created <see cref="NugetPackage"/>.</returns>
        public static NugetPackage FromNuspec(NuspecFile nuspec)
        {
            NugetPackage package = new NugetPackage();

            package.Id = nuspec.Id;
            package.Version = nuspec.Version;
            package.Title = nuspec.Title;
            package.Description = nuspec.Description;
            package.ReleaseNotes = nuspec.ReleaseNotes;
            package.LicenseUrl = nuspec.LicenseUrl;
            //package.DownloadUrl = not in a nuspec

            if (!string.IsNullOrEmpty(nuspec.IconUrl))
            {
                package.Icon = NugetHelper.DownloadImage(nuspec.IconUrl);
            }

            // if there is no title, just use the ID as the title
            if (string.IsNullOrEmpty(package.Title))
            {
                package.Title = package.Id;
            }

            package.Dependencies = nuspec.Dependencies;

            return package;
        }

        /// <summary>
        /// Loads a <see cref="NugetPackage"/> from the .nupkg file at the given filepath.
        /// </summary>
        /// <param name="nupkgFilepath">The filepath to the .nupkg file to load.</param>
        /// <returns>The <see cref="NugetPackage"/> loaded from the .nupkg file.</returns>
        public static NugetPackage FromNupkgFile(string nupkgFilepath)
        {
            NugetPackage package = FromNuspec(NuspecFile.FromNupkgFile(nupkgFilepath));
            package.DownloadUrl = nupkgFilepath;
            return package;
        }

        /// <summary>
        /// Checks to see if the two given <see cref="NugetPackage"/>s are equal.
        /// </summary>
        /// <param name="x">The first <see cref="NugetPackage"/> to compare.</param>
        /// <param name="y">The second <see cref="NugetPackage"/> to compare.</param>
        /// <returns>True if the packages are equal, otherwise false.</returns>
        public bool Equals(NugetPackage x, NugetPackage y)
        {
            return x.Id == y.Id && x.Version == y.Version;
        }

        /// <summary>
        /// Gets the hashcode for the given <see cref="NugetPackage"/>.
        /// </summary>
        /// <returns>The hashcode for the given <see cref="NugetPackage"/>.</returns>
        public int GetHashCode(NugetPackage obj)
        {
            return obj.Id.GetHashCode() ^ obj.Version.GetHashCode();
        }
    }
}
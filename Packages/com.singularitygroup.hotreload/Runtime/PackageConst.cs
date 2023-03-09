namespace SingularityGroup.HotReload {
    internal static class PackageConst {
        public const string Version = "1.6.5";
        // Never higher than Version
        // Used for the download
        public const string ServerVersion = "1.6.3";
        public const string PackageName = "com.singularitygroup.hotreload";
        public const string LibraryCachePath = "Library/" + PackageName;
        public const string ConfigFileName = "hot-reload-config.json";
    }
}

namespace Backend3DForge.Services.FileStorage
{
	public abstract class DefaultConfiguration
	{
		public abstract string AvatarStoragePath { get; set; }
		public abstract string PathToFilesToPrint { get; set; }
		public abstract string PathToPreviewFiles { get; set; }
		public abstract string PathTo3DModelsPictures { get; set; }
	}
}

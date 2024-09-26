namespace Social_Media_APILayer.Repository
{
	public class PostRepo
	{
		public enum enMediaTypes { None = 1, Image = 2, Video = 3 }

		public static string GetMediaType(enMediaTypes enMediaType)
		{
			switch (enMediaType)
			{
				case enMediaTypes.None: 
					return "None";
				case enMediaTypes.Image: 
					return "Image";
				case enMediaTypes.Video: 
					return "Video";
				default:
					return "None";
			}
		}
	}
}

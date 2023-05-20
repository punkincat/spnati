using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using SPNATI_Character_Editor.DataStructures;

namespace SPNATI_Character_Editor
{
	public static class LiveImageCache
	{
		private static Dictionary<string, Bitmap> _images = new Dictionary<string, Bitmap>();

		public static void Clear()
		{
			foreach (Bitmap img in _images.Values)
			{
				img.Dispose();
			}
			_images.Clear();
		}

		public static void Refresh()
		{
			string[] keys = _images.Keys.ToArray();
			foreach (string key in keys)
			{
				_images.Remove(key);
				Bitmap img = null;

				string path = Path.Combine(Config.SpnatiDirectory, key);
				if (File.Exists(path))
				{
					try
					{
						using (Bitmap temp = new Bitmap(path))
						{
							img = new Bitmap(temp);
							_images[key] = img;
						}
					}
					catch { }
				}
			}
		}

		public static Bitmap Get(string src, ISkin skin, int? stage = null)
        {
			return Get(ImagePath.ParseAnyPath(src, skin), skin, stage);
        }

		public static Bitmap Get(ImagePath src, ISkin skin, int? stage = null)
		{
			if (ImagePath.IsNullOrEmpty(src)) { return null; }
			Debug.Assert(skin != null);

			string relPath = src.AsRelativeFilesystemPath(skin, stage);

			Bitmap img = null;
			if (_images.TryGetValue(relPath, out img))
			{
				return img;
			}

			string path = Path.Combine(Config.SpnatiDirectory, relPath);
			if (!File.Exists(path))
			{
				return null;
			}
			try
			{
				using (Bitmap temp = new Bitmap(path))
				{
					img = new Bitmap(temp);
					_images[relPath] = img;
				}
			}
			catch { }
			return img;
		}
	}
}

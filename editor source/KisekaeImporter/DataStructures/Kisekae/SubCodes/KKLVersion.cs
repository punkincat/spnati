namespace KisekaeImporter.SubCodes
{
	public class KKLVersion : KisekaeSubCode
	{
		public KKLVersion() : base("fv") { }

		public int MinorVersion
		{
			get { return GetInt(0); }
			set { Set(0, value.ToString()); }
		}

		public int AlphaVersion
		{
			get { return GetInt(1); }
			set { Set(1, value.ToString()); }
		}
	}
}

using KisekaeImporter.SubCodes;

namespace KisekaeImporter
{
	public class KisekaeModInfo : KisekaeComponent
	{
		public KisekaeModInfo() : base()
		{
			/* NOTE: Subcode "fv" is exported as part of both scene and character data. */
		}

		[KisekaeSubCode("fv")]
		public KKLVersion Version
		{
			get { return GetSubCode<KKLVersion>("fv"); }
			set { SetSubCode("fv", value); }
		}
	}
}

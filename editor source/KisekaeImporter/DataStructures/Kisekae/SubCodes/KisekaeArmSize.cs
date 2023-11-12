namespace KisekaeImporter.SubCodes
{
	public class KisekaeArmSize : KisekaeSubCode
	{
		public KisekaeArmSize() : base("cc") { }

		public int LeftArmScaleX
		{
			get { return GetInt(0); }
			set { Set(0, value.ToString()); }
		}

		public int LeftArmScaleY
		{
			get { return GetInt(1); }
			set { Set(1, value.ToString()); }
		}

		public int LeftArmOffsetX
		{
			get { return GetInt(2); }
			set { Set(2, value.ToString()); }
		}

		public int LeftArmOffsetY
		{
			get { return GetInt(3); }
			set { Set(3, value.ToString()); }
		}

		public int RightArmScaleX
		{
			get { return GetInt(4); }
			set { Set(4, value.ToString()); }
		}

		public int RightArmScaleY
		{
			get { return GetInt(5); }
			set { Set(5, value.ToString()); }
		}

		public int RightArmOffsetX
		{
			get { return GetInt(6); }
			set { Set(6, value.ToString()); }
		}

		public int RightArmOffsetY
		{
			get { return GetInt(7); }
			set { Set(7, value.ToString()); }
		}
	}
}
